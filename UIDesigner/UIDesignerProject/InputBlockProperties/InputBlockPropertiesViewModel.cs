using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DrawArea
{
    public class InputBlockPropertiesViewModel : ViewModelBase
    {
        public InputBlockPropertiesViewModel()
        {
            this.PropertyChanged += MyViewModel_PropertyChanged;
        }
        void MyViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "SomeProperty":
                    // Do something
                    break;
            }
        }

        string id;
        public string ID
        {
            get
            { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    RaisePropertyChangedEvent("ID");
                }
            }
        }

        string row;
        public string Row
        {
            get
            { return row; }
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
            { return col; }
            set
            {
                if (col != value)
                {
                    col = value;
                    RaisePropertyChangedEvent("Col");
                }
            }
        }

        string _dmlKeyword;
        public string dmlKeyword
        {
            get
            { return _dmlKeyword; }
            set
            {
                if (_dmlKeyword != value)
                {
                    _dmlKeyword = value;
                    RaisePropertyChangedEvent("dmlKeyword");
                }
            }
        }

        Visibility _inputBlockVisibility;
        public Visibility inputBlockVisibility
        {
            get
            {
                return _inputBlockVisibility;
            }
            set
            {
                if (_inputBlockVisibility != value)
                {
                    _inputBlockVisibility = value;
                    RaisePropertyChangedEvent("inputBlockVisibility");
                }
            }
        }

    }
}
