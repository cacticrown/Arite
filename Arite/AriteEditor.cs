using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Hexa.NET.ImGui;
using Arite.Editor;
using Arite.Data;
using Arite.Graphics;
using Arite.Style;
using Arite.Style.Themes;

namespace Arite;

public class AriteEditor : Game
{
    public static AriteEditor Instance { get; private set; } = null!;

    public static string Version => "0.1.0";

    public const string DefaultFontPath = "Assets/Fonts/Inter-VariableFont_opsz,wght.ttf";
    public const string ProjectFileExtension = "ariteproj";

    /// <summary>
    /// Null means that no project is currently open
    /// </summary>
    public Project Project { get; private set; } = null!;

    public ProjectEditor ProjectEditor = new ProjectEditor();

    public ImGuiRenderer ImguiRenderer = null!;

    private GraphicsDeviceManager _graphics;
    public SpriteBatch SpriteBatch = null!;

    public AriteEditor()
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

        base.Initialize();
    }

    protected override void LoadContent()
    {
        ImguiRenderer = new ImGuiRenderer(this);

        var io = ImGui.GetIO();
        io.ConfigFlags |= ImGuiConfigFlags.DockingEnable;

        io.Fonts.Clear();

        unsafe
        {
            io.FontDefault = io.Fonts.AddFontFromFileTTF(DefaultFontPath, 16);
        }

        ProjectEditor.Load();

        Settings.Load();
        if(Settings.RecentProjects.Count > 0)
        {
            string recentProject = Settings.RecentProjects[0];
            LoadProject(recentProject);
        }

        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        ProjectEditor.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.DarkGray);

        ImguiRenderer.BeforeLayout(gameTime);

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

        ProjectEditor.Draw(gameTime);

        ImGui.End();

        ImguiRenderer.AfterLayout();

        base.Draw(gameTime);
    }

    protected override void OnExiting(object sender, ExitingEventArgs args)
    {
        SaveProject();
        Settings.Save();
        ImGui.SaveIniSettingsToDisk("imgui.ini");
    }

    public void DrawMenuBar()
    {
        if (ImGui.BeginMainMenuBar())
        {
            if (ImGui.BeginMenu("File"))
            {
                if (ImGui.MenuItem("New"))
                {
                    NewProject();
                }
                if (ImGui.MenuItem("Open"))
                {
                    OpenProject();
                }
                if (ImGui.BeginMenu("Open Recent"))
                {
                    foreach(string recentProject in Settings.RecentProjects)
                    {
                        if(ImGui.MenuItem(recentProject))
                        {
                            LoadProject(recentProject);
                            break;
                        }
                    }
                    ImGui.EndMenu();
                }
                if (ImGui.MenuItem("Save"))
                {
                    SaveProject();
                }
                if(ImGui.MenuItem("Close Project"))
                {
                    CloseProject();
                }
                ImGui.Separator();
                if (ImGui.MenuItem("Restart"))
                {
                    Process.Start(Environment.ProcessPath);
                    Exit();
                }
                if (ImGui.MenuItem("Exit"))
                {
                    Exit();
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

    public void NewProject()
    {
        NativeFileDialogSharp.DialogResult result = NativeFileDialogSharp.Dialog.FileSave(ProjectFileExtension);
        if (result.IsOk)
        {
            CloseProject();
            Project = new Project();

            string filePath = result.Path;

            if (!Path.HasExtension(filePath))
            {
                filePath = Path.ChangeExtension(filePath, ProjectFileExtension);
            }

            Project.Path = filePath;

            SaveProject();

            OnProjectLoaded();
        }
        else
        {
            Log.Warning("New project creation failed or was canceled.");
        }
    }

    public void OpenProject()
    {
        NativeFileDialogSharp.DialogResult result = NativeFileDialogSharp.Dialog.FileOpen(ProjectFileExtension);
        if (result.IsOk)
        {
            CloseProject();

            string filePath = result.Path;
            Project = Project.Load(filePath);

            if(Project == null)
            {
                return;
            }

            OnProjectLoaded();
        }
    }

    public void LoadProject(string path)
    {
        CloseProject();
        Project = Project.Load(path);
        OnProjectLoaded();
    }

    public void SaveProject()
    {
        if(Project != null)
        {
            Project.Save();
        }
    }

    public void CloseProject()
    {
        if(Project == null)
        {
            return;
        }

        Project.Save();
        Project = null!;
        OnProjectUnloaded();
    }

    public void OnProjectLoaded()
    {
        if(Project == null)
        {
            OnProjectUnloaded();
        }

        Window.Title = $"Arite - {Project.Path}";
    }

    public void OnProjectUnloaded()
    {
        Window.Title = "Arite";
    }
}
