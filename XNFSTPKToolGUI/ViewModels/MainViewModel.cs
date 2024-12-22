using System.Collections.ObjectModel;
using XNFS_TPKTool_GUI.Interop;
using XNFS_TPKTool_GUI.Services;
using TextureInfo = XNFS_TPKTool_GUI.Models.TextureInfo;

namespace XNFS_TPKTool_GUI.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public ObservableCollection<TextureInfo> Textures { get; set; } = new ObservableCollection<TextureInfo>();

        private string _currentFilePath;
        public string CurrentFilePath
        {
            get => _currentFilePath;
            set => SetProperty(ref _currentFilePath, value);
        }

        // public void LoadTextures(string filePath)
        // {
        //     CurrentFilePath = filePath;
        //     var textures = TPKInteropServices.LoadTexturePack(filePath);
        //
        //     Textures.Clear();
        //     foreach (TextureInfo texture in textures)
        //     {
        //         Textures.Add(texture);
        //     }
        // }

        public void ExportTexture(TextureInfo texture, string outputPath)
        {
            TPKInteropServices.ExportTexture(CurrentFilePath, texture.Name, outputPath);
        }

        public void ImportTexture(string texturePath)
        {
            TPKInteropServices.ImportTexture(CurrentFilePath, texturePath);
        }

        public void SaveChanges(string savePath)
        {
            TPKInteropServices.SaveChanges(CurrentFilePath, savePath);
        }
    }
}