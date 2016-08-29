namespace JsonToDML
{
    public class DMLQualifiers
    {
        public string Len
        { get; set; }
        public string DisplayLength
        { get; set; }
        public string Height
        { get; set; }
        
        /*  
         * Add any qualifier here
         * but use type string because
         * any other type will raise an exception 
         * if not found in json file 
        */
        public void setMembers(dynamic qualifierData)
        {
            Len             = qualifierData.Len;
            DisplayLength   = qualifierData.DisplayLength;
            Height          = qualifierData.Height;

            /*
             * Use same tag names as in json file to get correct data here   
             * e.g. "Len":3
             *      "NewQualifier":"Value"  
            */
        }

    }
}
