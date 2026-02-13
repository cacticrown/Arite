using Microsoft.Xna.Framework;
using Hexa.NET.ImGui;
using Arite.Data;

namespace Arite;

public class TilesetEditor : EditorWindow
{
    private string newTilesetName = "";
    private bool focusNameField;
    private string? selectedTileset;
    private string selectedTilesetNewName = "";

    public override void Draw(GameTime gameTime)
    {
        if (AriteEditor.Instance.Project == null)
        {
            ImGui.Text("No project loaded.");
            return;
        }

        ImGui.Begin("Tilesets", DefaultWindowFlags);

        if (ImGui.Button("Add Tileset"))
        {
            newTilesetName = "";
            ImGui.OpenPopup("Add Tileset");
            focusNameField = true;
        }

        DrawAddTilesetPopup();

        ImGui.Separator();

        float height = ImGui.GetContentRegionAvail().Y;

        if (ImGui.BeginTable("TilesetSplit", 2, ImGuiTableFlags.Resizable | ImGuiTableFlags.BordersInnerV | ImGuiTableFlags.SizingStretchSame, new System.Numerics.Vector2(0, height)))
        {
            ImGui.TableNextColumn();

            ImGui.BeginChild("TilesetList", new System.Numerics.Vector2(0, 0));

            foreach (var _tileset in AriteEditor.Instance.Project.Tilesets)
            {
                bool isSelected = selectedTileset == _tileset.Name;

                if (ImGui.Selectable(_tileset.Name, isSelected))
                {
                    SetSelectedTileset(_tileset.Name);
                }
            }

            ImGui.EndChild();

            ImGui.TableNextColumn();

            ImGui.BeginChild("InspectorContent", new System.Numerics.Vector2(0, 0));

            if (selectedTileset != null && AriteEditor.Instance.Project.TryGetTileset(selectedTileset, out var tileset))
            {
                ImGui.InputText("Name", ref selectedTilesetNewName, 256);

                if (!selectedTilesetNewName.IsWhiteSpace() && selectedTilesetNewName != selectedTileset && !AriteEditor.Instance.Project.ContainsTileset(selectedTilesetNewName))
                {
                    tileset.Name = selectedTilesetNewName;
                    selectedTileset = selectedTilesetNewName;
                }

                ImGui.Separator();

                string oldImagePath = tileset.ImagePath;

                ImGui.Text("Image Path");
                ImGui.SameLine();
                if (ImGui.Button("Browse"))
                {
                    NativeFileDialogSharp.DialogResult result = NativeFileDialogSharp.Dialog.FileOpen("png,jpg,jpeg,bmp");
                    if (result.IsOk)
                    {
                        tileset.ImagePath = Path.GetRelativePath(AriteEditor.Instance.Project.Directory, result.Path);
                    }
                }
                ImGui.SameLine();
                ImGui.InputText("##", ref tileset.ImagePath, 256);

                if(tileset.ImagePath != oldImagePath)
                {
                    tileset.UnloadTexture();
                }

                ImGuiHelper.IntInput("Tile Width", ref tileset.TileWidth);
                ImGuiHelper.IntInput("Tile Height", ref tileset.TileHeight);
                ImGuiHelper.IntInput("Tile Separation X", ref tileset.TileSeperationX);
                ImGuiHelper.IntInput("Tile Separation Y", ref tileset.TileSeperationY);
				
				ImGui.Text("Image Preview:");
                var texture2D = tileset.Texture;
				if(texture2D != null)
				{
					var texture = AriteEditor.Instance.ImguiRenderer.BindTexture(tileset.Texture);
					ImGui.Image(texture, new System.Numerics.Vector2(tileset.Texture.Width, tileset.Texture.Height));
				}
				else
				{
					ImGui.Text("Texture not found.");
				}
            }
            else
            {
                ImGui.Text("Select a tileset to edit.");
            }

            ImGui.EndChild();

            ImGui.EndTable();
        }

        ImGui.End();

        base.Draw(gameTime);
    }

	public void DrawAddTilesetPopup()
	{
		if (ImGui.BeginPopupModal("Add Tileset", ImGuiWindowFlags.AlwaysAutoResize))
        {
            ImGui.Text("Enter Tileset Name");

            if (focusNameField)
            {
                ImGui.SetKeyboardFocusHere();
                focusNameField = false;
            }

            bool pressedEnter = ImGui.InputText("##TilesetName", ref newTilesetName, 256, ImGuiInputTextFlags.EnterReturnsTrue);

            bool pressedEscape = ImGui.IsKeyPressed(ImGuiKey.Escape);

            ImGui.Separator();

            if (ImGui.Button("Create") || pressedEnter)
            {
                if (!string.IsNullOrWhiteSpace(newTilesetName) && !AriteEditor.Instance.Project.TryGetTileset(newTilesetName, out _))
                {
                    AriteEditor.Instance.Project.Tilesets.Add(new Tileset
                    { 
                        Name = newTilesetName
                    });
                    SetSelectedTileset(newTilesetName);
                    AriteEditor.Instance.SaveProject();
                }

                ImGui.CloseCurrentPopup();
            }

            ImGui.SameLine();

            if (ImGui.Button("Cancel") || pressedEscape)
            {
                ImGui.CloseCurrentPopup();
            }

            ImGui.EndPopup();
        }
	}

    public void SetSelectedTileset(string name)
    {
        selectedTileset = name;
        selectedTilesetNewName = name;
    }
}
