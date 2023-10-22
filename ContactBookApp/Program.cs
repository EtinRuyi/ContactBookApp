
using CloudinaryDotNet;
using ContactBookApp.Commons.Validations;
using ContactBookApp.Core.Services.Abstractions;
using ContactBookApp.Core.Services.Implementations;
using ContactBookApp.Data;
using ContactBookApp.Model.Entity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Cryptography.Xml;
using System.Text;

namespace ContactBookApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            //Configure Database Connection
            builder.Services.AddDbContext<ContactBookAppDbContext>(Options => Options.UseSqlServer(builder.Configuration
               .GetConnectionString("Connection")));


            builder.Services.AddEndpointsApiExplorer();

            //Configure Swagger to use JWTokens
            builder.Services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo { Title = "ContactBook App", Version = "v1" });
                config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme 
                {
                    In = ParameterLocation.Header,
                    Description = "Insert Token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "Jwt",
                    Scheme = "Bearer"
                });

                config.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });


            //Configure all Dependenccy Injections in the project
            builder.Services.AddScoped<IAuthentication, Authentication>();
            builder.Services.AddScoped<IUserServices, UserServices>();
            builder.Services.AddScoped<IContactServices, ContactServices>();
            builder.Services.AddScoped<UserValidator>();

            //Configure IdentityRole to Users
            builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ContactBookAppDbContext>()
                .AddDefaultTokenProviders();

            //Configure Cloudinary Service Injection and all Cloudinary settings
            builder.Services.AddSingleton<ICloudinaryService, CloudinaryService>();
            var cloudinaryAccount = new Account
                (
                    "dkbs9kfz8",
                    "631333689795714",
                    "WxyFvwLVM3PJSjhx0JMzsc7ryT8"
                );
            var cloudinary = new Cloudinary(cloudinaryAccount);
            builder.Services.AddSingleton(cloudinary);

            //Configure JWTOKENS Authentication Options
            var jwtsetting = builder.Configuration.GetSection("Jwtsettings");
            var key = Encoding.ASCII.GetBytes(jwtsetting["secret"]);
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                    };
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CB-V1");
                });
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}