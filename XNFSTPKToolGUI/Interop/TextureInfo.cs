using System.Runtime.InteropServices;

namespace XNFS_TPKTool_GUI.Interop;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct TextureInfo
{
    // [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    // public string Name;

    public uint Unknown;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
    public int[] Padding_990;

    public uint NameHash;
    public uint ClassNameHash;
    public int ImagePlacement;
    public int PalettePlacement;
    public int ImageSize;
    public int PaletteSize;
    public int BaseImageSize;
    public short Width;
    public short Height;
    public byte ShiftWidth;
    public byte ShiftHeight;
    public byte ImageCompressionType;
    public byte PaletteCompressionType;
    public short NumPaletteEntries;
    public byte NumMipMapLevels;
    public byte TilableUV;
    public byte BiasLevel;
    public byte RenderingOrder;
    public byte ScrollType;
    public byte UsedFlag;
    public byte ApplyAlphaSorting;
    public byte AlphaUsageType;
    public byte AlphaBlendType;
    public byte Flags;
    public short ScrollTimeStep;
    public short ScrollSpeedS;
    public short ScrollSpeedT;
    public short OffsetS;
    public short OffsetT;
    public short ScaleS;
    public short ScaleT;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 14)]
    public byte[] Other;
}