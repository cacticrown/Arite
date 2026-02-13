using Hexa.NET.ImGui;

namespace Arite;

public static class ImGuiHelper
{
    public static void TextInput(string label, ref string text, nuint maxLength = 256)
    {
        ImGui.AlignTextToFramePadding();
        ImGui.Text(label);
        ImGui.SameLine();
        ImGui.PushID(label);
        ImGui.InputText("", ref text, maxLength);
        ImGui.PopID();
    }

    public static void IntInput(string label, ref int number)
    {
        ImGui.AlignTextToFramePadding();
        ImGui.Text(label);
        ImGui.SameLine();
        ImGui.PushID(label);
        ImGui.InputInt("", ref number);
        ImGui.PopID();
    }
}
