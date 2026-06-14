using Microsoft.EntityFrameworkCore;
using PokeBid.API.Data;
using PokeBid.API.Repositories;
using PokeBid.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PokeBidDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAuctionRepository, AuctionRepository>();
builder.Services.AddScoped<IAuctionService, AuctionService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<PokeBidDbContext>();
    
    if (!context.Users.Any())
    {
        context.Users.Add(new PokeBid.API.Models.User 
        { 
            Username = "kaito", 
            Email = "abc@pokemon.com", 
            PasswordHash = "haslo123" 
        });
        context.SaveChanges(); 
    }
}

app.Run(); 
app.Run();