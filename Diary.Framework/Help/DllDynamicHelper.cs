using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Reflection;
using Caliburn.Micro;
using Microsoft.Win32;

namespace Diary.Framework.Help
{
    public class DllDynamicHelper
    {
        public static string _externalPluginsFolder = "\\ExternalPlugins";

        public static string FindDLL()
        {
            var openDialog = new OpenFileDialog();

            openDialog.Filter = "Extensions (*.DLL)|*.dll;";

            if (openDialog.ShowDialog() == true)
                return openDialog.FileName;

            return null;
        }

        public static void LoadExtension(string filename)
        {
            var executingPath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var path = executingPath + DllDynamicHelper._externalPluginsFolder;

            File.Copy(filename, path + "\\" + Path.GetFileName(filename), true);
        }
    }
}
