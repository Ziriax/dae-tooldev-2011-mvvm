using System.Collections.Generic;

namespace DaeMvvmFramework
{
    /// <summary>
    /// Groups multiple mutations together.
    /// You must create a group with History.BeginTransaction()
    /// </summary>
    internal class MutationGroup : Mutation
    {
        private readonly List<Mutation> _mutations = new List<Mutation>();

        public void Add(Mutation mutation)
        {
            _mutations.Add(mutation);
        }

        protected internal override void Do()
        {
            // Commands are already done.
        }

        protected internal override void Redo()
        {
            for (int i = 0; i < _mutations.Count; i++)
            {
                _mutations[i].Redo();
            }
        }

        protected internal override void Undo()
        {
            for (int i = _mutations.Count; --i >= 0; )
            {
                _mutations[i].Undo();
            }
        }
    }
}