using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DaeMvvmFramework
{
    public static class MutationFactory
    {
        public static ListMutation<T> Add<T>(IList<T> list, T item)
        {
            return new ListMutation<T>(ListOperation.Insert, list, item);
        }

        public static ListMutation<T> Remove<T>(IList<T> list, T item)
        {
            return new ListMutation<T>(ListOperation.Remove, list, item);
        }

        public static ListMutation<T> InsertAt<T>(IList<T> list, T item, int index)
        {
            return new ListMutation<T>(ListOperation.Insert, list, item, index);
        }

        public static ListMutation<T> RemoveAt<T>(IList<T> list, int index)
        {
            return new ListMutation<T>(ListOperation.Remove, list, list[index], index);
        }

        public static SwapMutation<TValue> Swap<TValue>(TValue oldValue, TValue newValue, Action<TValue> setter)
        {
            return new SwapMutation<TValue>(setter, oldValue, newValue);
        }
    }
}