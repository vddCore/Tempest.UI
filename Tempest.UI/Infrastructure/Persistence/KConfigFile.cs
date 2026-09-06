namespace Tempest.UI.Infrastructure.Persistence;

using System.Text.RegularExpressions;

internal class KConfigFile
{
    private readonly Dictionary<string, Dictionary<string, string>> _sections = 
        new(StringComparer.OrdinalIgnoreCase);
    
    private readonly string _filePath;

    public KConfigFile(string filePath)
    {
        _filePath = filePath;
        ParseIniFile();
    }

    public string? GetValue(string sectionName, string key)
    {
        if (_sections.TryGetValue(sectionName, out var section))
        {
            return section.GetValueOrDefault(key);
        }
        
        return null;
    }

    private void ParseIniFile()
    {
        if (!File.Exists(_filePath)) return;

        using var fs = new FileStream(
            _filePath, 
            FileMode.Open, 
            FileAccess.Read,
            FileShare.ReadWrite
        );
        
        using var sr = new StreamReader(fs);

        var currentSection = string.Empty;
        while (sr.ReadLine() is { } rawLine)
        {
            var line = rawLine.Trim();

            if (string.IsNullOrWhiteSpace(line) || line.StartsWith(';') || line.StartsWith('#'))
                continue;

            if (line.StartsWith('[') && line.EndsWith(']'))
            {
                currentSection = line[1..^1].Trim();
                if (!_sections.ContainsKey(currentSection))
                {
                    _sections[currentSection] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                }
                continue;
            }

            var separatorIndex = line.IndexOf('=');
            if (separatorIndex > 0 && !string.IsNullOrEmpty(currentSection))
            {
                var key = line[..separatorIndex].Trim();
                var value = line[(separatorIndex + 1)..].Trim();

                _sections[currentSection][key] = value;
            }
        }
    }
}