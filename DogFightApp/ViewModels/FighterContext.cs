using System;
using DogFight;

namespace DogFightApp
{
    public class FighterContext 
    {
        private readonly Fighter _model;

        public FighterContext(Fighter model)
        {
            _model = model;
        }

        public event EventHandler NameChanged;

        public string Name
        {
            get { return _model.Name; }

            set
            {
                if( _model.Name != value )
                {
                    _model.Name = value;
                    OnNameChanged();
                }
            }
        }

        protected virtual void OnNameChanged()
        {
            var handler = NameChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
    }
}