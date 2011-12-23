using System;

namespace DaeMvvmFramework
{
    /// <summary>
    /// A simple Mutation that swaps a value using a setter delegate.
    /// </summary>
    public class SwapMutation<TValue> : Mutation 
    {
        private readonly Action<TValue> _setter;
        private TValue _oldValue;
        private TValue _newValue;

        public SwapMutation(Action<TValue> setter, TValue oldValue, TValue newValue)
        {
            _setter = setter;
            _oldValue = oldValue;
            _newValue = newValue;
        }

        protected internal override void Undo()
        {
            Swap();
        }

        protected internal override void Redo()
        {
            Swap();
        }

        private void Swap()
        {
            TValue restoreValue = _oldValue;
            _setter(_newValue);
            _oldValue = _newValue;
            _newValue = restoreValue;
        }
    }
}