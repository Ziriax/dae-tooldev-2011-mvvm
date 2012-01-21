using System.Collections.Specialized;
using System.IO;
using System.Windows.Input;
using DaeMvvmFramework;

namespace DogFight
{
    public delegate string FilePathProvider(string currentFilePath);

    public class MainContext : UndoableContext
    {
        public ICommand NewCommand { get; private set; }
        public ICommand OpenCommand { get; private set; }
        public ICommand SaveAsCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }

        public ICommand RemoveFighterCommand { get; private set; }

        public ICommand MoveFighterDownCommand { get; private set; }
        public ICommand MoveFighterUpCommand { get; private set; }

        #region property WorldContext World

        private WorldContext _world;

        public static readonly string WorldProperty = "World";

        public WorldContext World
        {
            get { return _world; }
            private set { Change(ref _world, value, WorldProperty); }
        }

        #endregion

        #region property FighterContext SelectedFighter

        private FighterContext _selectedFighter;

        public static readonly string SelectedFighterProperty = "SelectedFighter";

        public FighterContext SelectedFighter
        {
            get { return _selectedFighter; }
            
            set
            {
                if (Change(_selectedFighter, value, x => Swap(ref _selectedFighter, x, SelectedFighterProperty)))
                {
                    // When selected fighter is removed from world, clear it.
                    if (value == null)
                    {
                        World.Fighters.CollectionChanged -= HandleWorldFightersCollectionChange;
                    }
                    else
                    {
                        World.Fighters.CollectionChanged -= HandleWorldFightersCollectionChange;
                    }
                }
            }
        }

        #endregion

        #region property string FilePath

        private string _filePath;

        public static readonly string FilePathProperty = "FilePath";

        public string FilePath
        {
            get { return _filePath; }
            set { Change(ref _filePath, value, FilePathProperty); }
        }

        #endregion

        public MainContext(Evolution evolution):base(evolution)
        {
            NewCommand = CommandFactory.Create(New);
            OpenCommand = CommandFactory.Create<FilePathProvider>(Open);
            SaveAsCommand = CommandFactory.Create<FilePathProvider>(SaveAs);
            SaveCommand = CommandFactory.Create(Save, CanSave, this, FilePathProperty);

            RemoveFighterCommand = CommandFactory.Create(RemoveFighter, IsFighterSelected, 
                this, SelectedFighterProperty);

            MoveFighterUpCommand = CommandFactory.Create(MoveSelectedFighterUp, IsFighterSelected, 
                this, SelectedFighterProperty);

            MoveFighterDownCommand = CommandFactory.Create(MoveSelectedFighterDown, IsFighterSelected, 
                this, SelectedFighterProperty);

            New();
        }

        private void MoveSelectedFighterDown()
        {
            using (BeginTransaction())
            {
                var fighters = World.Fighters;
                int index = fighters.IndexOf(SelectedFighter);

                if (index < fighters.Count - 1)
                {
                    fighters.Move(index, index + 1);
                }
            }
        }

        private void MoveSelectedFighterUp()
        {
            using (BeginTransaction())
            {
                var fighters = World.Fighters;
                int index = fighters.IndexOf(SelectedFighter);

                if (index > 0 )
                {
                    fighters.Move(index, index - 1);
                }
            }
        }

        private bool IsFighterSelected()
        {
            return SelectedFighter != null;
        }

        private void RemoveFighter()
        {
            using( BeginTransaction() )
            {
                var fighters = World.Fighters;

                int selectedFighterIndex = fighters.IndexOf(SelectedFighter);
                fighters.RemoveAt(selectedFighterIndex);

                // Select the fighter after the removed one,
                // or if the removed one was the last one, 
                // select the one before.
                FighterContext nextSelectedFighter = null;

                if (selectedFighterIndex < fighters.Count )
                {
                    nextSelectedFighter = fighters[selectedFighterIndex];
                }
                else if (selectedFighterIndex > 0 )
                {
                    nextSelectedFighter = fighters[selectedFighterIndex - 1];
                }

                SelectedFighter = nextSelectedFighter;
            }
        }

    	public void AddFighter(string name)
        {
            using (BeginTransaction())
            {
				var newFighter = new FighterContext(World) { Name = name };
                World.Fighters.Add(newFighter);
                SelectedFighter = newFighter;
            }
        }

        protected void New(WorldContext world)
        {
            ClearHistory();
            SelectedFighter = null;
            World = world;
        }

        public void New()
        {
            New(new WorldContext(this));
        }

        public void Open(FilePathProvider filePathProvider)
        {
            string newFilePath = filePathProvider(FilePath);

            if (newFilePath != null)
            {
                using (var stream = File.OpenRead(newFilePath))
                {
                    World worldModel = DogFight.World.LoadFrom(stream);
                    var worldContext = new WorldContext(this, worldModel);
                    New(worldContext);

                    FilePath = newFilePath;
                }
            }
        }

        public void SaveAs(FilePathProvider filePathProvider)
        {
            string newFilePath = filePathProvider(FilePath);

            if (newFilePath != null)
            {
                FilePath = newFilePath;
                Save();
            }
        }

        public void Save()
        {
            using (var stream = File.Create(FilePath))
            {
                World worldModel = World.CreateModel();
                worldModel.SaveTo(stream);
            }
        }

        public bool CanSave()
        {
            return !string.IsNullOrEmpty(FilePathProperty);
        }

        private void HandleWorldFightersCollectionChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            if( !World.Fighters.Contains(SelectedFighter) )
            {
                SelectedFighter = null;
            }
        }
    }
}