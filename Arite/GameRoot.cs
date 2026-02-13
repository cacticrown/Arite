using Arite.Editor;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arite;

public class GameRoot : Game
{
    public static GameRoot Instance { get; private set; } = null!;

    private GraphicsDeviceManager _graphics;
    public SpriteBatch SpriteBatch = null!;

    public AriteEditor Editor = new AriteEditor();

    public GameRoot()
    {
        Instance = this;

        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Window.AllowUserResizing = true;
    }

    protected override void Initialize()
    {
        SpriteBatch = new SpriteBatch(GraphicsDevice);

        Editor.Load();

        base.Initialize();
    }

    protected override void Update(GameTime gameTime)
    {
        Editor.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.DarkGray);

        Editor.Draw(gameTime);

        base.Draw(gameTime);
    }

    protected override void OnExiting(object sender, ExitingEventArgs args)
    {
        Editor.OnExit();
    }
}
