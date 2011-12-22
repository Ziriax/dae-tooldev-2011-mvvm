using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DogFight;
using Microsoft.Win32;

namespace DogFightApp
{
    /// <summary>
    /// Interaction logic for MenuView.xaml
    /// </summary>
    public partial class MenuView : UserControl
    {
        public MenuView()
        {
            InitializeComponent();
        }

        private string ShowFileDialog<T>(string oldFilePath) where T : FileDialog, new()
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

        public FilePathProvider OpenFilePathProvider
        {
            get { return ShowFileDialog<OpenFileDialog>; }
        }

        public FilePathProvider SaveFilePathProvider
        {
            get { return ShowFileDialog<SaveFileDialog>; }
        }
    }
}
