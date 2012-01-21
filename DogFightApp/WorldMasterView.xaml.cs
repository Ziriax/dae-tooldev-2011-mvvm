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

namespace DogFightApp
{
    /// <summary>
    /// Interaction logic for WorldMasterView.xaml
    /// </summary>
    public partial class WorldMasterView : UserControl
    {
        public WorldMasterView()
        {
            InitializeComponent();
        }

		private void HandleAddButtonClick(object sender, RoutedEventArgs e)
		{
			var mainContext = (MainContext)DataContext;
			
			var newFighterContext = new NewFighterContext(mainContext);
			
			var newFighterWindow = new NewFighterWindow
			{
				DataContext = newFighterContext,
				WindowStartupLocation = WindowStartupLocation.CenterOwner,
				Owner = Application.Current.MainWindow
			};
			
			newFighterWindow.ShowDialog();
		}
    }
}
