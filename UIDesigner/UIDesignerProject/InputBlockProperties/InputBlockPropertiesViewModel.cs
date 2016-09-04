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
        public delegate void ParameterChange(string parameterName);
        
        ParameterChange onParameterChange;
        public ParameterChange OnParameterChange 
        {
            get 
            { 
                return onParameterChange; 
            }
            set
            {
                if (onParameterChange != value)
                {
                    onParameterChange = value;
                    RaisePropertyChangedEvent("OnParameterChange");
                }
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
                    if (OnParameterChange != null)
                    {
                        OnParameterChange("Row");
                    }
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
                    if (OnParameterChange != null)
                    {
                        OnParameterChange("Col");
                    }
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
