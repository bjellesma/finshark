using api.Data;
using api.Interfaces;
using api.Repositories;
using api.Repository;
using Microsoft.EntityFrameworkCore;

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

app.MapControllers();

// origin of app
app.Run();
