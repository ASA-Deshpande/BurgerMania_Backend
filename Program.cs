using BurgerMania.Data;
using BurgerMania.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

namespace BurgerMania
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.Configure<JwtConfig>
                (builder.Configuration.GetSection("JwtConfig"));

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.MapInboundClaims = false;
                    var jwtConfig = builder.Configuration.GetSection("JwtConfig").Get<JwtConfig>();

                    options.TokenValidationParameters = new
                    TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtConfig.Issuer,
                        ValidAudience = jwtConfig.Audience,
                        IssuerSigningKey = new
                        SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Secret))
                    };

                });


            builder.Services.AddDbContext<BurgerManiaContext>
                (options => options.UseSqlServer(builder.Configuration.GetConnectionString("cs")));

            // Define the CORS policy name
            const string policyName = "_myAllowSpecificOrigins";

            // Add services to the container
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: policyName, builder =>
                {
                    builder.WithOrigins("http://127.0.0.1:5500") // Replace with your frontend URL
                           .AllowAnyMethod() // Allow any HTTP method (GET, POST, etc.)
                           .AllowAnyHeader() // Allow any header
                           .AllowCredentials(); // Allow credentials (cookies, authorization headers, etc.)
                });
            });

            builder.Services.AddControllers();

            //builder.Services.AddControllers()
            //    .AddJsonOptions(options =>
            //    {
            //        options.JsonSerializerOptions.ReferenceHandler =
            //        System.Text.Json.Serialization.ReferenceHandler.Preserve;
            //    });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();

            builder.Services.AddSwaggerGen((options) =>
            {
                options.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme()
                    {
                        In = ParameterLocation.Header,
                        Description = "Please enter a valid token",
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        BearerFormat = "JWT",
                        Scheme = "Bearer"
                    });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                                            new OpenApiSecurityScheme()
                    {
                        Reference = new OpenApiReference()
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[]
                    {

                    }
                    }

                });
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Use the defined CORS policy
            app.UseCors(policyName);
            //app.UseCors(configurePolicy => { configurePolicy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
            //app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
