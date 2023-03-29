using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
var configuration = app.Configuration;
ProductRepository.Init(configuration);

app.MapPost("/product", (Product products) =>
{
    ProductRepository.Add(products);
    return Results.Created($"/product/{products.Code}", $"{products.Code} - {products.Name}" );
});

app.MapGet("/product/{code}", ([FromRoute] string code) => 
{
    var product = ProductRepository.GetBy(code);
    if(product != null)
        return Results.Ok(product);
    else
        return Results.NotFound();
});

app.MapPut("/product", (Product product) =>
{
    var productSaved = ProductRepository.GetBy(product.Code);
    productSaved.Name = product.Name;
    return Results.Ok();
});

app.MapDelete("/product/{code}", ([FromRoute] string code) =>
{
    var productSaved = ProductRepository.GetBy(code);
    ProductRepository.Remove(productSaved);
    return Results.Ok();
});

app.MapGet("/configuration/database", (IConfiguration configuration) =>
{
    return Results.Ok($"{configuration["database:conection"]}/{configuration["database:port"]}");
});

app.Run();