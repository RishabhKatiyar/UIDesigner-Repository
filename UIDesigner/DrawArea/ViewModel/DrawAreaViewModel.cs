using DrawArea.View;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DrawArea.ViewModel
{
    public partial class DrawAreaViewModel : INotifyPropertyChanged
    {
        Grid MyGrid;
        int selectedColumnIndex = -1, selectedRowIndex = -1;
        const int WIDTH = 20;
        const int HEIGHT = 20;
        public event PropertyChangedEventHandler PropertyChanged;
        string row;
        public string Row
        {
            get
            {
                return row;
            }
            set
            {
                if (row != value)
                {
                    row = value;
                    RaisePropertyChangedEvent("Row");
                }
            }
        }
        string col;
        public string Col
        {
            get
            {
                return col;
            }
            set
            {
                if (col != value)
                {
                    col = value;
                    RaisePropertyChangedEvent("Col");
                }
            }
        }
        protected void RaisePropertyChangedEvent(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand GoCommand
        {
            get { return new DelegateCommand(GoFun); }
        }

        private void GoFun()
        {
            Row = "10";
            Col = "20";

        }
        private RelayCommand<Grid> _makeSelected;
        public RelayCommand<Grid> MakeSelected
        {
            get { return _makeSelected ?? (_makeSelected = new RelayCommand<Grid>(MakeSelectedExecute)); }
        }
        private void MakeSelectedExecute(Grid myGrid)
        {
            MyGrid = myGrid;
            Point mousePos = Mouse.GetPosition(MyGrid);
            getGridRowCol(mousePos.X, mousePos.Y);
            createBlock(selectedRowIndex, selectedColumnIndex, 4, 1);
        }
        private void getGridRowCol(double x, double y)
        {
            var grid = MyGrid;
            if (grid != null)
            {
                var temp = x;
                for (var i = 0; i < grid.ColumnDefinitions.Count; i++)
                {
                    var colDef = grid.ColumnDefinitions[i];
                    temp -= colDef.ActualWidth;
                    if (temp <= -1)
                    {
                        selectedColumnIndex = i;
                        break;
                    }
                }
                temp = y;
                for (var i = 0; i < grid.RowDefinitions.Count; i++)
                {
                    var rowDef = grid.RowDefinitions[i];
                    temp -= rowDef.ActualHeight;
                    if (temp <= -1)
                    {
                        selectedRowIndex = i;
                        break;
                    }
                }
            }
        }
        private void createBlock(int row, int col, int blockWidth, int blockHeight)
        {
            int numberOfBlocksLabel;
            int numberOfBlocksBox;
            LabelTextBox ltb = new LabelTextBox();

            ltb.txb.Text = "Label";
            ltb.Block_Row = row;
            ltb.Block_Col = col;
            ltb.Id = System.DateTime.Now.Millisecond.ToString();

            Size s = MeasureTextSize(ltb.txb.Text, ltb);
            numberOfBlocksLabel = (int)Math.Ceiling((double)(Math.Ceiling(s.Width) / WIDTH));
            numberOfBlocksBox = blockWidth - numberOfBlocksLabel;

            ltb.Width = blockWidth * WIDTH;
            ltb.Height = blockHeight * HEIGHT;

            ltb.txb.Width = WIDTH * numberOfBlocksLabel;
            ltb.txb.Height = HEIGHT * blockHeight;

            ltb.txt.Width = WIDTH * numberOfBlocksBox;
            ltb.txt.Height = HEIGHT * blockHeight;

            ltb.txt.BorderBrush = Brushes.Red;
            ltb.txt.BorderThickness = new Thickness(2, 2, 2, 2);

            //ltb.txt.TextChanged += blockTextChange;
            //ltb.PreviewKeyDown += selectInputBlock;
            //ltb.PreviewMouseDown += mouseClickInputBlock;

            Grid.SetRow(ltb, row);
            Grid.SetColumn(ltb, col - numberOfBlocksLabel);
            Grid.SetColumnSpan(ltb, ((int)(ltb.Width) / WIDTH));
            Grid.SetRowSpan(ltb, ((int)(ltb.Height) / HEIGHT));
            MyGrid.Children.Add(ltb);

            //var blockName = "BLOCK";
            //blockNameCount = blockNameCount + 1;
            //blockName = "BLOCK_" + blockNameCount;

            //daElement = new DrawAreaUiElement(ltb.Id, "INPUT_BLOCK", ltb.Block_Row.ToString(), ltb.Block_Col.ToString(), blockName, "TOKEN", "", "#TARGET", "#SOURCE", "", ltb.txt.Width.ToString(), "12", ltb.txt.Height.ToString());
            //daList.addUIElementToUIElementList(daElement);
            //DrawToJSON.DrawAreaToJSON(daList.UIL);

        }
        public static Size MeasureTextSize(string text, LabelTextBox ob)
        {
            FontFamily fontFamily = ob.txb.FontFamily;
            FontStyle fontStyle = ob.txb.FontStyle;
            FontWeight fontWeight = ob.txb.FontWeight;
            FontStretch fontStretch = ob.txb.FontStretch;
            double fontSize = ob.txb.FontSize;
            FormattedText ft = new FormattedText(text,
                                                 CultureInfo.CurrentCulture,
                                                 FlowDirection.LeftToRight,
                                                 new Typeface(fontFamily, fontStyle, fontWeight, fontStretch),
                                                 fontSize,
                                                 Brushes.Black);
            return new Size(ft.Width, ft.Height);
        }
    }
}
