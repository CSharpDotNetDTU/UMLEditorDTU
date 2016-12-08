using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PropertyChanged;
using Uml_Creator.ViewModel;
using ClassFolder = Uml_Creator.ViewModel.Class;

namespace Uml_Creator.View
{

    public partial class MainWindow : Window
    {
        public static MainViewModel vm;

        public MainWindow()
        {
            InitializeComponent();
            vm = (MainViewModel) this.DataContext;
        }


    }

}