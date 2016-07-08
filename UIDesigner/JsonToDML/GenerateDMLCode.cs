using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace JsonToDML
{
    public class GenerateDMLCode
    {
        DMLUIElement ue;
        public GenerateDMLCode()
        {
            /*
             * This Class will help generate the DML code for 
             * the UI Element sent to it
             */
        }
        public GenerateDMLCode(DMLUIElement ue)
        {
            this.ue = ue;
        }
        public string serializeDML()
        {
            string dmlCode = "";
            switch(ue.DMLKeyword)
            {
                case DMLUIElementSyntax.InputBlock:
                    dmlCode += ue.DMLKeyword + hardCodedString.Spacebar + ue.BlockName + hardCodedString.Spacebar;
                    dmlCode += hardCodedString.Row + ue.Row + hardCodedString.Spacebar;
                    dmlCode += hardCodedString.Col + ue.Col + hardCodedString.Spacebar;

                    if (ue.qualifiers.Len != null)
                    {
                        dmlCode += hardCodedString.Len + ue.qualifiers.Len + hardCodedString.Spacebar;
                    }
                    if(ue.qualifiers.DisplayLength != null)
                    {
                        dmlCode += hardCodedString.DisplayLength + ue.qualifiers.DisplayLength + hardCodedString.Spacebar;
                    }
                    if(ue.qualifiers.Height != null)
                    {
                        dmlCode += hardCodedString.Height + ue.qualifiers.Height + hardCodedString.Spacebar;
                    }
                    dmlCode += hardCodedString.DMLNewLine;

                    dmlCode += hardCodedString.Target + ue.Target + hardCodedString.Spacebar;
                    
                    //at the end
                    dmlCode += hardCodedString.NewLine;
                    break;

                case DMLUIElementSyntax.OutputBlock:
                    dmlCode += ue.DMLKeyword + hardCodedString.Spacebar + ue.BlockName + hardCodedString.Spacebar;
                    dmlCode += hardCodedString.Row + ue.Row + hardCodedString.Spacebar;
                    dmlCode += hardCodedString.Col + ue.Col + hardCodedString.Spacebar;

                    if (ue.qualifiers.Len != null)
                    {
                        dmlCode += hardCodedString.Len + ue.qualifiers.Len + hardCodedString.Spacebar;
                    }
                    if(ue.qualifiers.DisplayLength != null)
                    {
                        dmlCode += hardCodedString.DisplayLength + ue.qualifiers.DisplayLength + hardCodedString.Spacebar;
                    }
                    if(ue.qualifiers.Height != null)
                    {
                        dmlCode += hardCodedString.Height + ue.qualifiers.Height + hardCodedString.Spacebar;
                    }
                    dmlCode += hardCodedString.DMLNewLine;
                    dmlCode += hardCodedString.Source + ue.Source + hardCodedString.Spacebar;

                    //at the end
                    dmlCode += hardCodedString.NewLine;
                    break;
                default: 
                    dmlCode = "Logic for type " + ue.DMLKeyword + " not implemented in GenerateDMLCode.cs\n";
                    break;
            }
            dmlCode += "\n";
            return dmlCode;
        }
    }
}
