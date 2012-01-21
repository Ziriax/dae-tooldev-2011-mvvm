using System.Reflection;
using System.Windows.Input;

using DaeMvvmFramework;

namespace DogFight
{
	/// <summary>
	/// Context for creating a new fighter, where the name is a required.
	/// </summary>
	public class NewFighterContext : PropertyChangeSource
	{
		public ICommand AddFighterCommand { get; private set; }

		public MainContext MainContext { get; private set; }

		public NewFighterContext(MainContext mainContext)
		{
			MainContext = mainContext;

			AddFighterCommand = CommandFactory.Create(AddNewFighter, CanAddNewFighter,
				this, IsValidProperty);
		}

		private bool CanAddNewFighter()
		{
			return IsValid;
		}

		public void AddNewFighter()
		{
			MainContext.AddFighter(Name);
		}

		#region property string Name

		private string _name;

		public const string NameProperty = "Name";

		public string Name
		{
			get { return _name; }
			set { Change(ref _name, value, NameProperty, IsValidProperty); }
		}

		#endregion

		#region property bool IsValid

		public const string IsValidProperty = "IsValid";

		public bool IsValid
		{
			get { return !string.IsNullOrWhiteSpace(Name); }
		}

		#endregion
	}
}