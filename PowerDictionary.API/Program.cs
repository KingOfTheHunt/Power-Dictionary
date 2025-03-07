using PowerDictionary.API.Exceptions;
using PowerDictionary.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("v1/{word}", async (string word) =>
    {
        var scope = app.Services.CreateScope();
        var httpClient = scope.ServiceProvider.GetRequiredService<HttpClient>();
        var wordService = new WordService(httpClient);

        try
        {
            var result = await wordService.GetWordDescription(word);

            return Results.Ok(new { status = true, data = result });
        }
        catch (WordNotFoundException e)
        {
            return Results.NotFound(new { status = false, data = e.Message });
        }
        catch (Exception e)
        {
            return Results.BadRequest(new { status = false, data = e.Message });
        }
    })
    .WithTags("Word")
    .WithDescription("Obtém as informações da palavra desejada.");

app.Run();