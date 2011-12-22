using System.Diagnostics;
using System.Reflection;

namespace DaeMvvmFramework
{
    /// <summary>
    /// A simple Mutation that simply sets the property of an object
    /// </summary>
    /// <remarks>
    /// This Mutation can only be used when setting the property 
    /// to the previous value completely restores the state of the object.
    /// This is not always the case!
    /// </remarks>
    public class SetPropertyMutation : Mutation
    {
        private readonly object _target;
        private readonly PropertyInfo _propertyInfo;
        private object _value;

        public SetPropertyMutation(object target, PropertyInfo propertyInfo, object value)
        {
            Debug.Assert(propertyInfo.DeclaringType.IsAssignableFrom(target.GetType()));
            Debug.Assert(propertyInfo.PropertyType.IsAssignableFrom(value.GetType()));
            _target = target;
            _value = value;
            _propertyInfo = propertyInfo;
        }

        public SetPropertyMutation(object target, string propertyName, object value) 
            : this(target, target.GetType().GetProperty(propertyName), value)
        {
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
            object oldValue = _propertyInfo.GetValue(_target, null);
            _propertyInfo.SetValue(_target, _value, null);
            _value = oldValue;
        }
    }
}