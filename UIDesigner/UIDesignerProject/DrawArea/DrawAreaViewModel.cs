using DrawAreaToJSON;
using GalaSoft.MvvmLight.Command;
using JsonToDML;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DrawArea
{
    public partial class DrawAreaViewModel : ViewModelBase
    {
        public DrawAreaViewModel()
        {
            GridHeight = HEIGHT;
            GridWidth = WIDTH;
            
            CreateFileWatcher();

            inputBlockPropertiesViewModel = new InputBlockPropertiesViewModel();
            inputBlockPropertiesViewModel.ID = "";
            inputBlockPropertiesViewModel.Row = "";
            inputBlockPropertiesViewModel.Col = "";
            inputBlockPropertiesViewModel.inputBlockVisibility = Visibility.Hidden;
        }

        #region drawArea

            #region globals

                Grid MyGrid;
                int selectedColumnIndex = -1, selectedRowIndex = -1;
                const int WIDTH = 20;
                const int HEIGHT = 20;
                int blockNameCount = 0;
                int uid = 0;
                static DrawAreaUiElementList daList = new DrawAreaUiElementList();
                List<InputBlockPropertiesViewModel> viewModelList = new List<InputBlockPropertiesViewModel>();
                DrawAreaUiElement daElement;

            #endregion

            #region properties

                InputBlockPropertiesViewModel _inputBlockPropertiesViewModel;
                public InputBlockPropertiesViewModel inputBlockPropertiesViewModel
                {
                    get
                    { return _inputBlockPropertiesViewModel; }
                    set
                    {
                        if (_inputBlockPropertiesViewModel != value)
                        {
                            _inputBlockPropertiesViewModel = value;
                            RaisePropertyChangedEvent("inputBlockPropertiesViewModel");
                        }
                    }
                }

                int gridHeight;
                public int GridHeight
                {
                    get
                    { return gridHeight; }
                    set
                    {
                        if (gridHeight != value)
                        {
                            gridHeight = value;
                            RaisePropertyChangedEvent("GridHeight");
                        }
                    }
                }

                int gridWidth;
                public int GridWidth
                {
                    get
                    { return gridWidth; }
                    set
                    {
                        if (gridWidth != value)
                        {
                            gridWidth = value;
                            RaisePropertyChangedEvent("GridWidth");
                        }
                    }
                }

                //Visibility _outputBlockVisibility;
                //public Visibility outputBlockVisibility
                //{
                //    get
                //    {
                //        return _outputBlockVisibility;
                //    }
                //    set
                //    {
                //        if (_outputBlockVisibility != value)
                //        {
                //            _outputBlockVisibility = value;
                //            RaisePropertyChangedEvent("outputBlockVisibility");
                //        }
                //    }
                //}

            #endregion

            #region commands

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
            
            #endregion

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
                //uID = "";
                InputBlockPropertiesViewModel ob = new InputBlockPropertiesViewModel();
                
                int numberOfBlocksLabel;
                int numberOfBlocksBox;
                LabelTextBox ltb = new LabelTextBox();
                ltb.DataContext = ob;

                uid++;
                ob.ID = uid.ToString();
                ob.Row = row.ToString();
                ob.Col = col.ToString();
                ob.dmlKeyword = "INPUT_BLOCK";
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

                ltb.txt.TextChanged += blockTextChange;
                ltb.PreviewKeyDown += selectInputBlock;
                ltb.PreviewMouseDown += mouseClickInputBlock;

                Grid.SetRow(ltb, row);
                Grid.SetColumn(ltb, col - numberOfBlocksLabel);
                Grid.SetColumnSpan(ltb, ((int)(ltb.Width) / WIDTH));
                Grid.SetRowSpan(ltb, ((int)(ltb.Height) / HEIGHT));
                MyGrid.Children.Add(ltb);

                var blockName = "BLOCK";
                blockNameCount = blockNameCount + 1;
                blockName = "BLOCK_" + blockNameCount;

                daElement = new DrawAreaUiElement(ob.ID, "INPUT_BLOCK", ob.Row.ToString(), ob.Col.ToString(), blockName, "TOKEN", "", "#TARGET", "#SOURCE", "", ltb.txt.Width.ToString(), "12", ltb.txt.Height.ToString());
                daList.addUIElementToUIElementList(daElement);
                DrawToJSON.DrawAreaToJSON(daList.UIL);

                inputBlockPropertiesViewModel = ob;
                viewModelList.Add(ob);
            }
            private void blockTextChange(object sender, RoutedEventArgs e)
            {
                LabelTextBox ltb = new LabelTextBox();
                ltb.txt = (TextBox)sender;
                ltb.txb.Text = ltb.txt.Text;
            }
            //public void readPropertiesAndModifyUIElement()
            //{
            //    int i = 0, j = 0;
            //    int flag1 = 0;
            //    foreach (InputBlockPropertiesClass a in vmList)
            //    {
            //        if (a.ID == uID)
            //        {
            //            flag1 = 1;
            //            break;
            //        }
            //        i++;
            //    }
            //    if (flag1 == 1)
            //    {
            //        vmList.ElementAt(i).row = Row;
            //        foreach (LabelTextBox l in MyGrid.Children)
            //        {
            //            if (l.tID.Text == uID)
            //            {
            //                Grid.SetRow(l, int.Parse(Row));
            //                int blockWidth = (int)l.Width;
            //                int boxWidth = (int)l.txt.Width;
            //                int widthDiff = blockWidth - boxWidth;
            //                int calculatedCol = int.Parse(Col) - ((widthDiff / WIDTH));
            //                Grid.SetColumn(l, calculatedCol);
            //                vmList.ElementAt(i).row = Row;
            //                vmList.ElementAt(i).col = Col;
            //                modifyElement(vmList.ElementAt(i).ID, "", int.Parse(Row), int.Parse(Col), 0, 0);
            //                break;
            //            }
            //            j++;
            //        }
            //    }
            //}
            private void selectInputBlock(object sender, KeyEventArgs e)
            {
                LabelTextBox ltb = (LabelTextBox)sender;
                inputBlockPropertiesViewModel = (InputBlockPropertiesViewModel)ltb.DataContext;

                int blockWidth = (int)ltb.Width;
                int blockHeight = (int)ltb.Height;
                int boxWidth = (int)ltb.txt.Width;
                int boxHeight = (int)ltb.txt.Height;
                int widthDiff = blockWidth - boxWidth;
                
                int blockRow = int.Parse(inputBlockPropertiesViewModel.Row);
                int blockCol = int.Parse(inputBlockPropertiesViewModel.Col);

                if (e.Key == Key.Delete)
                {
                    MyGrid.Children.Remove(ltb);
                    modifyElement(inputBlockPropertiesViewModel.ID, "DELETE", 0, 0, 0, 0);
                    return;
                }

                if ((Keyboard.IsKeyDown(Key.RightCtrl)) && (Keyboard.IsKeyDown(Key.Up)))
                {
                    blockRow -= 1;
                    Grid.SetRow(ltb, blockRow);
                    inputBlockPropertiesViewModel.Row = blockRow.ToString();
                    modifyElement(inputBlockPropertiesViewModel.ID, "", blockRow, 0, 0, 0);
                    return;
                }

                if ((Keyboard.IsKeyDown(Key.RightCtrl)) && (Keyboard.IsKeyDown(Key.Down)))
                {
                    blockRow += 1;
                    Grid.SetRow(ltb, blockRow);
                    inputBlockPropertiesViewModel.Row = blockRow.ToString();
                    modifyElement(inputBlockPropertiesViewModel.ID, "", blockRow, 0, 0, 0);
                    return;
                }

                if ((Keyboard.IsKeyDown(Key.RightCtrl)) && (Keyboard.IsKeyDown(Key.Left)))
                {
                    blockCol -= ((widthDiff / WIDTH) + 1);
                    Grid.SetColumn(ltb, blockCol);
                    int tempCol = int.Parse(inputBlockPropertiesViewModel.Col) - 1;
                    inputBlockPropertiesViewModel.Col = tempCol.ToString();
                    modifyElement(inputBlockPropertiesViewModel.ID, "", 0, tempCol, 0, 0);

                    return;
                }

                if ((Keyboard.IsKeyDown(Key.RightCtrl)) && (Keyboard.IsKeyDown(Key.Right)))
                {
                    blockCol += (-(widthDiff / WIDTH) + 1);
                    //blockCol += 1;
                    Grid.SetColumn(ltb, blockCol);
                    int tempCol = int.Parse(inputBlockPropertiesViewModel.Col) + 1;
                    inputBlockPropertiesViewModel.Col = tempCol.ToString();
                    modifyElement(inputBlockPropertiesViewModel.ID, "", 0, tempCol, 0, 0);

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
                modifyElement(inputBlockPropertiesViewModel.ID, "", 0, 0, boxWidth, boxHeight);
            }
            private void mouseClickInputBlock(object sender, MouseEventArgs e)
            {
                LabelTextBox ltb = (LabelTextBox)sender;
                inputBlockPropertiesViewModel = (InputBlockPropertiesViewModel)ltb.DataContext;

                string dmlKeyword = inputBlockPropertiesViewModel.dmlKeyword;
                if (dmlKeyword == "INPUT_BLOCK")
                {
                    inputBlockPropertiesViewModel.inputBlockVisibility = Visibility.Visible;
                    //inputBlockPropertiesViewModel.outputBlockVisibility = Visibility.Hidden;
                   
                }
                else if (dmlKeyword == "OUTPUT_BLOCK")
                {
                    inputBlockPropertiesViewModel.inputBlockVisibility = Visibility.Hidden;
                    //outputBlockVisibility = Visibility.Visible;
                }
            }
            public void modifyElement(string blockId, string mode, int row, int col, int len, int height)
            {
                foreach (var element in daList.UIL)
                {
                    if (element.ID == blockId)
                    {
                        if (mode == "DELETE")
                            daList.removeUIElementToUIElementList(element);
                        else
                        {
                            if (row > 0)
                                element.Row = row.ToString();
                            if (col > 0)
                                element.Col = col.ToString();
                            else if ((len > 0) || height > 0)
                            {
                                element.Len = len.ToString();
                                element.Height = height.ToString();
                            }
                        }
                        break;
                    }
                }
                DrawToJSON.DrawAreaToJSON(daList.UIL);
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

        #endregion

        #region codeArea
        
            #region codeAreaGlobals

                DMLUIElementList ui;    //List of objects i.e. UI Elements
                DMLUIElement ue;        //Temporary object of UI Element that will be initialized for 
                string codeArea;

            #endregion

            #region properties

                public string CodeArea
                {
                    get { return codeArea; }
                    set
                    {
                        if (codeArea != value)
                        {
                            codeArea = value;
                            RaisePropertyChangedEvent("CodeArea");
                        }
                    }
                }

            #endregion

            public void CreateFileWatcher()
            {
                FileSystemWatcher watcher = new FileSystemWatcher();
                watcher.Path = "c:\\temp\\";
                watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                   | NotifyFilters.FileName | NotifyFilters.DirectoryName;
                watcher.Filter = "*.json";

                watcher.Changed += new FileSystemEventHandler(OnChanged);
                //watcher.Created += new FileSystemEventHandler(OnChanged);
                //watcher.Deleted += new FileSystemEventHandler(OnChanged);
                //watcher.Renamed += new RenamedEventHandler(OnRenamed);

                watcher.EnableRaisingEvents = true;
            }
            private void OnChanged(object source, FileSystemEventArgs e)
            {
                writeDML();
            }
            public void writeDML()
            {
                string json;
                try
                {
                    using (StreamReader r = new StreamReader("c:\\temp\\UIDesign.json"))
                    {
                        json = r.ReadToEnd();
                    }
                }
                catch (Exception)
                {
                    return;
                }

                ui = new DMLUIElementList();
                //delete the previous list and create a new one for any json file change
                //can be optimized

                dynamic dynObj = JsonConvert.DeserializeObject(json);

                foreach (var data in dynObj.UIElements)
                {
                    string dmlKeyword = data.DMLKeyword;
                    switch (dmlKeyword)
                    {
                        case DMLUIElementSyntax.InputBlock: ue = new InputBlock();
                            break;
                        case DMLUIElementSyntax.OutputBlock: ue = new OutputBlock();
                            break;
                        case DMLUIElementSyntax.Text: ue = new JsonToDML.Text();
                            break;
                        case DMLUIElementSyntax.Line: ue = new JsonToDML.Line();
                            break;
                        case DMLUIElementSyntax.ItemBlock: ue = new ItemBlock();
                            break;
                        case DMLUIElementSyntax.MenuBlock: ue = new MenuBlock();
                            break;
                        case DMLUIElementSyntax.PauseBlock: ue = new PauseBlock();
                            break;
                        case DMLUIElementSyntax.YesNoBlock: ue = new YesNoBlock();
                            break;
                    }

                    ue.setMembers(data);
                    ue.qualifiers.setMembers(data);

                    ui.addUIElementToUIElementList(ue);
                }
                refreshDisplayedDMLCode();
            }
            async public void  refreshDisplayedDMLCode()
            {
                GenerateDMLCode genDC;
                await Application.Current.Dispatcher.BeginInvoke(new Action(delegate
                {
                    CodeArea = "";
                    foreach (var ob in ui.UIL)
                    {
                        genDC = new GenerateDMLCode(ob);
                        CodeArea += genDC.serializeDML();
                    }
                }));
            }

        #endregion 

    }
}
