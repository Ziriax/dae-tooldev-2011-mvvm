using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Input;
using DaeMvvmFramework;

namespace DogFight
{
    public class FighterContext : UndoableChangeSource
    {
        public WorldContext World { get; private set; }

        #region property: string Name

        private string _name;

        public static readonly string NameProperty = "Name";

        public string Name
        {
            get { return _name; }
            set { Change(_name, value, newValue => Swap(ref _name, newValue, NameProperty)); }
        }

        #endregion

        #region property: FighterContext Target

        private FighterContext _target;

        public static readonly string TargetProperty = "Target";

        public FighterContext Target
        {
            get { return _target; }
            
            set
            {
                if( Change(_target, value, newValue => Swap(ref _target, value, TargetProperty)) )
                {
                    // When target fighter is removed from world, clear it.
                    if( value == null )
                    {
                        World.Fighters.CollectionChanged -= HandleWorldFightersCollectionChange;
                    }
                    else
                    {
                        World.Fighters.CollectionChanged -= HandleWorldFightersCollectionChange;
                    }
                }
            }
        }

        #endregion

        #region property: Point Position

        private Point _position;

        public static readonly string PositionProperty = "Position";

        public Point Position
        {
            get { return _position; }
            set { Change(_position, value, newValue => Swap(ref _position, newValue, PositionProperty)); }
        }

        #endregion

        #region property double Rotation

        private double _rotation;

        public static readonly string RotationProperty = "Rotation";

        public double Rotation
        {
            get { return _rotation; }
            set { Change(_rotation, value, newValue => Swap(ref _rotation, newValue, RotationProperty)); }
        }

        #endregion

        public ICommand ClearTargetCommand { get; private set; }

        public FighterContext(WorldContext world)
        {
            World = world;
            ClearTargetCommand = CommandFactory.Create(ClearTarget, CanClearTarget, this, TargetProperty);
        }

        private void HandleWorldFightersCollectionChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!World.Fighters.Contains(Target))
            {
                Target = null;
            }
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

        public override Evolution Evolution
        {
            get { return World.Main.Evolution; }
        }
    }
}