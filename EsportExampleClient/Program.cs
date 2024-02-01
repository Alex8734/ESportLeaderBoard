using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Channels;
using ESportLeaderBoard.Model;
using ESportLeaderBoard.Model.Interfaces;
using static System.Net.WebClient;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using TypedSignalR.Client;

Console.OutputEncoding = Encoding.UTF8;

var client = new HttpClient();
var url = "http://localhost:5001/";

var connection = new HubConnectionBuilder()
    .AddJsonProtocol(c => c.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
    .WithUrl($"{url}{LeaderBoardConfig.Route}")
    .Build();

await connection.StartAsync();

var hubProxy = connection.CreateHubProxy<ILeaderBoardHub>();

using var subscription = connection.Register<ILeaderBoardClient>(new LeaderBoardClient());


var response = await client.GetAsync($"{url}Player");
var content = await response.Content.ReadFromJsonAsync<PlayerResponse[]>();

if(content is not null)
{
    var cols = 7;
    var pPerCol = content.Length / cols;
    for (int i = 0; i <= pPerCol; i++)
    {
        var builder = new StringBuilder();
        for (int j = 0; j <= cols; j++)
        {
            builder.Append($"{pPerCol * j + i:000} {content.ElementAtOrDefault(pPerCol * j + i )?.Name ?? " ",-30}");
        }
        Console.WriteLine(builder.ToString());
    }
}

var rnd = new Random();
var games = Enum.GetValues<Game>();
while (true)
{
    var game = games[rnd.Next(games.Length)];
    var startIndex = rnd.Next(0, content.Length);

    // Generate a random length for the range.
    var length = rnd.Next(1, content.Length - startIndex + 1);

    foreach (var player in content.Skip(startIndex).Take(length))
    {
        
        var score = rnd.Next(0, 5) + (LeaderBoardClient.LeaderBoard?.Players.FirstOrDefault(p => p.Player.Name == player.Name)?.Score ?? 0);
        await hubProxy.SendScore(new NameScoreResponse
        {
            Name = player.Name,
            Score = score
        }, game);
        Console.WriteLine($"[↑] Posting {score}, {player.Name} to {game}...");

    }
    Thread.Sleep(1000);
}

Console.ReadKey();

sealed class LeaderBoardClient : ILeaderBoardClient
{
    public static LeaderBoardResponse LeaderBoard { get; private set; }
    
    public Task ReceiveLeaderBoard(LeaderBoardResponse leaderBoard)
    {
        Console.WriteLine($"[↓] Got {leaderBoard.Game} LeaderBoard!");
        LeaderBoard = leaderBoard;
        return Task.CompletedTask;
    }

    public Task ReceiveMessage(string text)
    {
        Console.Error.WriteLine($"[\u2193] Failed by getting LeaderBoard!");
        return Task.CompletedTask;
    }
}