using System.Reflection;
using Discord;
using Discord.Interactions;

namespace DiscordBot.Services;

[AttributeUsage(AttributeTargets.Parameter)]
public class ChoiceProviderAttribute : Attribute
{
    public List<ApplicationCommandOptionChoiceProperties> Choices { get; private set; }
    public ChoiceProviderAttribute(string methodName)
    {
        var assembly = Assembly.GetEntryAssembly();
        Choices = new List<ApplicationCommandOptionChoiceProperties>();
        foreach (var type in assembly.GetTypes())
        {
            var method = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            if (method != null && method.ReturnType == typeof(List<ApplicationCommandOptionChoiceProperties>))
            {
                Choices = method.Invoke(null, null) as List<ApplicationCommandOptionChoiceProperties> ?? [];
                break;
            }
        }
    }
    
}