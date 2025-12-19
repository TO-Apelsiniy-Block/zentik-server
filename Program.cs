var builder = WebApplication.CreateBuilder(args);

// Add controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/sse", async (HttpContext context) =>
{
    try
    {
        context.Response.Headers.ContentType = "text/event-stream";
        while (true)
        {
            await context.Response.WriteAsync($"data: {123}\n\n");
            await context.Response.Body.FlushAsync();
            await Task.Delay(TimeSpan.FromSeconds(3));
            Console.WriteLine("12321");
        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        throw;
    }

});

app.Run();




