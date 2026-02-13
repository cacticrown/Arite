using Hexa.NET.ImGui;
using Microsoft.Xna.Framework;

namespace Arite.Editor;

public class ProjectEditor : EditorWindow
{
    public TilesetEditor TilesetEditor = new TilesetEditor();
    public SelectedProjectEditorTab SelectedTab = SelectedProjectEditorTab.General;

    public override void Draw(GameTime gameTime)
    {
        ImGui.Begin("Tabs", DefaultWindowFlags);

        float fullWidth = ImGui.GetContentRegionAvail().X;
        float buttonHeight = fullWidth / 6;

        if (ImGui.Button("General", new System.Numerics.Vector2(fullWidth, buttonHeight)))
        {
            SelectedTab = SelectedProjectEditorTab.General;
        }

        if (ImGui.Button("Layers", new System.Numerics.Vector2(fullWidth, buttonHeight)))
        {
            SelectedTab = SelectedProjectEditorTab.Layers;
        }

        if (ImGui.Button("Tilesets", new System.Numerics.Vector2(fullWidth, buttonHeight)))
        {
            SelectedTab = SelectedProjectEditorTab.Tilesets;
        }

        if (ImGui.Button("Entities", new System.Numerics.Vector2(fullWidth, buttonHeight)))
        {
            SelectedTab = SelectedProjectEditorTab.Entities;
        }

        ImGui.End();

        switch (SelectedTab)
        {
            case SelectedProjectEditorTab.Tilesets:
                TilesetEditor.Draw(gameTime);
                break;
        }

        base.Draw(gameTime);
    }
}

public enum SelectedProjectEditorTab
{
    General,
    Layers,
    Tilesets,
    Entities
}
