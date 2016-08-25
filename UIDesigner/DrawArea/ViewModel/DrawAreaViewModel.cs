using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace DrawArea.ViewModel
{
    class DrawAreaViewModel : INotifyPropertyChanged
    {
        Grid MyGrid;
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
            //_fieldViewModel.MarkCellsForTurn(this);
        }

    }
}
