using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Hexa.NET.ImGui;
using Arite.Style;
using Arite.Style.Themes;
using Arite.Graphics;
using Hexa.NET.ImGui.Widgets.Dialogs;

namespace Arite;

public class AriteEditor
{
    public static AriteEditor Instance { get; private set; } = null!;
    public static string Version => "0.1.0";

    public const string DefaultFontPath = "Assets/Fonts/Inter-VariableFont_opsz,wght.ttf";

    public static SpriteBatch SpriteBatch => GameRoot.Instance.SpriteBatch;

    private ImGuiRenderer imguiRenderer = null!;
    
    private OpenFileDialog openFileDialog = new OpenFileDialog();
    private SaveFileDialog saveFileDialog = new SaveFileDialog();

    public AriteEditor()
    {
        Instance = this;
    }

    public void Load()
    {
        imguiRenderer = new ImGuiRenderer(GameRoot.Instance);

        var io = ImGui.GetIO();
        io.ConfigFlags |= ImGuiConfigFlags.DockingEnable;

        io.Fonts.Clear();

        unsafe
        {
            io.FontDefault = io.Fonts.AddFontFromFileTTF(DefaultFontPath, 16);
        }

        Theme.Apply(new DefaultDarkTheme());
    }

    public void ToggleDefaultTheme()
    {
        if (Theme.Current is DefaultDarkTheme)
        {
            Theme.Apply(new DefaultLightTheme());
        }
        else
        {
            Theme.Apply(new DefaultDarkTheme());
        }
    }

    public void Update(GameTime gameTime)
    {
        
    }

    public void Draw(GameTime gameTime)
    {
        imguiRenderer.BeforeLayout(gameTime);

        DrawMenuBar();

        ImGuiDockNodeFlags dockspaceFlags = ImGuiDockNodeFlags.PassthruCentralNode;
        ImGuiWindowFlags windowFlags = ImGuiWindowFlags.NoDocking | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoBringToFrontOnFocus | ImGuiWindowFlags.NoNavFocus | ImGuiWindowFlags.NoBackground;
        
        ImGuiViewportPtr viewport = ImGui.GetMainViewport();
        ImGui.SetNextWindowPos(viewport.WorkPos);
        ImGui.SetNextWindowSize(viewport.WorkSize);
        ImGui.SetNextWindowViewport(viewport.ID);

        ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0.0f);
        ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0.0f);
        ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, System.Numerics.Vector2.Zero);

        ImGui.Begin("MainDockSpaceWindow", windowFlags);
        ImGui.PopStyleVar(3);

        uint dockspaceId = ImGui.GetID("MyDockSpace");
        ImGui.DockSpace(dockspaceId, System.Numerics.Vector2.Zero, dockspaceFlags);

        ImGui.ShowDebugLogWindow();
        
        openFileDialog.Draw(ImGuiWindowFlags.None);
        saveFileDialog.Draw(ImGuiWindowFlags.None);

        ImGui.End();

        imguiRenderer.AfterLayout();
    }

    public void DrawMenuBar()
    {
        if (ImGui.BeginMainMenuBar())
        {
            if (ImGui.BeginMenu("File"))
            {
                if (ImGui.MenuItem("New"))
                {
                    saveFileDialog.Show();
                }
                if (ImGui.MenuItem("Open"))
                {
                    openFileDialog.Show();
                }
                if (ImGui.MenuItem("Save"))
                {
                    saveFileDialog.Show();
                }
                ImGui.Separator();
                if (ImGui.MenuItem("Exit"))
                {
                    GameRoot.Instance.Exit();
                }
                ImGui.EndMenu();
            }

            if (ImGui.BeginMenu("Edit"))
            {
                if(ImGui.MenuItem("Undo", "Ctrl+Z"))
                {
                    Log.Error("Undo is not implemented yet!");
                }
                if(ImGui.MenuItem("Redo", "Ctrl+Y"))
                {
                    Log.Error("Redo is not implemented yet!");
                }
                ImGui.EndMenu();
            }

            if (ImGui.BeginMenu("View"))
            {
                if(ImGui.BeginMenu("Themes"))
                {
                    if(ImGui.MenuItem("Default Light"))
                    {
                        Theme.Apply(new DefaultLightTheme());
                    }
                    if(ImGui.MenuItem("Default Dark"))
                    {
                        Theme.Apply(new DefaultDarkTheme());
                    }
                    ImGui.EndMenu();
                }
                ImGui.EndMenu();
            }

            if (ImGui.BeginMenu("Help"))
            {
                if (ImGui.MenuItem("Github ↗")) // ↗ is \u2197 in unicode
                {
                    try
                    {
                        const string url = "https://github.com/cacticrown/Arite";
                        System.Diagnostics.ProcessStartInfo processStartInfo = new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = url,
                            UseShellExecute = true
                        };
                        System.Diagnostics.Process.Start(processStartInfo);
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"Failed to open URL: {ex.Message}");
                    }
                }
                ImGui.EndMenu();
            }

            ImGui.EndMainMenuBar();
        }
    }
}
