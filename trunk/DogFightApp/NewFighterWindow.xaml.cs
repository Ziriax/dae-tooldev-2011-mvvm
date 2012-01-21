using System.Windows;

namespace DogFightApp
{
	/// <summary>
	/// Interaction logic for NewFighterWindow.xaml
	/// </summary>
	public partial class NewFighterWindow
	{
		public NewFighterWindow()
		{
			InitializeComponent();
		}

		private void HandleOkButtonClick(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
