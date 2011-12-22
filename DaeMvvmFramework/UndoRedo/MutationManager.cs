using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DaeMvvmFramework
{
    /// <summary>
    /// A MutationManager keeps two stacks of commands,
    /// one for the commands that can be undone,
    /// one for the commands that can be redone.
    /// </summary>
    /// <remarks>
    /// Doing a new Mutation clears the redo stack;
    /// it is however possible to keep all commands,
    /// but this is left as a hard exercise :)
    /// </remarks>
    public class MutationManager
    {
        private readonly Stack<Mutation> _undoables = new Stack<Mutation>();
        private readonly Stack<Mutation> _redoables = new Stack<Mutation>();

        /// <summary>
        /// Raised whenever Undo, Redo or Do is called.
        /// </summary>
        public event EventHandler<MutationEventArgs> Changed;

        public bool CanUndo
        {
            get { return _undoables.Count > 0;  }
        }

        public bool CanRedo
        {
            get { return _redoables.Count > 0; }
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
            _redoables.Clear();
            _undoables.Push(cmd);
            OnChanged(cmd, MutationEventKind.Do);
        }

        protected virtual void OnChanged(Mutation mutation, MutationEventKind kind)
        {
            var handler = Changed;
            if (handler != null)
                handler(this, new MutationEventArgs(mutation, kind));
        }
    }
}
