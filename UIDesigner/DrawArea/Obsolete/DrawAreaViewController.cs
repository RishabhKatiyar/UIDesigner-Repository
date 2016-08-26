//using DrawArea.View;
//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Input;
//using System.Windows.Media;
//using Microsoft.Practices.Prism.Commands;
//using DrawAreaToJSON;

//namespace DrawArea
//{
//    public partial class DrawAreaViewController
//    {
//        Grid myGrid;
//        StackPanel stk;
//        InputBlockProperties ibp;
//        OutputBlockProperties obp;
//        static int blockNameCount = 0;
//        int selectedColumnIndex = -1, selectedRowIndex = -1;
//        const int WIDTH = 20;
//        const int HEIGHT = 20;
//        static DrawAreaUiElementList daList = new DrawAreaUiElementList();
//        DrawAreaUiElement daElement;
//        public void OnMouseLeftButtonDown(MouseButtonEventArgs e, Grid myGrid, StackPanel stk)
//        {
//            this.myGrid = myGrid;
//            this.stk = stk;
//            getGridRowCol(this.myGrid, e);
//            createBlock(selectedRowIndex, selectedColumnIndex, 4, 1);
//        }
//        private void getGridRowCol(Grid myGrid, MouseButtonEventArgs e)
//        {
//            var grid = myGrid;
//            if (grid != null)
//            {
//                var pos = e.GetPosition(grid);
//                var temp = pos.X;
//                for (var i = 0; i < grid.ColumnDefinitions.Count; i++)
//                {
//                    var colDef = grid.ColumnDefinitions[i];
//                    temp -= colDef.ActualWidth;
//                    if (temp <= -1)
//                    {
//                        selectedColumnIndex = i;
//                        break;
//                    }
//                }

//                temp = pos.Y;
//                for (var i = 0; i < grid.RowDefinitions.Count; i++)
//                {
//                    var rowDef = grid.RowDefinitions[i];
//                    temp -= rowDef.ActualHeight;
//                    if (temp <= -1)
//                    {
//                        selectedRowIndex = i;
//                        break;
//                    }
//                }
//            }
//            var pos1 = e.GetPosition(grid);
//        }
//        private void createBlock(int row, int col, int blockWidth, int blockHeight)
//        {
//            int numberOfBlocksLabel;
//            int numberOfBlocksBox;
//            LabelTextBox ltb = new LabelTextBox();

//            ltb.txb.Text = "Label";
//            ltb.Block_Row = row;
//            ltb.Block_Col = col;
//            ltb.Id = System.DateTime.Now.Millisecond.ToString();

//            Size s = MeasureTextSize(ltb.txb.Text, ltb);
//            numberOfBlocksLabel = (int)Math.Ceiling((double)(Math.Ceiling(s.Width) / WIDTH));
//            numberOfBlocksBox = blockWidth - numberOfBlocksLabel;

//            ltb.Width = blockWidth * WIDTH;
//            ltb.Height = blockHeight * HEIGHT;

//            ltb.txb.Width = WIDTH * numberOfBlocksLabel;
//            ltb.txb.Height = HEIGHT * blockHeight;

//            ltb.txt.Width = WIDTH * numberOfBlocksBox;
//            ltb.txt.Height = HEIGHT * blockHeight;

//            ltb.txt.BorderBrush = Brushes.Red;
//            ltb.txt.BorderThickness = new Thickness(2, 2, 2, 2);

//            ltb.txt.TextChanged += blockTextChange;
//            ltb.PreviewKeyDown += selectInputBlock;
//            ltb.PreviewMouseDown += mouseClickInputBlock;

//            Grid.SetRow(ltb, row);
//            Grid.SetColumn(ltb, col - numberOfBlocksLabel);
//            Grid.SetColumnSpan(ltb, ((int)(ltb.Width) / WIDTH));
//            Grid.SetRowSpan(ltb, ((int)(ltb.Height) / HEIGHT));
//            myGrid.Children.Add(ltb);

//            var blockName = "BLOCK";
//            blockNameCount = blockNameCount + 1;
//            blockName = "BLOCK_" + blockNameCount;

//            daElement = new DrawAreaUiElement(ltb.Id, "INPUT_BLOCK", ltb.Block_Row.ToString(), ltb.Block_Col.ToString(), blockName, "TOKEN", "", "#TARGET", "#SOURCE", "", ltb.txt.Width.ToString(), "12", ltb.txt.Height.ToString());
//            daList.addUIElementToUIElementList(daElement);
//            DrawToJSON.DrawAreaToJSON(daList.UIL);

//        }
//        public static Size MeasureTextSize(string text, LabelTextBox ob)
//        {
//            FontFamily fontFamily = ob.txb.FontFamily;
//            FontStyle fontStyle = ob.txb.FontStyle;
//            FontWeight fontWeight = ob.txb.FontWeight;
//            FontStretch fontStretch = ob.txb.FontStretch;
//            double fontSize = ob.txb.FontSize;
//            FormattedText ft = new FormattedText(text,
//                                                 CultureInfo.CurrentCulture,
//                                                 FlowDirection.LeftToRight,
//                                                 new Typeface(fontFamily, fontStyle, fontWeight, fontStretch),
//                                                 fontSize,
//                                                 Brushes.Black);
//            return new Size(ft.Width, ft.Height);
//        }
//        private void blockTextChange(object sender, RoutedEventArgs e)
//        {
//            LabelTextBox ltb = new LabelTextBox();
//            ltb.txt = (TextBox)sender;
//            ltb.txb.Text = ltb.txt.Text;
//        }
//        private void selectInputBlock(object sender, KeyEventArgs e)
//        {
//            LabelTextBox ltb = (LabelTextBox)sender;
//            int blockWidth = (int)ltb.Width;
//            int blockHeight = (int)ltb.Height;
//            int boxWidth = (int)ltb.txt.Width;
//            int boxHeight = (int)ltb.txt.Height;
//            int widthDiff = blockWidth - boxWidth;

