using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace DrawAreaToJSON
{
    public class DrawToJSON
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
}
