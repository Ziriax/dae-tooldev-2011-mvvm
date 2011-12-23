using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DaeMvvmFramework
{
    /// <summary>
    /// A History keeps two stacks of mutations,
    /// one for the mutations that can be undone,
    /// one for the mutations that can be redone.
    /// </summary>
    /// <remarks>
    /// Doing a new Mutation clears the redo stack;
    /// it is however possible to keep all mutations,
    /// but this is left as a hard exercise :)
    /// </remarks>
    public class History : PropertyChangeSource
    {
        private int _groupLevel;
        private MutationGroup _currentGroup;

        private readonly Stack<Mutation> _undoables = new Stack<Mutation>();
        private readonly Stack<Mutation> _redoables = new Stack<Mutation>();

        /// <summary>
        /// Raised whenever Undo, Redo or Do is called.
        /// </summary>
        public event EventHandler<MutationEventArgs> Changed;

        public const string CanUndoProperty = "CanUndo";
        public const string CanRedoProperty = "CanRedo";

        public bool CanUndo
        {
            get { return _undoables.Count > 0;  }
        }

        public bool CanRedo
        {
            get { return _redoables.Count > 0; }
        }

        public Transaction BeginTransaction()
        {
            if (_groupLevel++ == 0 )
            {
                _currentGroup = new MutationGroup();
            }

            return new Transaction(this, _groupLevel);
        }

        internal void EndTransaction(Transaction transaction)
        {
            if( transaction.History != this || transaction.Level != _groupLevel )
                throw new ArgumentException("transaction");

            if( --_groupLevel == 0 )
            {
                Debug.Assert(_currentGroup!= null);
                var group = _currentGroup;
                _currentGroup = null;
                Add(group);
            }
        }

        public void Undo()
        {
            Debug.Assert(CanUndo);
            Mutation cmd = _undoables.Pop();
            cmd.Undo();
            _redoables.Push(cmd);
            OnChanged(cmd, MutationEventKind.Undo);
        }

        public void Redo()
        {
            Debug.Assert(CanRedo);
            Mutation cmd = _redoables.Pop();
            cmd.Redo();
            _undoables.Push(cmd);
            OnChanged(cmd, MutationEventKind.Redo);
        }

        public void Do(Mutation cmd)
        {
            cmd.Do();

            if (_currentGroup != null)
            {
                _currentGroup.Add(cmd);
            }
            else
            {
                Add(cmd);
            }
        }

        private void Add(Mutation cmd)
        {
            _redoables.Clear();
            _undoables.Push(cmd);
            OnChanged(cmd, MutationEventKind.Do);
        }

        public void Clear()
        {
            _undoables.Clear();
            _redoables.Clear();
            OnPropertyChanged(CanUndoProperty, CanRedoProperty);
        }

        protected virtual void OnChanged(Mutation mutation, MutationEventKind kind)
        {
            OnPropertyChanged(CanUndoProperty, CanRedoProperty);

            var handler = Changed;
            if (handler != null)
                handler(this, new MutationEventArgs(mutation, kind));
        }
    }
}
