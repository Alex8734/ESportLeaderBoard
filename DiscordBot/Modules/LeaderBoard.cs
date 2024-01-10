using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Text.Json;
using csharpi.Services;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using RequireUserPermissionAttribute = Discord.Commands.RequireUserPermissionAttribute;

namespace DiscordBot.Services;

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
            members.Select(u => new {name = u.DisplayName, profilePicture = u.GetAvatarUrl()}).ToList();
        var json = JsonSerializer.Serialize(responsePlayers);
        var output= await _client.PutAsync(@"http://localhost:5001/Player", new StringContent(json, Encoding.UTF8, "application/json"));
        await Context.Channel.SendMessageAsync($"```css\n{output.StatusCode}-{output.ReasonPhrase}```");
    }
}
