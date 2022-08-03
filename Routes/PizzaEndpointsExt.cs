using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
// using PizzaStore.DB;
using PizzaStore.Models;

namespace PizzaStore.Routes
{
    public static class PizzaEndpointsExt
    {
        public static void MapPizzaEndpoints(this WebApplication app)
        {
            app.MapGet("/", () => "Hello World!");

            // app.MapGet("/pizzas", () => PizzaDB.GetPizzas());

            // app.MapGet("/pizzas/{id}", (int id) => PizzaDB.GetPizza(id));

            // app.MapPost("/pizzas", (Pizza pizza) => PizzaDB.CreatePizza(pizza));

            // app.MapPut("/pizzas", (Pizza pizza) => PizzaDB.UpdatePizza(pizza));

            // app.MapDelete("/pizzas/{id}", (int id) => PizzaDB.RemovePizza(id));

            app.MapGet("/pizzas", async (PizzaDb db) => await db.Pizzas.ToListAsync());

            app.MapGet("/pizza/{id}", async (PizzaDb db, int id) => await db.Pizzas.FindAsync(id));

            app.MapPost("/pizza", async (PizzaDb db, Pizza pizza) =>
            {
                await db.Pizzas.AddAsync(pizza);
                await db.SaveChangesAsync();
                return Results.Created($"/pizza/{pizza.Id}", pizza);
            });

            app.MapPut("/pizza/{id}", async (PizzaDb db, Pizza updatepizza, int id) =>
            {
                var pizza = await db.Pizzas.FindAsync(id);
                if (pizza is null) return Results.NotFound();
                pizza.Name = updatepizza.Name;
                pizza.Description = updatepizza.Description;
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            app.MapDelete("/pizza/{id}", async (PizzaDb db, int id) => 
            {
                var pizza = await db.Pizzas.FindAsync(id);
                if (pizza is null) return Results.NotFound();
                db.Pizzas.Remove(pizza);
                await db.SaveChangesAsync();
                return Results.Ok();
            } );
        }

        public static void SwaggerEndpoint(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
          {
              c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API V1");
          });
        }
    }
}