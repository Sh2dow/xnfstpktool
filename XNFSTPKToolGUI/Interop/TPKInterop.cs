using System.Runtime.InteropServices;

namespace XNFS_TPKTool_GUI.Interop
{
    public static class TPKInterop
    {
        [DllImport("XNFSTPKTool.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateTPKTool(string filePath);

        [DllImport("XNFSTPKTool.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DestroyTPKTool(IntPtr tpkTool);

        [DllImport("XNFSTPKTool.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetTexturePackMetadata(IntPtr tpkTool);

        [DllImport("XNFSTPKTool.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ExportTexture(IntPtr tpkTool, string textureName, string outputPath);

        [DllImport("XNFSTPKTool.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImportTexture(IntPtr tpkTool, string texturePath);

        [DllImport("XNFSTPKTool.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SaveChanges(IntPtr tpkTool, string outputPath);

        [DllImport("XNFSTPKTool.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr LoadBinFile(string filePath);

        [DllImport("XNFSTPKTool.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr LoadTPK(string filePath);

        [DllImport("XNFSTPKTool.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetTPKCount(IntPtr binHandle);

        [DllImport("XNFSTPKTool.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetTPKMetadata(IntPtr binHandle, int index);

        [DllImport("XNFSTPKTool.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ExportAllImages(IntPtr tpkTool, string outputDir);

        [DllImport("XNFSTPKTool.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool SaveChanges(IntPtr tpkTool, string inputFile, string outputFile);

        [DllImport("XNFSTPKTool.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint GetTextureCount(IntPtr tpkTool);

        [DllImport("XNFSTPKTool.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GetTextureInfo(IntPtr tpkTool, uint index, out TextureInfo textureInfo);
    }
}