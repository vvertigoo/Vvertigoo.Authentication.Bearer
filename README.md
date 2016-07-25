# Vvertigoo.Authentication.Bearer

### How to use:

* Add this project as refence to your project.
* In your Startup.cs add this lines:
```c#
using Vvertigoo.Authentication.Bearer;
...

public void ConfigureServices(IServiceCollection services)
{
  ...
  // Replace ApplicationUser with your user type
  services.AddScoped<BearerSignInManager<ApplicationUser>, BearerSignInManager<ApplicationUser>>();
  ...
}

public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
{
  ...
  app.UseBearerAuthentication();
  ...
}
```

* Now you can inject `BearerSignInManager<T>` to your controller:
```c#
using Vvertigoo.Authentication.Bearer;
...

public class MyController : Controller
{
  private readonly BearerSignInManager<ApplicationUser> _signInManager;

  public MyController(BearerSignInManager<ApplicationUser> signInManager)
  {
    _signInManager = signInManager;
  }
}
```
* And use it to sign in your users:
```c#
public async Task<IActionResult> SignIn(string email, string password)
{
  var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == email);
  if (user == null) return BadRequest();

  var result = await _signInManager.PasswordSignInAsync(user, password);
  if (result == BearerSignInManagerResult.Success)
  {
    return Ok();
  }
  return BadRequest(result);
}
```

You will get token via response body. It contains JSON object with 4 fields: `issued`, `expires`, `expires_in` and `access_token`

After you receive token you can use it in your requests - put token into `Authorization` header and place `[Authorize]` attribute on your controller or action.



### Additional settings:
You can change token expire time and set HTTPS usage by adding `BearerAuthenticationOptions` object to `app.UseBearerAuthentication();`:
```c#
app.UseBearerAuthentication(new BearerAuthenticationOptions {
  BearerSecure = BearerSecureOption.Always, // HTTPS only token response
  ExpireTimeSpan = System.TimeSpan.FromDays(10) // any timespan, default is .FromDays(14)
});
```

### License
[Apache 2.0](http://www.apache.org/licenses/LICENSE-2.0) © Vadym Prosianiuk
