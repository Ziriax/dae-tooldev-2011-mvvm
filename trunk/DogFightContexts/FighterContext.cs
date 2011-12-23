using System;
using System.Windows;
using System.Windows.Input;
using DaeMvvmFramework;

namespace DogFight
{
    public class FighterContext : PropertyChangeSource
    {
        public WorldContext World { get; private set; }

        #region undoable property: string Name

        private string _name;

        public static readonly string NameProperty = "Name";

        public string Name
        {
            get { return _name; }
            set { World.Main.Swap(_name, value, newValue => Change(ref _name, newValue, NameProperty)); }
        }

        #endregion

        #region undoable property: FighterContext Target

        private FighterContext _target;

        public static readonly string TargetProperty = "Target";

        public FighterContext Target
        {
            get { return _target; }
            set { World.Main.Swap(_target, value, newValue => Change(ref _target, value, TargetProperty)); }
        }

        #endregion

        #region undoable property: Point Position

        private Point _position;

        public static readonly string PositionProperty = "Position";

        public Point Position
        {
            get { return _position; }
            set { World.Main.Swap(_position, value, newValue => Change(ref _position, newValue, PositionProperty)); }
        }

        #endregion

        #region undoable property double Rotation

        private double _rotation;

        public static readonly string RotationProperty = "Rotation";

        public double Rotation
        {
            get { return _rotation; }
            set { World.Main.Swap(_rotation, value, newValue => Change(ref _rotation, newValue, RotationProperty)); }
        }

        #endregion

        public ICommand ClearTargetCommand { get; private set; }

        public FighterContext(WorldContext parent)
        {
            World = parent;
            ClearTargetCommand = CommandFactory.Create(ClearTarget, CanClearTarget, this, TargetProperty);
        }

        private bool CanClearTarget()
        {
            return TargetProperty != null;
        }

        private void ClearTarget()
        {
            Target = null;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}