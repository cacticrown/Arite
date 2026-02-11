using System.Numerics;

namespace Arite.Style.Themes;

public sealed class DefaultDarkTheme : ITheme
{
    public string Name => "DefaultDarkTheme";

    public ThemeData GetThemeData() => new()
    {
        Colors = new ThemeColors
        {
            Text = new Vector4(0.95f, 0.96f, 0.98f, 1f),
            TextDisabled = new Vector4(0.50f, 0.50f, 0.50f, 1f),

            WindowBackground = new Vector4(0.11f, 0.11f, 0.12f, 1f),
            ChildBackground = new Vector4(0.11f, 0.11f, 0.12f, 1f),
            PopupBackground = new Vector4(0.14f, 0.14f, 0.15f, 1f),

            Border = new Vector4(0.25f, 0.25f, 0.26f, 1f),
            BorderShadow = new Vector4(0f, 0f, 0f, 0f),

            FrameBackground = new Vector4(0.18f, 0.18f, 0.19f, 1f),
            FrameBackgroundHovered = new Vector4(0.24f, 0.24f, 0.25f, 1f),
            FrameBackgroundActive = new Vector4(0.28f, 0.28f, 0.29f, 1f),

            TitleBackground = new Vector4(0.10f, 0.10f, 0.11f, 1f),
            TitleBackgroundActive = new Vector4(0.16f, 0.16f, 0.17f, 1f),
            TitleBackgroundCollapsed = new Vector4(0.10f, 0.10f, 0.11f, 1f),

            MenuBarBackground = new Vector4(0.14f, 0.14f, 0.15f, 1f),

            ScrollbarBackground = new Vector4(0.05f, 0.05f, 0.06f, 1f),
            ScrollbarGrab = new Vector4(0.31f, 0.31f, 0.32f, 1f),
            ScrollbarGrabHovered = new Vector4(0.41f, 0.41f, 0.42f, 1f),
            ScrollbarGrabActive = new Vector4(0.51f, 0.51f, 0.52f, 1f),

            CheckMark = new Vector4(0.26f, 0.59f, 0.98f, 1f),
            SliderGrab = new Vector4(0.26f, 0.59f, 0.98f, 1f),
            SliderGrabActive = new Vector4(0.37f, 0.69f, 1f, 1f),

            Button = new Vector4(0.20f, 0.20f, 0.21f, 1f),
            ButtonHovered = new Vector4(0.26f, 0.59f, 0.98f, 1f),
            ButtonActive = new Vector4(0.06f, 0.53f, 0.98f, 1f),

            Header = new Vector4(0.20f, 0.20f, 0.21f, 1f),
            HeaderHovered = new Vector4(0.26f, 0.59f, 0.98f, 1f),
            HeaderActive = new Vector4(0.06f, 0.53f, 0.98f, 1f),

            Separator = new Vector4(0.28f, 0.28f, 0.28f, 1f),
            SeparatorHovered = new Vector4(0.44f, 0.44f, 0.44f, 1f),
            SeparatorActive = new Vector4(0.26f, 0.59f, 0.98f, 1f),

            ResizeGrip = new Vector4(0.26f, 0.59f, 0.98f, 0.25f),
            ResizeGripHovered = new Vector4(0.26f, 0.59f, 0.98f, 0.67f),
            ResizeGripActive = new Vector4(0.26f, 0.59f, 0.98f, 0.95f),

            Tab = new Vector4(0.18f, 0.18f, 0.19f, 1f),
            TabHovered = new Vector4(0.26f, 0.59f, 0.98f, 1f),
            TabSelected = new Vector4(0.20f, 0.20f, 0.21f, 1f),
            TabSelectedOverline = new Vector4(0.26f, 0.59f, 0.98f, 1f),

            TabDimmed = new Vector4(0.15f, 0.15f, 0.16f, 1f),
            TabDimmedSelected = new Vector4(0.20f, 0.20f, 0.21f, 1f),
            TabDimmedSelectedOverline = new Vector4(0.26f, 0.59f, 0.98f, 1f),

            DockingPreview = new Vector4(0.26f, 0.59f, 0.98f, 0.70f),
            DockingEmptyBackground = new Vector4(0.11f, 0.11f, 0.12f, 1f),

            PlotLines = new Vector4(0.61f, 0.61f, 0.61f, 1f),
            PlotLinesHovered = new Vector4(1f, 0.43f, 0.35f, 1f),
            PlotHistogram = new Vector4(0.90f, 0.70f, 0.00f, 1f),
            PlotHistogramHovered = new Vector4(1f, 0.60f, 0.00f, 1f),

            TableHeaderBackground = new Vector4(0.19f, 0.19f, 0.20f, 1f),
            TableBorderStrong = new Vector4(0.31f, 0.31f, 0.32f, 1f),
            TableBorderLight = new Vector4(0.21f, 0.21f, 0.22f, 1f),
            TableRowBackground = new Vector4(0f, 0f, 0f, 0f),
            TableRowBackgroundAlt = new Vector4(1f, 1f, 1f, 0.06f),

            TextSelectedBackground = new Vector4(0.26f, 0.59f, 0.98f, 0.35f),
            DragDropTarget = new Vector4(1f, 1f, 0f, 0.9f),

            NavWindowHighlight = new Vector4(0.26f, 0.59f, 0.98f, 1f),
            NavWindowDimBackground = new Vector4(0f, 0f, 0f, 0.20f),
            ModalWindowDimBackground = new Vector4(0f, 0f, 0f, 0.35f)
        },
        Metrics = new ThemeMetrics
        {
            DpiScale = 1f,
            Alpha = 1f,
            DisabledAlpha = 0.60f,
            WindowPadding = new Vector2(8f, 8f),
            WindowRounding = 5f,
            WindowBorderSize = 1f,
            ChildRounding = 4f,
            ChildBorderSize = 1f,
            PopupRounding = 4f,
            PopupBorderSize = 1f,
            FramePadding = new Vector2(4f, 3f),
            FrameRounding = 3f,
            FrameBorderSize = 1f,
            ItemSpacing = new Vector2(8f, 4f),
            ItemInnerSpacing = new Vector2(4f, 4f),
            CellPadding = new Vector2(4f, 2f),
            IndentSpacing = 21f,
            ScrollbarSize = 14f,
            ScrollbarRounding = 9f,
            GrabMinimumSize = 10f,
            GrabRounding = 3f,
            TabRounding = 4f,
            TabBorderSize = 1f,
            AntiAliasedLines = true,
            AntiAliasedFill = true
        }
    };
}
