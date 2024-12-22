namespace XNFS_TPKTool_GUI.Services;

public static class Helper
{
    public static string ResolveTextureFormat(byte compressionType)
    {
        return compressionType switch
        {
            0x20 => "RGB",
            0x22 => "DXT1",
            0x24 => "DXT3",
            0x26 => "DXT5",
            0x08 => "P8",
            _ => "Unknown"
        };
    }

}