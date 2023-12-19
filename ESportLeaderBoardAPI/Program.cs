using System.Text.Json.Serialization;
using ESportLeaderBoardAPI;
using Microsoft.AspNetCore.Http.Json;

const string Policy = "AllowOrigin";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});;
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
app.Run();

