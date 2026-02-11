using Hexa.NET.ImGui;
using Microsoft.Xna.Framework;

namespace Arite;

public abstract class EditorWindow
{
    public string Title { get; }
    public bool Visible { get; set; } = true;

    protected EditorWindow(string title)
    {
        Title = title;
    }

    public virtual void Load()
    {
        
    }

    public virtual void Update(GameTime gameTime)
    {
        
    }

    public virtual void Draw(GameTime gameTime)
    {
        
    }
}
