namespace XNFS_TPKTool_GUI.ViewModels;

public class TextureInfoViewModel
{
    public string Name { get; set; }
    public uint Width { get; set; }
    public uint Height { get; set; }
    public string Format { get; set; } // Optionally resolve the format
    public string Size { get; set; } // Convert size to human-readable format
}