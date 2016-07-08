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
    public class DrawToJSON
    {
        public void DrawAreaToJSON(DMLUIElementList inputObj)
        {
            foreach (var iteration in inputObj)
            {
                // serialize JSON directly to a file
                using (StreamWriter file = File.CreateText(@"c:\temp\ROSSjson.json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, iteration.qualifiers);
                }
            }
        }
    }
}
