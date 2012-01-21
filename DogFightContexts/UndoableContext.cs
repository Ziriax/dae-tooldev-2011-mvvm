using System;
using System.Windows.Input;
using DaeMvvmFramework;

namespace DogFight
{
    public class UndoableContext : UndoableChangeSource
    {
        private readonly Evolution _evolution;

        public ICommand UndoCommand { get; private set; }
        public ICommand RedoCommand { get; private set; }

        public UndoableContext(Evolution evolution)
        {
            _evolution = evolution;

			if (_evolution != null)
			{
				UndoCommand = CommandFactory.Create(
					evolution.Undo,
					() => evolution.CanUndo,
					evolution,
					Evolution.CanUndoProperty);

				RedoCommand = CommandFactory.Create(
					evolution.Redo,
					() => evolution.CanRedo,
					evolution,
					Evolution.CanRedoProperty);
			}
        }

        public void ClearHistory()
        {
			if (_evolution != null)
				_evolution.Clear();
        }

        public override Evolution Evolution
        {
            get { return _evolution; }
        }
    }
}