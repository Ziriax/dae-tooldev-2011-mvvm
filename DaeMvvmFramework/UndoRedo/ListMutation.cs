using System.Collections.Generic;
using System.Diagnostics;

namespace DaeMvvmFramework
{
    /// <summary>
    /// A simple Mutation that inserts or removes an item from a list
    /// </summary>
    public abstract class ListMutation<T> : Mutation 
    {
        private readonly IList<T> _list;
        private readonly T _item;
        private readonly int _index;
        private readonly int _operation;

        protected T Item
        {
            get { return _item; }
        }

        protected int Index
        {
            get { return _index; }
        }

        protected ListOperation Operation
        {
            get { return (ListOperation)_operation; }
        }

        protected IList<T> List
        {
            get { return _list; }
        }

        protected ListMutation(ListOperation operation, IList<T> list, T item, int index)
        {
            Debug.Assert(list != null);
            Debug.Assert(index <= list.Count);
            Debug.Assert(index >= 0);
            _operation = (int) operation;
            _list = list;
            _item = item;
            _index = index;
        }

        protected ListMutation(ListOperation operation, IList<T> list, T item)
            : this(operation, list, item, 
                operation==ListOperation.Insert ? list.Count : list.IndexOf(item))
        {
        }

        protected virtual void Insert()
        {
            _list.Insert(_index, _item);
        }

        protected virtual void Remove()
        {
            _list.RemoveAt(_index);
        }

        protected virtual void Apply(ListOperation op)
        {
            if( op == ListOperation.Insert )
            {
                Insert();
            }
            else
            {
                Remove();
            }
        }

        protected internal override void Undo()
        {
            Apply( (ListOperation) (-_operation));
        }

        protected internal override void Redo()
        {
            Apply((ListOperation)_operation);
        }
    }
}