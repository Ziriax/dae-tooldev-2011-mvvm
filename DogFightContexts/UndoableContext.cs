using System;
using System.Windows.Input;
using DaeMvvmFramework;

namespace DogFight
{
    public class UndoableContext : PropertyChangeSource
    {
        private readonly History _history;

        public ICommand UndoCommand { get; private set; }
        public ICommand RedoCommand { get; private set; }

        public UndoableContext(History history)
        {
            _history = history;

            UndoCommand = CommandFactory.Create(history.Undo, () => history.CanUndo,
                history, History.CanUndoProperty);

            RedoCommand = CommandFactory.Create(history.Redo, () => history.CanRedo,
                history, History.CanRedoProperty);
        }

        public void ClearHistory()
        {
            _history.Clear();
        }

        public void Do(Mutation mutation)
        {
            _history.Do(mutation);
        }

        public Transaction BeginTransaction()
        {
            return _history.BeginTransaction();
        }

        public void Swap<TValue>(TValue oldValue, TValue newValue, Action<TValue> setter)
        {
            Do(MutationFactory.Swap(oldValue, newValue, setter));
        }
    }
}