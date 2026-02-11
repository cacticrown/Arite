namespace Arite.Data;

public class Project
{
    public string Path;

    public void Load(string path)
    {
        Path = path;

        Log.Info($"Loaded project from {path}");
    }

    public void Save()
    {
        File.WriteAllText(Path, "Project data goes here...");

        Log.Info($"Saved project to {Path}");
    }
}
