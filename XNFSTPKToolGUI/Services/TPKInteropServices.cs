using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using XNFS_TPKTool_GUI.Interop;
using XNFS_TPKTool_GUI.Models;
using TextureInfo = XNFS_TPKTool_GUI.Interop.TextureInfo;

namespace XNFS_TPKTool_GUI.Services
{
    public static class TPKInteropServices
    {
        public static List<string> LoadBin(string filePath)
        {
            IntPtr binHandle = TPKInterop.LoadBinFile(filePath);
            // IntPtr binHandle = LoadTPK(filePath);
            if (binHandle == IntPtr.Zero)
                throw new Exception("Failed to load bin file.");

            int tpkCount = TPKInterop.GetTPKCount(binHandle);
            var metadataList = new List<string>();
            for (int i = 0; i < tpkCount; i++)
            {
                IntPtr metadataPtr = TPKInterop.GetTPKMetadata(binHandle, i);
                string metadata = Marshal.PtrToStringAnsi(metadataPtr);
                metadataList.Add(metadata);
            }

            return metadataList;
        }
        
        public static List<TextureInfo> LoadTexturePack(string filePath)
        {
            IntPtr tpkTool = TPKInterop.CreateTPKTool(filePath);
            IntPtr metadata = TPKInterop.GetTexturePackMetadata(tpkTool);

            // Example of how metadata could be parsed:
            var textures = new List<TextureInfo>();
            // Populate `textures` list based on the metadata structure.

            TPKInterop.DestroyTPKTool(tpkTool);
            return textures;
        }

        public static void ExportTexture(string filePath, string textureName, string outputPath)
        {
            IntPtr tpkTool = TPKInterop.CreateTPKTool(filePath);
            TPKInterop.ExportTexture(tpkTool, textureName, outputPath);
            TPKInterop.DestroyTPKTool(tpkTool);
        }

        public static void ImportTexture(string filePath, string texturePath)
        {
            IntPtr tpkTool = TPKInterop.CreateTPKTool(filePath);
            TPKInterop.ImportTexture(tpkTool, texturePath);
            TPKInterop.DestroyTPKTool(tpkTool);
        }

        public static void SaveChanges(string filePath, string outputPath)
        {
            IntPtr tpkTool = TPKInterop.CreateTPKTool(filePath);
            TPKInterop.SaveChanges(tpkTool, outputPath);
            TPKInterop.DestroyTPKTool(tpkTool);
        }
    }
}
