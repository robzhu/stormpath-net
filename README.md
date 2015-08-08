# stormpath-net
A .net client for Stormpath (identity and access management service)

# authenticate a user
```
var apiKeyId = "your_key_here";
var apiKeySecret = "your_secret_here";
var appUrl = "https://api.stormpath.com/v1/applications/{your_app_id}";

var client = new Stormpath.Client( apiKeyId, apiKeySecret );
var app = await client.GetApplicationAsync( appUrl );

//username and password for the user you want to authenticate
var username = "johndoe";
var password = "Password1";

try
{
    var account = await app.AttemptLoginAsync( username, password );
    //you can use interact with Stormpath using the account object now
    Console.WriteLine( "login successful" );
}
catch( OperationErrorException )
{
    Console.WriteLine( "login failed" );
}
```
