using System.Windows;
using System.Windows.Input;
using DaeMvvmFramework;

namespace DogFight
{
    public class FighterContext : PropertyChangeSource
    {
        public WorldContext Parent { get; private set; }

        #region property string Name

        private string _name;

        public static readonly string NameProperty = "Name";

        public string Name
        {
            get { return _name; }
            set { Change(ref _name, value, NameProperty); }
        }

        #endregion

        #region property FighterContext Target

        private FighterContext _target;

        public static readonly string TargetProperty = "Target";

        public FighterContext Target
        {
            get { return _target; }
            set { Change(ref _target, value, TargetProperty); }
        }

        #endregion

        #region property Point Position

        private Point _position;

        public static readonly string PositionProperty = "Position";

        public Point Position
        {
            get { return _position; }
            set { Change(ref _position, value, PositionProperty); }
        }

        #endregion

        #region property double Rotation

        private double _rotation;

        public static readonly string RotationProperty = "Rotation";

        public double Rotation
        {
            get { return _rotation; }
            set { Change(ref _rotation, value, RotationProperty); }
        }

        #endregion

        public ICommand ClearTargetCommand { get; private set; }

        public FighterContext(WorldContext parent)
        {
            Parent = parent;
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