//            int blockRow = ltb.Block_Row;
//            int blockCol = ltb.Block_Col;

//            if (e.Key == Key.Delete)
//            {
//                myGrid.Children.Remove(ltb);
//                modifyElement(ltb.Id, "DELETE", 0, 0, 0, 0);
//                return;
//            }
//            if ((Keyboard.IsKeyDown(Key.RightCtrl)) && (Keyboard.IsKeyDown(Key.Up)))
//            {
//                blockRow -= 1;
//                Grid.SetRow(ltb, blockRow);
//                ltb.Block_Row -= 1;
//                modifyElement(ltb.Id, "", ltb.Block_Row, 0, 0, 0);
//                return;
//            }
//            if ((Keyboard.IsKeyDown(Key.RightCtrl)) && (Keyboard.IsKeyDown(Key.Down)))
//            {
//                blockRow += 1;
//                Grid.SetRow(ltb, blockRow);
//                ltb.Block_Row += 1;
//                modifyElement(ltb.Id, "", ltb.Block_Row, 0, 0, 0);
//                return;
//            }
//            if ((Keyboard.IsKeyDown(Key.RightCtrl)) && (Keyboard.IsKeyDown(Key.Left)))
//            {
//                blockCol -= ((widthDiff / WIDTH) + 1);
//                Grid.SetColumn(ltb, blockCol);
//                ltb.Block_Col -= 1;
//                modifyElement(ltb.Id, "", 0, ltb.Block_Col, 0, 0);
//                return;
//            }
//            if ((Keyboard.IsKeyDown(Key.RightCtrl)) && (Keyboard.IsKeyDown(Key.Right)))
//            {
//                blockCol += (-(widthDiff / WIDTH) + 1);
//                Grid.SetColumn(ltb, blockCol);
//                ltb.Block_Col += 1;
//                modifyElement(ltb.Id, "", 0, ltb.Block_Col, 0, 0);
//                return;
//            }

//            if (e.Key == Key.Left)
//            {
//                boxWidth -= WIDTH;
//            }
//            if (e.Key == Key.Right)
//            {
//                boxWidth += WIDTH;
//            }
//            if (e.Key == Key.Up)
//            {
//                boxHeight -= HEIGHT;
//                blockHeight -= HEIGHT;
//            }
//            if (e.Key == Key.Down)
//            {
//                boxHeight += WIDTH;
//                blockHeight += WIDTH;
//            }
//            if (e.Key >= Key.A && e.Key <= Key.Z)
//            {
//            }

//            if (boxWidth <= 0)
//                boxWidth = (int)ltb.txt.Width;
//            if (boxHeight <= 0 || blockHeight <= 0)
//            {
//                blockHeight = (int)ltb.Height;
//                boxHeight = (int)ltb.txt.Height;
//            }

//            ltb.Width = boxWidth + widthDiff;
//            ltb.Height = blockHeight;

//            Grid.SetColumnSpan(((LabelTextBox)sender), (boxWidth + widthDiff) / WIDTH);
//            Grid.SetRowSpan(((LabelTextBox)sender), blockHeight / HEIGHT);

//            ltb.txt.Width = boxWidth;
//            ltb.txt.Height = boxHeight;

//            Grid.SetColumnSpan(((LabelTextBox)sender).txt, boxWidth / WIDTH);
//            Grid.SetRowSpan(((LabelTextBox)sender).txt, boxHeight / HEIGHT);
//            modifyElement(ltb.Id, "", 0, 0, boxWidth, boxHeight);
//        }
//        private void mouseClickInputBlock(object sender, MouseEventArgs e)
//        {
//            DrawAreaUiElement Element = daElement;
//            LabelTextBox ltb = (LabelTextBox)sender;
//            foreach (var element in daList.UIL)
//            {
//                if (element.ID == ltb.Id)
//                {
//                    Element = element;
//                    break;
//                }
//            }

//            string dmlKeyword = Element.DMLKeyword;
//            if (dmlKeyword == "INPUT_BLOCK")
//            {
//                ibp = new InputBlockProperties();
//                ibp.blockName = Element.BlockName;
//                ibp.row = Element.Row.ToString();
//                ibp.column = Element.Col.ToString();
//                stk.Children.RemoveAt(2);
//                stk.Children.Add(ibp);
//            }
//            else if (dmlKeyword == "OUTPUT_BLOCK")
//            {
//                obp = new OutputBlockProperties();
//                stk.Children.RemoveAt(2);
//                stk.Children.Add(obp);
//            }
//        }
//        public void modifyElement(string blockId, string mode, int row, int col, int len, int height)
//        {
//            foreach (var element in daList.UIL)
//            {
//                if (element.ID == blockId)
//                {
//                    if (mode == "DELETE")
//                        daList.removeUIElementToUIElementList(element);
//                    else
//                    {
//                        if (row > 0)
//                            element.Row = row.ToString();
//                        else if (col > 0)
//                            element.Col = col.ToString();
//                        else if ((len > 0) || height > 0)
//                        {
//                            element.Len = len.ToString();
//                            element.Height = height.ToString();
//                        }
//                    }
//                    break;
//                }
//            }
//            DrawToJSON.DrawAreaToJSON(daList.UIL);
//        }
//    }
//}
