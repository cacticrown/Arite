using System.Text.Json;
using Arite.Style;
using Arite.Style.Themes;

namespace Arite;

public static class Settings
{
    public const string Path = "settings.json";

    public static string SelectedTheme { get; set; } = "DefaultLightTheme";

    public static List<string> RecentProjects { get; set; } = new List<string>();

    public static void AddRecentProject(string path)
    {
        RecentProjects.Remove(path);
        RecentProjects.Insert(0, path);
        if (RecentProjects.Count > 10)
        {
            RecentProjects.RemoveAt(10);
        }
    }

    public static void Load()
    {
        if (!File.Exists(Path))
        {
            Log.Info($"Settings file not found at {Path}, using default settings.");
            return;
        }

        try
        {
            JsonDocument jsonDocument = JsonDocument.Parse(File.ReadAllText(Path));
            JsonElement root = jsonDocument.RootElement;

            if (root.TryGetProperty("SelectedTheme", out JsonElement selectedThemeElement))
            {
                SelectedTheme = selectedThemeElement.GetString() ?? SelectedTheme;
            }
            if (root.TryGetProperty("RecentProjects", out JsonElement recentProjectsElement))
            {
                foreach (JsonElement recentProjectElement in recentProjectsElement.EnumerateArray())
                {
                    RecentProjects.Add(recentProjectElement.GetString() ?? string.Empty);
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error($"Failed to load settings from {Path}: {ex.Message}");
        }

        switch(SelectedTheme)
        {
            case "DefaultDarkTheme":
                Theme.Apply(new DefaultDarkTheme());
                break;
            case "DefaultLightTheme":
                Theme.Apply(new DefaultLightTheme());
                break;
            default:
                Log.Error($"Unknown theme '{SelectedTheme}' in settings, applying default light theme.");
                Theme.Apply(new DefaultLightTheme());
                break;
        }
    }

    public static void Save()
    {
        SelectedTheme = Theme.Current.Name;

        try
        {
            using Stream stream = File.Create(Path);
            using (var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true }))
            {
                writer.WriteStartObject();

                writer.WriteString("SelectedTheme", SelectedTheme);
                writer.WriteStartArray("RecentProjects");
                foreach (var recentProject in RecentProjects)
                {
                    writer.WriteStringValue(recentProject);
                }
                writer.WriteEndArray();

                writer.WriteEndObject();

                writer.Flush();
            }

            Log.Info($"Saved project to {Path}");
        }
        catch (Exception ex)
        {
            Log.Error($"Failed to save settings to {Path}: {ex.Message}");
        }
    }
}
