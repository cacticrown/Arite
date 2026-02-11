using System.IO;
using System.Text;
using System.Text.Json;

namespace Arite.Data;

public class Project
{
    public string Path;

    public void Load(string path)
    {
        Path = path;

        JsonDocument jsonDocument = JsonDocument.Parse(File.ReadAllText(path));
        JsonElement jsonRoot = jsonDocument.RootElement;

        string ariteVersion = jsonRoot.GetProperty("AriteVersion").GetString() ?? "Unknown";
        if (ariteVersion != AriteEditor.Version)
        {
            Log.Warning($"Project was created with Arite version {ariteVersion}, but you are using version {AriteEditor.Version}. This may cause compatibility issues.");
        }

        Log.Info($"Loaded project from {path}");
    }

    public void Save()
    {
        using Stream stream = File.Create(Path);
        using (var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true }))
        {
            writer.WriteStartObject();

            writer.WriteString("AriteVersion", AriteEditor.Version);

            writer.WriteEndObject();

            writer.Flush();
        }

        Log.Info($"Saved project to {Path}");
    }
}
