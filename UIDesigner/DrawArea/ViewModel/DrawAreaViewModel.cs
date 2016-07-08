using DrawArea.View;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;

namespace DrawArea
{
    public class DrawAreaViewModel : ViewModelBase
    {
        int selectedColumnIndex = -1, selectedRowIndex = -1;
        const int WIDTH = 20;
        const int HEIGHT = 20;
        Grid myGrid;
        private void doSomething(Grid myGrid, MouseButtonEventArgs e)
        {
            var grid = myGrid;
            if (grid != null)
            {
                var pos = e.GetPosition(grid);
                var temp = pos.X;
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

                temp = pos.Y;
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
            var pos1 = e.GetPosition(grid);
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
        private void createBlock(int row, int col, int blockWidth, int blockHeight)
        {
            int numberOfBlocksLabel;
            int numberOfBlocksBox;
            LabelTextBox ltb = new LabelTextBox();

            ltb.txb.Text = "Label";

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

            ltb.txt.TextChanged += block_Text_Change;
            ltb.PreviewKeyDown += select_Input_Block;

            Grid.SetRow(ltb, row);
            Grid.SetColumn(ltb, col - numberOfBlocksLabel);
            Grid.SetColumnSpan(ltb, ((int)(ltb.Width) / WIDTH));
            Grid.SetRowSpan(ltb, ((int)(ltb.Height) / HEIGHT));
            myGrid.Children.Add(ltb);
        }

        //private void modifyBlock(LabelTextBox ltb)
        //{
        //    if (ltb.txt.Text.Length <= 5)
        //    {
        //        int numberOfBlocksLabel;
        //        int numberOfBlocksBox;
        //        int numberOfBlocksBlock;
        //        int widthDiff;
        //        int col = Grid.GetColumn(ltb);

        //        numberOfBlocksBlock = (int)ltb.Width / WIDTH;
        //        numberOfBlocksBox = (int)ltb.txt.Width / WIDTH;
        //        widthDiff = numberOfBlocksBlock - numberOfBlocksBox;

        //        Size s = MeasureTextSize(ltb.txb.Text, ltb);
        //        numberOfBlocksLabel = (int)Math.Ceiling((double)(Math.Ceiling(s.Width) / WIDTH));
        //        numberOfBlocksBox = (int)ltb.Width - numberOfBlocksLabel;

        //        ltb.Width = (numberOfBlocksLabel + numberOfBlocksBox) * WIDTH;
        //        ltb.txb.Width = WIDTH * numberOfBlocksLabel;
        //        ltb.txt.Width = WIDTH * numberOfBlocksBox;

        //        col -= numberOfBlocksLabel;

        //        if (col <= 0)
        //        {
        //            col = Grid.GetColumn(ltb);
        //        }

        //        Grid.SetColumn(ltb, col);
        //        Grid.SetColumnSpan(ltb, ((int)(ltb.Width) / WIDTH));
        //    }
        //}

        private void block_Text_Change(object sender, RoutedEventArgs e)
        {
            LabelTextBox ltb = new LabelTextBox();
            ltb.txt = (TextBox)sender;
            ltb.txb.Text = ltb.txt.Text;
        }

        private void select_Input_Block(object sender, KeyEventArgs e)
        {
            LabelTextBox ltb = (LabelTextBox)sender;
            int blockWidth = (int)ltb.Width;
            int blockHeight = (int)ltb.Height;
            int boxWidth = (int)ltb.txt.Width;
            int boxHeight = (int)ltb.txt.Height;
            int widthDiff = blockWidth - boxWidth;

            if (e.Key == Key.Delete)
            {
                myGrid.Children.Remove(ltb);
                return;
            }

            if (e.Key == Key.Left)
            {
                boxWidth -= WIDTH;
            }
            if (e.Key == Key.Right)
            {
                boxWidth += WIDTH;
            }
            if (e.Key == Key.Up)
            {
                boxHeight -= HEIGHT;
                blockHeight -= HEIGHT;
            }
            if (e.Key == Key.Down)
            {
                boxHeight += WIDTH;
                blockHeight += WIDTH;
            }
            if (e.Key >= Key.A && e.Key <= Key.Z)
            {
            }

            if (boxWidth <= 0)
                boxWidth = (int)ltb.txt.Width;
            if (boxHeight <= 0 || blockHeight <= 0)
            {
                blockHeight = (int)ltb.Height;
                boxHeight = (int)ltb.txt.Height;
            }

            ltb.Width = boxWidth + widthDiff;
            ltb.Height = blockHeight;

            Grid.SetColumnSpan(((LabelTextBox)sender), (boxWidth + widthDiff) / WIDTH);
            Grid.SetRowSpan(((LabelTextBox)sender), blockHeight / HEIGHT);

            ltb.txt.Width = boxWidth;
            ltb.txt.Height = boxHeight;

            Grid.SetColumnSpan(((LabelTextBox)sender).txt, boxWidth / WIDTH);
            Grid.SetRowSpan(((LabelTextBox)sender).txt, boxHeight / HEIGHT);
        }

        public void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e, Grid myGrid)
        {
            this.myGrid = myGrid;
            doSomething(this.myGrid, e);
            createBlock(selectedRowIndex, selectedColumnIndex, 4, 1);
        }
    }
}
