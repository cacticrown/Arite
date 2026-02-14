using Arite.Data;
using Arite.Graphics;
using Arite.Style;
using Arite.Style.Themes;
using Hexa.NET.ImGui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Arite.Editor;

public class AriteEditor
{
    public static AriteEditor Instance { get; private set; } = null!;
    public static string Version => "0.1.0";

    public const string DefaultFontPath = "Assets/Fonts/Inter-VariableFont_opsz,wght.ttf";
    public const string ProjectFileExtension = "ariteproj";

    public static SpriteBatch SpriteBatch => GameRoot.Instance.SpriteBatch;

    /// <summary>
    /// Null means that no project is currently open
    /// </summary>
    public Project Project { get; private set; } = null!;

    public ProjectEditor ProjectEditor = new ProjectEditor();

    public ImGuiRenderer ImguiRenderer = null!;

    public AriteEditor()
    {
        Instance = this;
    }

    public void Load()
    {
        ImguiRenderer = new ImGuiRenderer(GameRoot.Instance);

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
            Project = new Project();
            Project.Load(recentProject);
        }
    }

    public void Update(GameTime gameTime)
    {
        ProjectEditor.Update(gameTime);

        if (Project != null)
        {
            GameRoot.Instance.Window.Title = $"Arite - {Project.Path}";
        }
        else
        {
            GameRoot.Instance.Window.Title = "Arite";
        }
    }

    public void Draw(GameTime gameTime)
    {
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
                            if(Project is null)
                            {
                                Project = new Project();
                            }
                            Project.Load(recentProject);
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
                    GameRoot.Instance.Exit();
                }
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

    public void NewProject()
    {
        NativeFileDialogSharp.DialogResult result = NativeFileDialogSharp.Dialog.FileSave(ProjectFileExtension);
        if (result.IsOk)
        {
            if(Project == null)
            {
                Project = new Project();
            }

            Project.Reset();

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
            if(Project is null)
            {
                Project = new Project();
            }

            string filePath = result.Path;
            Project.Load(filePath);

            OnProjectLoaded();
        }
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
        Project.Save();
        Project = null!;
        OnProjectUnloaded();
    }

    public void OnProjectLoaded()
    {
        GameRoot.Instance.Window.Title = $"Arite - {Project.Path}";
    }

    public void OnProjectUnloaded()
    {
        GameRoot.Instance.Window.Title = "Arite";
    }

    public void OnExit()
    {
        SaveProject();
        Settings.Save();
        ImGui.SaveIniSettingsToDisk("imgui.ini");
    }
}
