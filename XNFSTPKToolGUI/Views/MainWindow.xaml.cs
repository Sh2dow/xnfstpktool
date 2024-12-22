using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using XNFS_TPKTool_GUI.Interop;
using XNFS_TPKTool_GUI.Services;
using XNFS_TPKTool_GUI.ViewModels;

namespace XNFS_TPKTool_GUI.Views
{
    public partial class MainWindow : Window
    {
        private MainViewModel ViewModel { get; }
        private Dictionary<TreeViewItem, IntPtr> tpkHandles = new Dictionary<TreeViewItem, IntPtr>();
        private IntPtr CurrentTPKTool = IntPtr.Zero;

        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainViewModel();
            DataContext = ViewModel;
        }

        private void _LoadBinButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "TPK and BIN Files (*.tpk;*.bin)|*.tpk;*.bin|All Files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;

                try
                {
                    // Load the BIN file using the TPKInterop
                    IntPtr tpkTool = TPKInterop.CreateTPKTool(filePath);

                    if (tpkTool == IntPtr.Zero)
                    {
                        MessageBox.Show("Failed to load BIN file.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Store the loaded TPKTool for further operations
                    CurrentTPKTool = tpkTool; // Ensure CurrentTPKTool is a global or class-level variable

                    // Populate the TreeView with BIN structure
                    PopulateBinTreeView(filePath);

                    MessageBox.Show($"Successfully loaded BIN file: {filePath}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while loading the BIN file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        
        private void LoadBinButton_Click(object sender, RoutedEventArgs e)
        {
            // Show OpenFileDialog to select the BIN file
            var openFileDialog = new OpenFileDialog
            {
                Filter = "TPK and BIN Files (*.tpk;*.bin)|*.tpk;*.bin|All Files (*.*)|*.*",
                Title = "Select a BIN File"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                var binFilePath = openFileDialog.FileName;

                // Attempt to load the BIN file
                IntPtr tpkTool = TPKInterop.CreateTPKTool(binFilePath);
                if (tpkTool != IntPtr.Zero)
                {
                    // Populate the dictionary for texture hash to name mapping
                    PopulateHashToNameDictionary(tpkTool);

                    // Populate the textures grid
                    PopulateTexturesGrid(tpkTool);
                }
                else
                {
                    MessageBox.Show("Failed to load BIN file.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void TexturesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TexturesDataGrid.SelectedItem is TextureInfoViewModel selectedTexture)
            {
                // Replace with actual texture loading logic
                string imagePath = Path.Combine("ExtractedImages", selectedTexture.Name + ".dds");

                if (File.Exists(imagePath))
                {
                    ImageViewer.Source = new BitmapImage(new Uri(imagePath));
                }
                else
                {
                    ImageViewer.Source = null; // Clear the viewer if no image is found
                }
            }
        }

        private static readonly Dictionary<uint, string> hashToNameDictionary = new Dictionary<uint, string>
        {
            // Add known mappings of NameHash to readable names
            // Add more mappings as needed
        };

        private string ResolveTextureName(uint nameHash)
        {
            if (hashToNameDictionary.TryGetValue(nameHash, out string name))
            {
                return name;
            }
            return nameHash.ToString("X8"); // Fallback to hash as a hexadecimal string
        }

        private void PopulateHashToNameDictionary(IntPtr tpkTool)
        {
            uint textureCount = TPKInterop.GetTextureCount(tpkTool);

            for (uint i = 0; i < textureCount; i++)
            {
                if (TPKInterop.GetTextureInfo(tpkTool, i, out Interop.TextureInfo info))
                {
                    if (!hashToNameDictionary.ContainsKey(info.NameHash))
                    {
                        // Dynamically populate the dictionary with placeholder names
                        hashToNameDictionary[info.NameHash] = $"Texture_{i}";
                    }
                }
            }
        }

        private void PopulateTexturesGrid(IntPtr tpkTool)
        {
            uint textureCount = TPKInterop.GetTextureCount(tpkTool);

            var textures = new List<TextureInfoViewModel>();
            for (uint i = 0; i < textureCount; i++)
            {
                if (TPKInterop.GetTextureInfo(tpkTool, i, out TextureInfo info))
                {
                    string resolvedName = ResolveTextureName(info.NameHash);

                    textures.Add(new TextureInfoViewModel
                    {
                        Name = resolvedName,
                        Width = (uint)info.Width,
                        Height = (uint)info.Height,
                        Format = Helper.ResolveTextureFormat(info.ImageCompressionType), // Example formatter
                        Size = $"{info.ImageSize / 1024} KB"
                    });
                }
            }

            TexturesDataGrid.ItemsSource = textures;
        }

        private void PopulateBinTreeView(string filePath)
        {
            // Clear existing items
            BinTreeView.Items.Clear();

            // Add root node for the BIN file
            var rootNode = new TreeViewItem { Header = System.IO.Path.GetFileName(filePath) };

            // Example: Add dummy TPK nodes (replace with actual BIN parsing logic)
            for (int i = 0; i < 5; i++)
            {
                var tpkNode = new TreeViewItem { Header = $"TPK {i + 1}" };
                rootNode.Items.Add(tpkNode);
            }

            BinTreeView.Items.Add(rootNode);
        }

        private IntPtr GetSelectedTPKTool()
        {
            if (BinTreeView.SelectedItem is TreeViewItem selectedItem && tpkHandles.TryGetValue(selectedItem, out IntPtr tpkToolHandle))
            {
                return tpkToolHandle;
            }

            return IntPtr.Zero;
        }

        private void CleanupTPKHandles()
        {
            foreach (var handle in tpkHandles.Values)
            {
                TPKInterop.DestroyTPKTool(handle); // Free the TPK tool instance
            }
            tpkHandles.Clear();
        }

        private void BinTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            IntPtr tpkTool = GetSelectedTPKTool();
            if (tpkTool == IntPtr.Zero)
            {
                ImageMetadataTextBox.Text = "No TPK selected or failed to retrieve TPK tool.";
                return;
            }

            string outputDir = "ExtractedImages";
            Directory.CreateDirectory(outputDir);

            int imageCount = TPKInterop.ExportAllImages(tpkTool, outputDir);
            if (imageCount > 0)
            {
                // Load the first image for preview
                string firstImagePath = Path.Combine(outputDir, "Image1.dds"); // Adjust indexing dynamically
                if (File.Exists(firstImagePath))
                {
                    BitmapImage bitmap = new BitmapImage(new Uri(firstImagePath, UriKind.RelativeOrAbsolute));
                    ImageViewer.Source = bitmap;

                    ImageMetadataTextBox.Text = $"Extracted {imageCount} images.\nPreview: {Path.GetFileName(firstImagePath)}";
                }
            }
        }
        
        private void ExportImagesButton_Click(object sender, RoutedEventArgs e)
        {
            IntPtr tpkTool = GetSelectedTPKTool(); // Ensure you have this method implemented
            if (tpkTool == IntPtr.Zero)
            {
                MessageBox.Show("No valid TPK selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "ExtractedImages");
            Directory.CreateDirectory(outputDir);

            try
            {
                int exportedCount = TPKInterop.ExportAllImages(tpkTool, outputDir);
                if (exportedCount > 0)
                {
                    MessageBox.Show($"Successfully exported {exportedCount} images to {outputDir}.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Optionally update UI or display metadata
                }
                else
                {
                    MessageBox.Show("No images were exported.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting images: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExportTPKButton_Click(object sender, RoutedEventArgs e)
        {
            if (TexturesDataGrid.SelectedItem is Models.TextureInfo selectedTexture)
            {
                var saveFileDialog = new SaveFileDialog
                {
                    Filter = "Textures (*.dds;*.png)|*.dds;*.png|All Files (*.*)|*.*"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    ViewModel.ExportTexture(selectedTexture, saveFileDialog.FileName);
                }
            }
        }

        private void ImportTPKButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files (*.png;*.dds)|*.png;*.dds|All Files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                ViewModel.ImportTexture(openFileDialog.FileName);
            }
        }

        private void SaveTPKButton_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "TPK Files (*.tpk)|*.tpk|All Files (*.*)|*.*"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                ViewModel.SaveChanges(saveFileDialog.FileName);
            }
        }
    }
}
