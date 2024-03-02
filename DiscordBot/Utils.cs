using System.Text;

namespace DiscordBot;

public static class Utils
{
    public static string UnicodeToAscii(string unicodeString)
    {
        Encoding ascii = Encoding.ASCII;
        Encoding unicode = Encoding.Unicode;

        byte[] unicodeBytes = unicode.GetBytes(unicodeString);
        byte[] asciiBytes = Encoding.Convert(unicode, ascii, unicodeBytes);

        string asciiString = ascii.GetString(asciiBytes);
        return asciiString;
    }
}