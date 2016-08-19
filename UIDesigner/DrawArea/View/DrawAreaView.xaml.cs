using DrawArea.View;
using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace DrawArea
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class DrawAreaView : Window
    {
        public DrawAreaView()
        {
            InitializeComponent();
        }
        private void myGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DrawAreaViewModel ob = new DrawAreaViewModel(); ob.OnMouseLeftButtonDown(sender, e, myGrid);
        }

        //ws
    }
}
