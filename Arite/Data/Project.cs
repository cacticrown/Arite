using System.IO;
using System.Text;
using System.Text.Json;
using Arite.Graphics;

namespace Arite.Data;

public class Project
{
    public string Path = null!;

    public List<Tileset> Tilesets = new List<Tileset>();

    public bool TryGetTileset(string name, out Tileset tileset)
    {
        foreach (var _tileset in Tilesets)
        {
            if (_tileset.Name == name)
            {
                tileset = _tileset;
                return true;
            }
        }

        tileset = null!;
        return false;
    }

    public bool ContainsTileset(string name)
    {
        foreach (var _tileset in Tilesets)
        {
            if (_tileset.Name == name)
            {
                return true;
            }
        }

        return false;
    }

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

        JsonElement jsonTilesets = jsonRoot.GetProperty("Tilesets");
        foreach (JsonElement jsonTileset in jsonTilesets.EnumerateArray())
        {
            string name = jsonTileset.GetProperty("Name").GetString() ?? "Unnamed";
            string imagePath = jsonTileset.GetProperty("ImagePath").GetString() ?? "";
            int tileWidth = jsonTileset.GetProperty("TileWidth").GetInt32();
            int tileHeight = jsonTileset.GetProperty("TileHeight").GetInt32();
            int tileSeperationX = jsonTileset.GetProperty("TileSeperationX").GetInt32();
            int tileSeperationY = jsonTileset.GetProperty("TileSeperationY").GetInt32();

            Tilesets.Add(new Tileset
            {
                Name = name,
                ImagePath = imagePath,
                TileWidth = tileWidth,
                TileHeight = tileHeight,
                TileSeperationX = tileSeperationX,
                TileSeperationY = tileSeperationY
            });
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

            writer.WriteStartArray("Tilesets");
            foreach (var tileset in Tilesets)
            {
                writer.WriteStartObject();

                writer.WriteString("Name", tileset.Name);
                writer.WriteString("ImagePath", tileset.ImagePath);
                writer.WriteNumber("TileWidth", tileset.TileWidth);
                writer.WriteNumber("TileHeight", tileset.TileHeight);
                writer.WriteNumber("TileSeperationX", tileset.TileSeperationX);
                writer.WriteNumber("TileSeperationY", tileset.TileSeperationY);

                writer.WriteEndObject();
            }
            writer.WriteEndArray();

            writer.WriteEndObject();

            writer.Flush();
        }

        Log.Info($"Saved project to {Path}");
    }
}
