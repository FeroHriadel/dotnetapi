using API.Extensions;



var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAppServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();
app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
// app.UseAuthentication();
// app.UseAuthorization();
app.MapControllers();
app.Run();

