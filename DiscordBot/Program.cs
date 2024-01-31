using System;
using System.Reflection;
using System.Text;
using Discord;
using Discord.Net;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using DiscordBot.Modules;
using DiscordBot.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Text.Json;
using RunMode = Discord.Interactions.RunMode;

DiscordSocketClient client;
InteractionService commands;
ulong testGuildId = 885176257506082866;

using (var services = ConfigureServices())
{
    client = services.GetRequiredService<DiscordSocketClient>();
    commands = services.GetRequiredService<InteractionService>();
    
    client.Log += Log;
    commands.Log += Log;
    client.Ready += Ready;

    await client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("DiscordToken"));
    await client.StartAsync();
    
    await services.GetRequiredService<CommandHandler>().InitializeAsync();
    
    await Task.Delay(Timeout.Infinite);
}


async Task Ready()
{
    if (IsDebug())
    {
        Console.WriteLine($"In debug mode, adding commands to {testGuildId}...");
        await commands.RegisterCommandsToGuildAsync(testGuildId, true);
        
        Console.WriteLine($"{client.Guilds.Count} guilds have been loaded");
    }
    else
    {
       
        await commands.RegisterCommandsGloballyAsync(true);
    }

    var guild = client.Guilds.First(g => g.Id == 1026219411859836939);
    Console.WriteLine("InitPlayers");
    var members = guild.Users.Where(u => !u.IsBot).ToList();
    var responsePlayers =
        members.Select(u => new {name = u.DisplayName, profilePicture = u.GetDisplayAvatarUrl()}).ToList();
    var json = JsonSerializer.Serialize(responsePlayers);
    var output= await new HttpClient().PutAsync(@"http://localhost:5001/Player", new StringContent(json, Encoding.UTF8, "application/json"));

}

ServiceProvider ConfigureServices()
{
    return new ServiceCollection()
        .AddSingleton(x => new DiscordSocketClient(new DiscordSocketConfig
        {
            LogLevel = LogSeverity.Info,
            MessageCacheSize = 50,
            GatewayIntents = GatewayIntents.All
        }))
        .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>(),new InteractionServiceConfig
        {
            DefaultRunMode = RunMode.Async,
            LogLevel = LogSeverity.Info,
            
        }))
        .AddSingleton<CommandHandler>()
        .BuildServiceProvider();
}

static Task Log(LogMessage message)
{
    switch (message.Severity)
    {
        case LogSeverity.Critical:
        case LogSeverity.Error:
            Console.ForegroundColor = ConsoleColor.Red;
            break;
        case LogSeverity.Warning:
            Console.ForegroundColor = ConsoleColor.Yellow;
            break;
        case LogSeverity.Info:
            Console.ForegroundColor = ConsoleColor.White;
            break;
        case LogSeverity.Verbose:
        case LogSeverity.Debug:
            Console.ForegroundColor = ConsoleColor.DarkGray;
            break;
    }
    Console.WriteLine($"{DateTime.Now,-19} [{message.Severity,8}] {message.Source}: {message.Message} {message.Exception}");
    Console.ResetColor();
        
    // If you get an error saying 'CompletedTask' doesn't exist,
    // your project is targeting .NET 4.5.2 or lower. You'll need
    // to adjust your project's target framework to 4.6 or higher
    // (instructions for this are easily Googled).
    // If you *need* to run on .NET 4.5 for compat/other reasons,
    // the alternative is to 'return Task.Delay(0);' instead.
    return Task.CompletedTask;
}

static bool IsDebug ( )
{
#if DEBUG
    return true;
#else
                return false;
#endif
}