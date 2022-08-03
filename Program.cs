using Microsoft.OpenApi.Models;
using PizzaStore.Routes;
using PizzaStore.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Pizzas") ?? "Data Source=Pizza.db";
// builder.Services.AddDbContext<PizzaDb>(options => options.UseInMemoryDatabase("items"));
builder.Services.AddSqlite<PizzaDb>(connectionString);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
  {
      c.SwaggerDoc("v1", new OpenApiInfo { Title = "A pizza Minimal API", Description = "Yum yum", Version = "v1" });
  });


var app = builder.Build();

app.SwaggerEndpoint();
app.MapPizzaEndpoints();

app.Run();