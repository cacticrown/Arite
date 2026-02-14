using Hexa.NET.ImGui;
using Microsoft.Xna.Framework;

namespace Arite.Editor;

public abstract class EditorWindow
{
    public ImGuiWindowFlags DefaultWindowFlags = ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoTitleBar;

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
