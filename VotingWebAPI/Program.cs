using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Voting.DB;
using Voting.Host.Auth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var parentDirPath = Directory.GetParent(Environment.CurrentDirectory).FullName;
builder.Services.AddDbContext<IVotingDBContext, VotingDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString").Replace("{BasePath}", parentDirPath)));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthentication("BasicAuthentication").AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
builder.Services.Configure<BasicAuthenticationOptions>(options => {
    options.CredentialValidator = (username, password) => {
        return username == "admin" && password == "password";
    };
});

builder.Services.AddCors(options => {
    options.AddPolicy("AllowAllHeaders", builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
});

//Configure Swagger
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Voting API", Version = "v1" });

    // Configure Swagger to use Basic Authentication
    c.AddSecurityDefinition("Basic", new OpenApiSecurityScheme {
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        Description = "Basic Authentication"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Basic"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors("AllowAllHeaders");
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers().RequireAuthorization(); // Require authentication for all endpoints

    // If using Swagger UI with Swashbuckle
    endpoints.MapSwagger(); // Example, adjust to your actual Swagger route
});
app.UseHttpsRedirection();
app.MapControllers();
app.Run();