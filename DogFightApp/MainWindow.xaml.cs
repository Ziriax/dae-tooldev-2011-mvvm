using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DogFight;

namespace DogFightApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new MainContext();
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
