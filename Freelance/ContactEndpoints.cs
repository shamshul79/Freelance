using Microsoft.EntityFrameworkCore;
using Freelance.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
namespace Freelance;

public static class ContactEndpoints
{
    public static void MapContactEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Contact").WithTags(nameof(Contact));

        group.MapGet("/", async (FreelanceContext db) =>
        {
            return await db.Contact.ToListAsync();
        })
        .WithName("GetAllContacts")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Contact>, NotFound>> (int id, FreelanceContext db) =>
        {
            return await db.Contact.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Contact model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetContactById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Contact contact, FreelanceContext db) =>
        {
            var affected = await db.Contact
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.Id, contact.Id)
                  .SetProperty(m => m.UserName, contact.UserName)
                  .SetProperty(m => m.Email, contact.Email)
                  .SetProperty(m => m.PhoneNo, contact.PhoneNo)
                  .SetProperty(m => m.skillsets, contact.skillsets)
                  .SetProperty(m => m.hobby, contact.hobby)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateContact")
        .WithOpenApi();

        group.MapPost("/", async (Contact contact, FreelanceContext db) =>
        {
            db.Contact.Add(contact);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Contact/{contact.Id}",contact);
        })
        .WithName("CreateContact")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, FreelanceContext db) =>
        {
            var affected = await db.Contact
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteContact")
        .WithOpenApi();
    }
}
