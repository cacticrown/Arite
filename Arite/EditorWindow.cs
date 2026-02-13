using Hexa.NET.ImGui;
using Microsoft.Xna.Framework;

namespace Arite;

public abstract class EditorWindow
{
    public ImGuiWindowFlags DefaultWindowFlags = ImGuiWindowFlags.NoCollapse;

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
