using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using DogFight;

namespace DogFightApp
{
    public class AddFighterCommand : ICommand
    {
        private readonly MainContext _context;

        public AddFighterCommand(MainContext context)
        {
            _context = context;
        }

        public void Execute(object parameter)
        {
            _context.Fighters.Add(
                new FighterContext(
                    new Fighter { Name="Fighter"+Environment.TickCount}));
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }

    public class MainContext
    {
        public ObservableCollection<FighterContext> Fighters { get; private set; }

        public ICommand NewCommand { get; private set; }
        public ICommand OpenCommand { get; private set; }
        public ICommand SaveAsCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }

        public ICommand AddFighterCommand { get; private set; }
        public ICommand RemoveFighterCommand { get; private set; }

        private FighterContext _selectedFighter;
        
        public FighterContext SelectedFighter
        {
            get { return _selectedFighter; }
            
            set
            {
                if (_selectedFighter != value)
                {
                    _selectedFighter = value;
                    OnSelectedFighterChanged(EventArgs.Empty);
                }
            }
        }

        public event EventHandler SelectedFighterChanged;

        protected virtual void OnSelectedFighterChanged(EventArgs e)
        {
            EventHandler handler = SelectedFighterChanged;
            if (handler != null) handler(this, e);
        }

        public MainContext()
        {
            NewCommand = OpenCommand = SaveAsCommand = SaveCommand = new DummyCommand();

            AddFighterCommand = new AddFighterCommand(this);

            Fighters = new ObservableCollection<FighterContext>();
        }
    }
}