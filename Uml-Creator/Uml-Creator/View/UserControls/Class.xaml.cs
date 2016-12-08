using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Uml_Creator.ViewModel;

namespace Uml_Creator.View.UserControls
{
    /// <summary>
    /// Interaction logic for Class.xaml
    /// </summary>
    public partial class Class : UserControl
    {
        private MainViewModel vm;

        public Class()
        {
            InitializeComponent();
            vm = MainWindow.vm;
        }

        private void ClassGrid_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Grid mainGrid = (Grid) sender;

            double height = mainGrid.ActualHeight;
            double width = mainGrid.ActualWidth;
            var connectedFigure = (FigureViewModel) mainGrid.DataContext;
     
           /*vm.FiguresViewModels[connectedFigure.FigureNr].Height = height;
           vm.FiguresViewModels[connectedFigure.FigureNr].Width = width;*/

        }
    }
}
