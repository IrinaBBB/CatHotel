using Business.Repository.IRepository;
using Business.Repository;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using DataAccess.Data;
using CatHotel.Api.Helper;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Newtonsoft.Json.Serialization;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");
var configuration = builder.Configuration;

builder.Services.AddControllers()
    .AddJsonOptions(opt => opt.JsonSerializerOptions.PropertyNamingPolicy = null)
                .AddNewtonsoftJson(opt =>
                {
                    opt.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                }); ;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CatHotel.Api", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please Bearer and then token in the field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                   {
                     new OpenApiSecurityScheme
                     {
                       Reference = new OpenApiReference
                       {
                         Type = ReferenceType.SecurityScheme,
                         Id = "Bearer"
                       }
                      },
                      new string[] { }
                    }
                });
});
builder.Services.AddSingleton(sp => sp.GetRequiredService<ILoggerFactory>().CreateLogger("DefaultLogger"));
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IHotelRoomRepository, HotelRoomRepository>();
builder.Services.AddScoped<IAmenityRepository, AmenityRepository>();
builder.Services.AddScoped<IHotelImagesRepository, HotelImagesRepository>();
builder.Services.AddScoped<IRoomOrderDetailsRepository, RoomOrderDetailsRepository>();


var appSettingsSection = builder.Configuration.GetSection("APISettings");

var apiSettings = appSettingsSection.Get<APISettings>();
var key = Encoding.ASCII.GetBytes(apiSettings.SecretKey);

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidAudience = apiSettings.ValidAudience,
            ValidIssuer = apiSettings.ValidIssuer,
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.Configure<APISettings>(appSettingsSection);

builder.Services.AddCors(o => o.AddPolicy("CatHotel", builder =>
{
    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CatHotel");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
