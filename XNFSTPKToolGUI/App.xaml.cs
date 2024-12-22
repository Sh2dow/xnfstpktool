using System;
using System.IO;
using System.Windows;

namespace XNFS_TPKTool_GUI
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Add DLL directory to PATH
            string dllDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Release-XDKLibs");
            Environment.SetEnvironmentVariable("PATH", Environment.GetEnvironmentVariable("PATH") + ";" + dllDirectory);
        }
    }
}