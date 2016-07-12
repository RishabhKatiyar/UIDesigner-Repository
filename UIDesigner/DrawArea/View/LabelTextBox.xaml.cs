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

namespace DrawArea.View
{
    /// <summary>
    /// Interaction logic for LabelTextBox.xaml
    /// </summary>
    public partial class LabelTextBox : UserControl
    {
        private int block_Row, block_Col;
        private string id;
        public int Block_Row
        { 
           get { return block_Row; }
           set { block_Row = value; }
        }
        public int Block_Col
        {
            get { return block_Col; }
            set { block_Col = value; }
        }
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        public LabelTextBox()
        {
            InitializeComponent();
        }
    }
}
