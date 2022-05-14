# Backend for budget app

dotnet run to run the application.  
Make sure to add connection strings and other environment variables before running.

PostgreSql is used as the database for user accounts management.  
MongoDB is the database for the budget data.

Provide the appropriate connection string for both database as user secrets or in environment variables.

## You can view a running example of the project [here](https://budget-6e9c16f0.herokuapp.com/)

It's deployed through docker on Heroku's free dynos and uses Heroku postgres and Mongodb atlas.

Use postman or other API tools to access the endpoints.  
Swagger endpoint is available at [/swagger](https://budget-6e9c16f0.herokuapp.com/swagger/index.html) but not recommended as it provides negligible control over requests. You can use it as a reference to structure your requests.

To Register yourself, please provide your name, email and password in json format. Please make sure you remember your password since I haven't implemented reset password yet.

You'll need to authorize with an access token for every data request. Access tokens and refresh tokens can be obtained by [logging in](https://budget-6e9c16f0.herokuapp.com/api/authenticate/login) or by using the [guest](https://budget-6e9c16f0.herokuapp.com/api/authenticate/guest) option.  
You can use the refresh token and your expired access token to get a new access token.  
While making requests for data, provide the access token as jwt bearer token in the authorization header.

Basic healthchecks are available at

- [/health/mongo](https://budget-6e9c16f0.herokuapp.com/health/mongo) for MongoDB Atlas connection
- [/health/postgres](https://budget-6e9c16f0.herokuapp.com/health/postgres) for Heroku Postgres connection
- [/health/live](https://budget-6e9c16f0.herokuapp.com/health/ready) to see if the application is running

### TODO:

- [x] Add health checks
- [ ] Add endpoints to get user's budget items formatted by year/month
- [ ] Add option to create categories and subcategories
- [ ] Implement password reset feature
