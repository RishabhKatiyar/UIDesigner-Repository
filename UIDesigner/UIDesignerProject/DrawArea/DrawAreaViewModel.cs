using DrawAreaToJSON;
using GalaSoft.MvvmLight.Command;
using JsonToDML;
using Microsoft.Practices.Prism.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

            inputBlock = JsonToDML.DMLUIElementName.InputBlock;
            outputBlock = JsonToDML.DMLUIElementName.OutputBlock;
            pauseBlock = JsonToDML.DMLUIElementName.PauseBlock;
            currentBlockType = JsonToDML.DMLUIElementSyntax.InputBlock;

            CreateFileWatcher();

            inputBlockPropertiesViewModel = new InputBlockPropertiesViewModel();
            inputBlockPropertiesViewModel.inputBlockVisibility = Visibility.Hidden;

            PropertyChanged += (obj, args) =>
            { System.Console.WriteLine("Property " + args.PropertyName + " changed"); };

            _oldRow = inputBlockPropertiesViewModel.Row;
            _timer = new System.Timers.Timer();
            //_timer.AutoReset = false;
            //interval set to 1 second
            _timer.Interval = 100;
            _timer.Elapsed += timerElapsed;
            _timer.Start();
        }

        private void timerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (_oldRow != inputBlockPropertiesViewModel.Row || _oldCol != inputBlockPropertiesViewModel.Col)
            {
                _oldRow = inputBlockPropertiesViewModel.Row;
                _oldCol = inputBlockPropertiesViewModel.Col;
                Application.Current.Dispatcher.Invoke(() => readPropertiesAndModifyUIElement());
            }
            //because we did _timer.AutoReset = false; we need to manually restart the timer.
            _timer.Start();
        }

        #region drawArea

            #region globals

                Grid MyGrid;
                int selectedColumnIndex = -1, selectedRowIndex = -1;
                const int WIDTH = 20;
                const int HEIGHT = 20;
                int blockNameCount = 0;
                int uid = 0;
                DrawAreaUiElementList daList = new DrawAreaUiElementList();
                List<InputBlockPropertiesViewModel> viewModelList = new List<InputBlockPropertiesViewModel>();
                DrawAreaUiElement daElement;
                private System.Timers.Timer _timer;
                string _oldRow;
                string _oldCol;
                string currentBlockType;
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

                string _inputBlock;
                public string inputBlock
                {
                    get { return _inputBlock; }
                    set
                    {
                        if (_inputBlock != value)
                        {
                            _inputBlock = value;
                            RaisePropertyChangedEvent("inputBlock");
                        }
                    }
                }

                string _outputBlock;
                public string outputBlock
                {
                    get { return _outputBlock; }
                    set
                    {
                        if (_outputBlock != value)
                        {
                            _outputBlock = value;
                            RaisePropertyChangedEvent("outputBlock");
                        }
                    }
                }

                string _pauseBlock;
                public string pauseBlock
                {
                    get { return _pauseBlock; }
                    set
                    {
                        if (_pauseBlock != value)
                        {
                            _pauseBlock = value;
                            RaisePropertyChangedEvent("pauseBlock");
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

                private RelayCommand<string> _toolBoxCommand;
                public RelayCommand<string> toolBoxCommand
                {
                    get { return _toolBoxCommand ?? (_toolBoxCommand = new RelayCommand<string>(toolBoxFun)); }
                }

                private void toolBoxFun(string blockName)
                {
                    switch(blockName)
                    {
                        case JsonToDML.DMLUIElementName.InputBlock: currentBlockType = JsonToDML.DMLUIElementSyntax.InputBlock;
                            break;
                        case JsonToDML.DMLUIElementName.OutputBlock: currentBlockType = JsonToDML.DMLUIElementSyntax.OutputBlock;
                            break;
                        case JsonToDML.DMLUIElementName.PauseBlock: currentBlockType = JsonToDML.DMLUIElementSyntax.PauseBlock;
                            break;
                    }
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
                if (currentBlockType == JsonToDML.DMLUIElementSyntax.InputBlock)
                {
                    InputBlockPropertiesViewModel ob = new InputBlockPropertiesViewModel();
                
                    int numberOfBlocksLabel;
                    int numberOfBlocksBox;
                    LabelTextBox ltb = new LabelTextBox();
                    ltb.DataContext = ob;

                    uid++;
                    ob.ID = uid.ToString();
                    ob.Row = row.ToString();
                    ob.Col = col.ToString();
                    ob.dmlKeyword = currentBlockType;
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

                    daElement = new DrawAreaUiElement(ob.ID, currentBlockType, ob.Row.ToString(), ob.Col.ToString(), blockName, "TOKEN", "", "#TARGET", "#SOURCE", "", ltb.txt.Width.ToString(), "12", ltb.txt.Height.ToString());
                    daList.addUIElementToUIElementList(daElement);
                    DrawToJSON.DrawAreaToJSON(daList.UIL);

                    inputBlockPropertiesViewModel = ob;
                    viewModelList.Add(ob);
                }
            }
            private void blockTextChange(object sender, RoutedEventArgs e)
            {
                LabelTextBox ltb = new LabelTextBox();
                ltb.txt = (TextBox)sender;
                ltb.txb.Text = ltb.txt.Text;
            }
            public void readPropertiesAndModifyUIElement()
            {
                var ob = inputBlockPropertiesViewModel;
                if (MyGrid != null)
                {
                    foreach (LabelTextBox child in MyGrid.Children)
                    {
                        if (child.DataContext == ob)
                        {
                            Grid.SetRow(child, int.Parse(inputBlockPropertiesViewModel.Row));
                            int blockWidth = (int)child.Width;
                            int boxWidth = (int)child.txt.Width;
                            int widthDiff = blockWidth - boxWidth;
                            int calculatedCol = int.Parse(inputBlockPropertiesViewModel.Col) - ((widthDiff / WIDTH));
                            Grid.SetColumn(child, calculatedCol);
                            modifyElement(inputBlockPropertiesViewModel.ID, "", int.Parse(inputBlockPropertiesViewModel.Row), int.Parse(inputBlockPropertiesViewModel.Col), 0, 0);
                        }
                    }
                }
            }
            private void selectInputBlock(object sender, KeyEventArgs e)
            {
                LabelTextBox ltb = (LabelTextBox)sender;
                //LTB = ltb;
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
                if (dmlKeyword == JsonToDML.DMLUIElementSyntax.InputBlock)
                {
                    inputBlockPropertiesViewModel.inputBlockVisibility = Visibility.Visible;
                    //inputBlockPropertiesViewModel.outputBlockVisibility = Visibility.Hidden;
                   
                }
                else if (dmlKeyword == JsonToDML.DMLUIElementSyntax.OutputBlock)
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
