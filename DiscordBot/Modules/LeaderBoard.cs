using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Text.Json;
using Discord;
using Discord.Commands;
using Discord.Utils;
using Discord.Interactions;
using Discord.Interactions.Builders;
using DiscordBot.Services;
using ESportLeaderBoard.Model;
using Microsoft.AspNetCore.Hosting;
using RequireUserPermissionAttribute = Discord.Commands.RequireUserPermissionAttribute;
using Game = ESportLeaderBoard.Model.Game;

namespace DiscordBot.Modules;

public class LeaderBoard : InteractionModuleBase<SocketInteractionContext>
{
    private HttpClient _client;
    public InteractionService Commands { get; set; }
    private CommandHandler _handler;

    public LeaderBoard (CommandHandler handler)
    {
        _handler = handler;
        _client = new HttpClient();
    }
    [SlashCommand("initplayers","Init User List on LeaderBoard API")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public async Task InitPlayers()
    {
        Console.WriteLine("InitPlayers");
        var members = Context.Guild.Users.Where(u => !u.IsBot).ToList();
        var responsePlayers =
            members.Select(u => new {name = u.DisplayName, profilePicture = u.GetDisplayAvatarUrl()}).ToList();
        var json = JsonSerializer.Serialize(responsePlayers);
        var output= await _client.PutAsync(@"http://localhost:5001/Player", new StringContent(json, Encoding.UTF8, "application/json"));
        await Context.Interaction.RespondAsync($"```css\n{output.StatusCode}-{(await output.Content.ReadAsStringAsync())}```");
        
    }

    [SlashCommand("showleaderboard", "Show Leaderboard")]
    public async Task ShowLeaderboard(Game game)
    {
        var board = await (await _client.GetAsync($"http://localhost:5001/LeaderBoard/{game}")).Content.ReadFromJsonAsync<LeaderBoardResponse>();
        if(board == null)
        {
            await Context.Interaction.RespondAsync($"```css\n LeaderBoard {game.ToString()} not found!```");
            return;
        }
        if(board.Players is null)
        {
            await Context.Interaction.RespondAsync($"```css\n LeaderBoard {game.ToString()} is empty!```");
            return;
        }
        var embed = CreateFields(board);
        /*await Context.Channel.SendMessageAsync($"```css\n Loading {game.ToString()} LeaderBoard...```")
            .ContinueWith(async t =>
            {
                await Task.Delay(2000);
                await Context.Channel.ModifyMessageAsync(t.Result.Id, m => m.Embeds = new[] {embed.Build()});
            });*/
        await Context.Interaction.RespondAsync(embed: embed);
    }
    public Embed CreateFields(LeaderBoardResponse board)
    {
        var fields = board.Players
            .Select((p,i) => new EmbedFieldBuilder
            {
                Name =  $"{i+1:###}.  |   {p.Player.Name} {new string(' ',30-CountChars(p.Player.Name))} **{p.Score}**",
                Value = new string('-',30),
                IsInline = false
            }).ToList();
        
        var embed = new EmbedBuilder()
            .WithTitle($"*{board.Game.ToString()}*")
            .WithColor(Color.Blue)
            .WithFields(fields);
        return embed.Build();
    }
    
    private int CountChars(string s)
        => s.Count(c => char.ToLower(c) >= ' ' && char.ToLower(c) <= '~');
    
    /*
    public static List<ApplicationCommandOptionChoiceProperties> CreateChoices()
    {
        var httpClient = new HttpClient();
        var response = httpClient.GetAsync(@"http://localhost:5001/LeaderBoard/Types").Result;
        if(!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error while getting LeaderBoard Types: {response.StatusCode}-{response.ReasonPhrase}");
        }
        return response.Content.ReadFromJsonAsync<string[]>().Result?.Select(s =>
               {
                     return new ApplicationCommandOptionChoiceProperties
                     {
                         Name = s,
                            Value = s
                            
                     };
               }).ToList() 
               ?? new List<ChoiceAttribute>();
    }*/
}
