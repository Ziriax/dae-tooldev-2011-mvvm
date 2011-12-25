using System.Collections.Generic;
using System.Windows;
using System.Windows.Automation.Peers;
using DaeMvvmFramework;
using DogFight;

namespace DogFightApp
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            DataContext = new MainContext(new Evolution(100));
            InitializeComponent();
        }

        #region HACK: To get WPF work better on systems with (Wacom) tablets
        // http://blogs.msdn.com/b/jgoldb/archive/2009/12/18/wpf-performance-on-tablet-touch-enabled-machines.aspx
        private class FakeWindowsPeer : WindowAutomationPeer
        {
            public FakeWindowsPeer(Window window)
                : base(window)
            {
                
            }
            protected override List<AutomationPeer> GetChildrenCore()
            {
                return null;
            }
        }

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new FakeWindowsPeer(this);
        }

        #endregion
    }
}
