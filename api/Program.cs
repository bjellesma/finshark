using api.Data;
using api.Interfaces;
using api.Models;
using api.Repositories;
using api.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// the controllers service will automatically discover any class with controller as the suffix
builder.Services.AddControllers();
// api explorer and swaggergen are used to allow the app to generate documentation based on api endpoints
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// The entity framework will commonly run into circular references with Navigational Properties
// Newtonsoft helps to ensure that the JSON output is clean
builder.Services.AddControllers().AddNewtonsoftJson(options => {
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

// builder allowing us to connect to the sql server
builder.Services.AddDbContext<ApplicationDBContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// add service for user signup
builder.Services.AddIdentity<AppUser, IdentityRole>(options => {
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 4;
}).AddEntityFrameworkStores<ApplicationDBContext>();

// add schemes for authentication user with JWT
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme =
    options.DefaultChallengeScheme =
    options.DefaultScheme =
    options.DefaultSignInScheme =
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters{
        ValidateIssuer = true,
        // all of these JWT configs are from appsettings.json
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])
        )
    };

});

// registers the stockrepository as an instance of the istockrepository interface
// scoped means that a new instance is generated for each user ensuring that requests are isolated
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// for login
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// origin of app
app.Run();
