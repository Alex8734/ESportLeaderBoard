using System.Net.Http.Json;
using System.Text;
using System.Threading.Channels;
using ESportsLeaderBoard.Model;
using static System.Net.WebClient;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var client = new HttpClient();
var url = "http://localhost:5001/";

var response = await client.GetAsync($"{url}Player");
var content = await response.Content.ReadFromJsonAsync<PlayerResponse[]>();
Console.WriteLine("Loading Players... Done!");
Thread.Sleep(2300);
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
    var leaderboardResponse = await client.GetAsync($"{url}Leaderboard/{game}");
    var leaderboard = await leaderboardResponse.Content.ReadFromJsonAsync<LeaderBoardResponse>();
    foreach (var player in content)
    {
        var score = rnd.Next(0, 5) + leaderboard.Players.FirstOrDefault(p => p.Player.Name == player.Name)?.Score ?? 0;
        var resp = await client.PostAsync($"{url}Leaderboard/{game}/{player.Name}",
            JsonContent.Create(new JsonSingleOutput<int>(score)));
        Console.WriteLine($"Posting {score}, {player.Name} to {game}... {resp.StatusCode} - {await resp.Content.ReadAsStringAsync()}");
    }
    Thread.Sleep(1000);
}