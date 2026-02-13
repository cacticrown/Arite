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

        TabButton("General", SelectedProjectEditorTab.General, fullWidth, buttonHeight);
        TabButton("Layers", SelectedProjectEditorTab.Layers, fullWidth, buttonHeight);
        TabButton("Tilesets", SelectedProjectEditorTab.Tilesets, fullWidth, buttonHeight);
        TabButton("Entities", SelectedProjectEditorTab.Entities, fullWidth, buttonHeight);

        ImGui.End();

        switch (SelectedTab)
        {
            case SelectedProjectEditorTab.Tilesets:
                TilesetEditor.Draw(gameTime);
                break;
        }

        base.Draw(gameTime);
    }

    private void TabButton(string label, SelectedProjectEditorTab tab, float width, float height)
    {
        bool isActive = SelectedTab == tab;

        if (isActive)
        {   
            var activeColor = ImGui.GetStyle().Colors[(int)ImGuiCol.ButtonActive];
            ImGui.PushStyleColor(ImGuiCol.Button, activeColor);
            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, activeColor);
            ImGui.PushStyleColor(ImGuiCol.ButtonActive, activeColor);
        }

        if (ImGui.Button(label, new System.Numerics.Vector2(width, height)))
        {
            SelectedTab = tab;
        }

        if (isActive)
        {
            ImGui.PopStyleColor(3);
        }
    }
}

public enum SelectedProjectEditorTab
{
    General,
    Layers,
    Tilesets,
    Entities
}
