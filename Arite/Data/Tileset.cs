using Arite.Editor;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arite.Data;

public class Tileset
{
    public string Name = string.Empty;
    public string ImagePath = string.Empty;
    public int TileWidth = 8;
    public int TileHeight = 8;
    public int TileSeperationX = 0;
    public int TileSeperationY = 0;

    private Texture2D texture;
    public Texture2D Texture
    {
        get
        {
            if (texture == null && File.Exists(Path.Combine(AriteEditor.Instance.Project.Directory, ImagePath)))
            {
                LoadTexture();    
            }

            return texture;
        }
    }

    private void LoadTexture()
    {
        try
        {
            using FileStream fileStream = new FileStream(Path.Combine(AriteEditor.Instance.Project.Directory, ImagePath), FileMode.Open, FileAccess.Read);
            texture = Texture2D.FromStream(AriteEditor.SpriteBatch.GraphicsDevice, fileStream);
        }
        catch (Exception ex)
        {
            Log.Error($"Failed to load tileset texture from path '{ImagePath}': {ex.Message}");
        }
    }

    public void UnloadTexture()
    {
        if (texture != null)
        {
            texture.Dispose();
            texture = null!;
        }
    }
}
