using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SisandBackend.Entities.Users;
using SisandBackend.EntityFrameworkCore.Contexts;
using SisandBackend.EntityFrameworkCore.Repositories.UserRepository;
using SisandBackend.Services.AuthService;
using System.Text;

namespace SisandBackend;

public class Startup
{
    #region Properties
    public IConfiguration Configuration { get; }
    #endregion

    #region Constructors
    public Startup(IConfiguration configuration) => Configuration = configuration;
    #endregion

    #region Methods
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseCors("CorsPolicy");
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints => endpoints.MapControllers());

        app.UseHttpsRedirection();
    }

    public void ConfigureServices(IServiceCollection services)
    {
        #region AutoMapper
        var mapperConfig = new MapperConfiguration(mc => mc.AddMaps(AppDomain.CurrentDomain.GetAssemblies()));
        services.AddSingleton(mapperConfig.CreateMapper());
        #endregion

        #region Identity
        services.AddIdentity<User, IdentityRole<long>>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
        })
        .AddEntityFrameworkStores<SisandDbContext>()
        .AddDefaultTokenProviders();
        #endregion

        #region Jwt 
        var key = Encoding.ASCII.GetBytes(Configuration["Jwt:Key"]);
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = Configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                };
            });
        #endregion

        #region CORS
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            {
                builder.WithOrigins("http://localhost:4200", "https://localhost:4200")
                       .AllowAnyHeader()
                       .AllowAnyMethod();
            });
        });
        #endregion

        #region Swagger
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "SisandBackendAPI", Version = "v1" });

            var securityScheme = new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Insira o token JWT",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            };

            options.AddSecurityDefinition("Bearer", securityScheme);
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { securityScheme, new string[] { } }
            });
        });
        #endregion

        #region Aditional Services
        services.AddDbContext<SisandDbContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("Default")));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAuthService, AuthService>();

        services.AddHttpClient();
        services.AddHttpContextAccessor();
        services.AddDistributedMemoryCache();
        services.AddLogging();
        services.AddControllers();
        #endregion
    }
    #endregion
}
