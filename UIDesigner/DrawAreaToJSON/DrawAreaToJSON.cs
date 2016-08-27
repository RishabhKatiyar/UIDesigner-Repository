using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JsonToDML;
using Newtonsoft.Json;
using System.IO;

namespace DrawAreaToJSON
{
    public  class DrawToJSON
    {
        public static void DrawAreaToJSON(List<DrawAreaUiElement> elementsList)
        {
            string jsonText = "{\n\"UIElements\":\n";
            jsonText += "[\n";
            foreach (var element in elementsList)
            {
                // serialize JSON directly to a file
                using (StreamWriter file = File.CreateText(@"c:\temp\UIDesign.txt"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, element);
                    jsonText += JsonConvert.SerializeObject(element);
                    jsonText += ", \n";
                }
            }
            jsonText += "\n]\n}";
            File.WriteAllText(@"c:\temp\UIDesign.json", jsonText);
            //int t = await Task.Run(() => WriteToJson(elementsList));
        }

        public static int WriteToJson(List<DrawAreaUiElement> elementsList)
        {
            return 1;
        }
    }

    public class DrawAreaUiElement
    {
        public DrawAreaUiElement(string id,string dmlKeyword,string row,string col,string blockName,string tokenText,string endRow,string target,string source,string facility,string len,string displayLength,string height )
        {
            this.ID = id;
            this.DMLKeyword = dmlKeyword;
            this.Row = row;
            this.Col = col;
            this.BlockName = blockName;
            this.TokenText = tokenText;
            this.EndRow = endRow;
            this.Target = target;
            this.Source = source;
            this.Facility = facility;
            this.Len = len;
            this.DisplayLength = displayLength;
            this.Height = height;
        }
        public string ID
        { get; set; }
        public string DMLKeyword
        { get; set; }
        public string Row
        { get; set; }
        public string Col
        { get; set; }
        public string BlockName
        { get; set; }
        public string TokenText
        { get; set; }
        public string EndRow
        { get; set; }
        public string Target
        { get; set; }
        public string Source
        { get; set; }
        public string Facility
        { get; set; }

        /*
         * Add new UI Element's properties here .
         * Do not add qualifiers.
         * Try to use type string.
         */
        public string Len
        { get; set; }
        public string DisplayLength
        { get; set; }
        public string Height
        { get; set; }
    }

    public class DrawAreaUiElementList : List<DrawAreaUiElement>
    {
        List<DrawAreaUiElement> uiL;
        public List<DrawAreaUiElement> UIL
        {
            get { return uiL; }
        }
        public DrawAreaUiElementList()
        {
            uiL = new List<DrawAreaUiElement>();
        }
        public void addUIElementToUIElementList(DrawAreaUiElement uie)
        {
            uiL.Add(uie);
        }
        public void removeUIElementToUIElementList(DrawAreaUiElement uie)
        {
            uiL.Remove(uie);
        }
        /*
         * this class holds all the UI Elements
         * and keeps them in a list
         */
    }
}
