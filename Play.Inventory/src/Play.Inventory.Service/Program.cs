
using Play.Common.MongoDB;
using Play.Inventory.Service.Entities;
using Play.Inventory.Service.Client;
using Polly.Timeout;
using Polly;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMongo().AddMongoRepository<InventoryItem>("inventoryitems");



////concerns the communication with another microservice
Random jitterer = new Random();
builder.Services.AddHttpClient<CatalogClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5001");
})
.AddTransientHttpErrorPolicy(builder => builder.Or<TimeoutRejectedException>().WaitAndRetryAsync(
    5,
    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) + TimeSpan.FromMilliseconds(jitterer.Next(0, 1000)),
    onRetry: (outcome, timespan, retryAttempt) =>
    {
        // var serviceProvider = app.Services;
        // serviceProvider.GetService<ILogger<CatalogClient>>()?
        //             .LogWarning($"Delaying for {timespan.TotalSeconds} seconds, then making retyr {retryAttempt}");
    }
))
.AddTransientHttpErrorPolicy(builder => builder.Or<TimeoutRejectedException>().CircuitBreakerAsync(
    3,
    TimeSpan.FromSeconds(15),
    onBreak: (outcome, timespan) =>
    {
        // var serviceProvider = app.Services;
        // serviceProvider.GetService<ILogger<CatalogClient>>()?
        //             .LogWarning($"Opening the circuit for {timespan.TotalSeconds} seconds...");
    },
    onReset: () =>
    {
        // var serviceProvider = app.Services;
        // serviceProvider.GetService<ILogger<CatalogClient>>()?
        //             .LogWarning($"Closing the circuit...");
    }
))
.AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(1));

    var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.Run();
