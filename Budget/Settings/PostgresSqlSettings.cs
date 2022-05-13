namespace Budget.Settings
{
    public class PostgresSqlSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Database { get; set; }

        public string ConnectionString
        {
            get
            {
                return $"Server={Host};Database={Database};Port={Port};User Id={Username};Password={Password}";
            }
        }
    }
}
