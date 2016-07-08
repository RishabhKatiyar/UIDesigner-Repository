using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrawAreaToJSON;
using JsonToDML;
using System.IO;
using Newtonsoft.Json;
namespace TestingConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            BlockInput element = new BlockInput();
            element.ID = "1";
            element.DMLKeyword = "INPUT_BLOCK";
            element.BlockName = "COMPANY_CODE_IN";
            element.Row = "20";
            element.Col = "3";
            element.Len = "38";
            element.DisplayLength = "2";
            element.Height = "44";
            element.Target = "#COMPANY_CODE";

            BlockInput element2 = new BlockInput();
            element2.ID = "2";
            element2.DMLKeyword = "OUTPUT_BLOCK";
            element2.BlockName = "PART_CODE_OUT";
            element2.Row = "3";
            element2.Col = "4";
            element2.Len = "2";
            element2.DisplayLength = "10";
            element2.Height = "1";
            element2.Target = "#PART_CODE";
            //InputBlock element = new InputBlock();
            //element.BlockName = "input_block";
            //element.Col = 7;
            //element.DMLKeyword = "dml_keyword";
            //element.EndRow = 8;
            //element.Facility = "pop_t_012";
            //element.qualifiers.DisplayLength = "7";
            //element.qualifiers.Height = "2";
            //element.qualifiers.Len = "6";
            //DMLUIElementList elementlist = new DMLUIElementList();
            //elementlist.addUIElementToUIElementList((DMLUIElement)element);

            //DrawToJSON drawobject = new DrawToJSON();
            //drawobject.DrawAreaToJSON(elementlist);
            //Movie obj = new Movie();
            //obj.Name = "The Name";
            //obj.Year = 2016;
            using (StreamWriter file = File.CreateText(@"c:\temp\UIDesign.txt"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, element);
                serializer.Serialize(file, element2);
            }
            string stringy = "{\n\"UIElements\":\n";
            stringy += "[\n";
            stringy += JsonConvert.SerializeObject(element);
            stringy += ", \n";
            stringy += JsonConvert.SerializeObject(element2);
            stringy += "\n]\n}";
            File.WriteAllText(@"c:\temp\UIDesign.json", stringy);
            Console.WriteLine("File Written : Check at c:\\temp\\UIDesign.txt");
            Console.ReadKey();
        }
    }
    public class BlockInput
    {
        public string ID { get; set; }
        public string DMLKeyword { get; set; }

        public string BlockName { get; set; }
        public string Row { get; set; }

        public string Col { get; set; }

        public string Len { get; set; }
        public string DisplayLength { get; set; }

        public string Height { get; set; }

        public string Source { get; set; }

        public string Target { get; set; }

    }
}
