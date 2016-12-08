using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Uml_Creator.View.UserControls
{
    /// <summary>
    /// Interaction logic for StatusBar.xaml
    /// </summary>
    public partial class StatusBar : UserControl
    {

        /*
        public static DependencyProperty StatusBarTextProperty =
            DependencyProperty.Register("TextBar", typeof(string), typeof(StatusBar),
                new PropertyMetadata("default value"));


        public string TextBar
        {
            get { return (string)GetValue(StatusBarTextProperty); }
            set { SetValue(StatusBarTextProperty, value); }
        }
        */
        public StatusBar()
        {
            InitializeComponent();
        }

     
    }
}
