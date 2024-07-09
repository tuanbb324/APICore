using ACBSChatbotConnector.Hubs;
using ACBSChatbotConnector.Repositories;
using ACBSChatbotConnector.Services;
using ACBSChatbotConnector.Services.Implement;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
//Add support to logging with SERILOG
Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("Logs/application-log.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.Seq("http://seq:5341")
                .CreateLogger();

// Add services to the container.
ConfigurationManager configuration = builder.Configuration;
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
}).ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "_allowAll",
                      builder =>
                      {
                          builder
                            .WithOrigins("*")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                      });
});
builder.Configuration.AddJsonFile("appsettings.json", optional: false);
builder.Services.AddMemoryCache();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        var tokenService = context.HttpContext.RequestServices.GetRequiredService<ITokenService>();
                        var token = context.SecurityToken as JwtSecurityToken;
                        //if (token != null && await tokenService.IsTokenBlacklisted(token.RawData))
                        //{
                        //    context.Fail("This token is blacklisted.");
                        //}
                    },
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        // If the request is for our hub...
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) &&
                            (path.StartsWithSegments("/chat") || path.StartsWithSegments("/chatbot")))
                        {
                            // Read the token out of the query string
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                    ClockSkew = TimeSpan.Zero
                };
            })
            .AddCookie(options =>
            {
                options.Cookie.Name = "accessToken";
                options.LoginPath = "/api/auth/login";
                options.LogoutPath = "/api/auth/revoke";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
            }); ;
builder.Services.AddSignalR(e =>
{
    e.MaximumReceiveMessageSize = 102400000;
    e.EnableDetailedErrors = true;
});

builder.Services.AddMemoryCache();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IDapperDA, DapperDA>();
builder.Services.AddScoped<ICMS_UserService, CMS_UserService>();
builder.Services.AddScoped<IFeatureService, FeatureService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IFileUploadService, FileUploadService>();
builder.Services.AddScoped<IProducerService, ProducerService>();
builder.Services.AddScoped<IChatMessageService, ChatMessageService>();
builder.Services.AddSingleton<IRedisService, RedisService>();
builder.Services.AddSingleton<IGroupService, GroupService>();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseRouting();

app.UseStaticFiles();
app.UseSwagger();
app.UseCors("_allowAll");
app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "ACBS server"));
app.UseAuthorization();
app.MapControllers();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<ChatHub>("/chat");
});
app.Run();
