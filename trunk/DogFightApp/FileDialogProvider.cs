using System.Windows;
using DogFight;
using Microsoft.Win32;

namespace DogFightApp
{
    public static class FileDialogProvider
    {
        private static string ShowFileDialog<T>(string oldFilePath) where T : FileDialog, new()
        {
            var dlg = new T
                          {
                              InitialDirectory =
                                  oldFilePath == null ? null : System.IO.Path.GetDirectoryName(oldFilePath),
                              FileName = oldFilePath == null ? null : System.IO.Path.GetFileName(oldFilePath),
                              Filter = "XML files|*.xml|All files|*.*"
                          };

            return dlg.ShowDialog(Application.Current.MainWindow) == true ? dlg.FileName : null;
        }

        public static FilePathProvider OpenFilePathProvider
        {
            get { return ShowFileDialog<OpenFileDialog>; }
        }

        public static FilePathProvider SaveFilePathProvider
        {
            get { return ShowFileDialog<SaveFileDialog>; }
        }
    }
}