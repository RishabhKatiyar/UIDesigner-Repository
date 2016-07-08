using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonToDML
{
    public class DMLUIElementList : List<DMLUIElement>
    {
        List<DMLUIElement> uiL;
        public List<DMLUIElement> UIL
        {
            get { return uiL; }
        }
        public DMLUIElementList()
        {
            uiL = new List<DMLUIElement>();
        }
        public void addUIElementToUIElementList(DMLUIElement uie)
        {
            uiL.Add(uie);
        }
        /*
         * this class holds all the UI Elements
         * and keeps them in a list
         */
    }
}
