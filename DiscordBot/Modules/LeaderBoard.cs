using csharpi.Services;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using RequireUserPermissionAttribute = Discord.Commands.RequireUserPermissionAttribute;

namespace DiscordBot.Modules;

public class LeaderBoard : Discord.Commands.ModuleBase<Discord.Commands.SocketCommandContext>
{
    public InteractionService Commands { get; set; }
    private CommandHandler _handler;
    
    public LeaderBoard (CommandHandler handler)
    {
        _handler = handler;
    }
    [SlashCommand("initplayers","Init User List on LeaderBoard API")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public async Task InitPlayers()
    {
        
    }
}
