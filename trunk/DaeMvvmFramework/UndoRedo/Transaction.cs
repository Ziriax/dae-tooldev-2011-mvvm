using System;

namespace DaeMvvmFramework
{
    public class Transaction : IDisposable
    {
        private History _history;

        public History History
        {
            get { return _history; }
        }

        public int Level { get; private set; }

        public Transaction(History history, int level)
        {
            _history = history;
            Level = level;
        }

        public void Do(Mutation cmd)
        {
            _history.Do(cmd);
        }

        public void Dispose()
        {
            if( _history != null )
            {
                _history.EndTransaction(this);
                _history = null;
            }
        }
    }
}