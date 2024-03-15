using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Microsoft.Extensions.Configuration;
using System.Reflection.Metadata;
using MongoDB.Driver;
using Play.Catalog.Service.Entities;
using Play.Common.MongoDB;
using Play.Common.Settings;
using MassTransit;
using Play.Catalog.Service.Settings;
using System.Reflection;
using Play.Common.MassTransit;




var builder = WebApplication.CreateBuilder(args);

// BsonSerializer.RegisterSerializer(new GuidSerializer(MongoDB.Bson.BsonType.String));
// BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(MongoDB.Bson.BsonType.String));


ServiceSettings serviceSettings = builder.Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>()!;

// builder.Services.AddSingleton(serviceProvider => 
// {
//     var mongoDbSettings = builder.Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
//     var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
//     return mongoClient.GetDatabase(serviceSettings.ServiceName);
// });
// builder.Services.AddSingleton<IRepository<Item>>(
//     serviceProvider => {
//         var database = serviceProvider.GetService<IMongoDatabase>();
//         return new MongoRepository<Item>(database, "items");
// });
builder.Services.AddMongo().AddMongoRepository<Item>("items").AddMassTransitWithRabbitMq();


builder.Services.AddControllers(
    options =>
    {
        options.SuppressAsyncSuffixInActionNames = false;
    }
);




// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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
