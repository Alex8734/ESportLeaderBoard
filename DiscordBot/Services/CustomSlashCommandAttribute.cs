using System.Reflection;
using Discord.Interactions;

namespace DiscordBot.Services;

[AttributeUsage(AttributeTargets.Method)]
public class CustomSlashCommandAttribute(
    string name,
    string description,
    bool ignoreGroupNames = false,
    RunMode runMode = RunMode.Default) : Attribute
{
    public string Name { get; } = name;
    public string Description { get; } = description;
    public bool IgnoreGroupNames { get; } = ignoreGroupNames;
    public RunMode RunMode { get; } = runMode;
}