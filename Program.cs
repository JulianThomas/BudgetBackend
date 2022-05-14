using Budget.Authentication;
using Budget.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;

BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
BsonSerializer.RegisterSerializer(new DateTimeSerializer(BsonType.String));
// Add services to the container.
builder.Services.AddSingleton<IUsersRepository, MongoDBItemsRepository>();
//var mongoSettings = configuration
//    .GetSection(nameof(MongoDBSettings))
//    .Get<MongoDBSettings>();
var mongoSettings = MongoClientSettings
    .FromConnectionString(configuration["MongoDBConnStr"]);
mongoSettings.ServerApi = new ServerApi(ServerApiVersion.V1);
builder.Services.AddSingleton<IMongoClient>(ServiceProvider =>
    new MongoClient(mongoSettings)
);


var uri = new Uri(configuration["PostgresConnStr"]);

var username = uri.UserInfo.Split(':')[0];
var password = uri.UserInfo.Split(':')[1];
string npgconnstr = "Server=" + uri.Host + 
    "; Database="+ uri.AbsolutePath.Substring(1) + 
    "; Username="+ username + "; Password="+ password + 
    "; Port="+ uri.Port + "; SSL Mode=Require; Trust Server Certificate=true;";



builder.Services.AddDbContext<ApplicationDbContext>(
    options => {
        //var postgresSettings = configuration.GetSection(nameof(NpgSqlSettings))
        //     .Get<NpgSqlSettings>();
        //options.UseNpgsql(postgresSettings.ConnectionString);
        options.UseNpgsql(npgconnstr);
    }
    );
//builder.Services.AddDbContext<ApplicationDbContext>(
//    options => {
//        options.UseNpgsql(configuration["PostgresConnStr"]);
//    });


builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero,
            ValidAudience = configuration["JWT:ValidAudience"],
            ValidIssuer = configuration["JWT:ValidIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSecretKey"]))
        };
    });

builder.Services.AddHealthChecks()
    .AddMongoDb(
        configuration["MongoDBConnStr"], 
        name:"mongodb", 
        timeout:TimeSpan.FromSeconds(10),
        tags: new [] {"ready"}
    );


builder.Services.AddControllers(options => {
    options.SuppressAsyncSuffixInActionNames = false;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate=(check) =>check.Tags.Contains("ready"),
    ResponseWriter = async (context, report) =>
    {
        var result = JsonSerializer.Serialize(
            new
            {
                status = report.Status.ToString(),
                checks = report.Entries.Select(entry => new
                {
                    name = entry.Key,
                    status = entry.Value.Status.ToString(),
                    exception = entry.Value.Exception != null ? entry.Value.Exception.Message : "none",
                    duration = entry.Value.Duration.ToString(),
                })
            });
        context.Response.ContentType = MediaTypeNames.Application.Json;
        await context.Response.WriteAsync(result);
    }
});
app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = (_) => false 
});

app.Run();
