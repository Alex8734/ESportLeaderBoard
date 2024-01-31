using System.Text.Json.Serialization;
using ESportLeaderBoard.Model;
using ESportLeaderBoard.Model.Interfaces;
using ESportLeaderBoardAPI;
using ESportLeaderBoardAPI.Hubs;
using Microsoft.AspNetCore.Http.Json;

const string Policy = "AllowOrigin";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});;
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR()
    .AddJsonProtocol(options =>
    {
        options.PayloadSerializerOptions.Converters
            .Add(new JsonStringEnumConverter());
    });
builder.Services.Configure<JsonOptions>(o => o.SerializerOptions.Converters
    .Add(new JsonStringEnumConverter()));

builder.Services.AddCors(c =>
{
    c.AddPolicy(Policy, options =>
    {
        options.AllowAnyOrigin();
        options.AllowAnyHeader();
        options.AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors(Policy);
app.MapHub<LeaderBoardHub>(LeaderBoardConfig.Route);
app.Run();

 