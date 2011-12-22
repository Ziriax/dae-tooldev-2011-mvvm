using System;
using System.IO;
using System.Windows.Input;
using DaeMvvmFramework;

namespace DogFight
{
    public delegate string FilePathProvider(string currentFilePath);

    public class MainContext : PropertyChangeSource
    {
        private int _newFighterCounter;

        public ICommand NewCommand { get; private set; }
        public ICommand OpenCommand { get; private set; }
        public ICommand SaveAsCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }

        public ICommand AddFighterCommand { get; private set; }
        public ICommand RemoveFighterCommand { get; private set; }

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
            set { Change(ref _selectedFighter, value, SelectedFighterProperty); }
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

        public MainContext()
        {
            NewCommand = CommandFactory.Create(New);
            OpenCommand = CommandFactory.Create<FilePathProvider>(Open);
            SaveAsCommand = CommandFactory.Create<FilePathProvider>(SaveAs);
            SaveCommand = CommandFactory.Create(Save, CanSave, this, FilePathProperty);
            AddFighterCommand = CommandFactory.Create(AddFighter);
            RemoveFighterCommand = CommandFactory.Create(RemoveFighter, CanRemoveFighter, this, SelectedFighterProperty);

            New();
        }

        private bool CanRemoveFighter()
        {
            return SelectedFighter != null;
        }

        private void RemoveFighter()
        {
            FighterContext selectedFighter = SelectedFighter;
            int fighterIndex = World.Fighters.IndexOf(selectedFighter);
            World.Fighters.RemoveAt(fighterIndex);

            // Make sure no remaining fighter has the SelectedFighter as Target
            foreach (var fighter in World.Fighters)
            {
                if (fighter.Target == selectedFighter)
                {
                    fighter.Target = null;
                }
            }

            // Select the fighter after the removed one,
            // or if the removed one was the last one, 
            // select the one before.
            SelectedFighter = World.Fighters.Count > 0 
                ? World.Fighters[Math.Min(fighterIndex, World.Fighters.Count-1)] 
                : null;
        }

        private void AddFighter()
        {
            string newFighterName = "Fighter" + (++_newFighterCounter);
            var newFighter = new FighterContext(World) {Name = newFighterName};
            World.Fighters.Add(newFighter);

            SelectedFighter = newFighter;
        }

        public void New()
        {
            _newFighterCounter = 0;

            World = new WorldContext(this);
        }

        public void Open(FilePathProvider filePathProvider)
        {
            string newFilePath = filePathProvider(FilePath);

            if (newFilePath != null)
            {
                using (var stream = File.OpenRead(newFilePath))
                {
                    World worldModel = DogFight.World.LoadFrom(stream);
                    World = new WorldContext(this, worldModel);
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
    }
}