namespace JsonToDML
{
    abstract public class DMLUIElement
    {
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

        public DMLQualifiers qualifiers;
        public DMLUIElement()
        {
            qualifiers = new DMLQualifiers();
        }
        public void setMembers(dynamic elementProperties)
        {
            if (DMLKeyword != DMLUIElementSyntax.Text && DMLKeyword != DMLUIElementSyntax.Line)
            {
                BlockName = elementProperties.BlockName;
            }

            ID = elementProperties.ID;
            Row = elementProperties.Row;
            Col = elementProperties.Col;
            Target = elementProperties.Target;
            Source = elementProperties.Source;
            
            /*
             * Use same tag names as in json file to get correct data here   
             * e.g. "Row":3
             *      "NewProperty":"Value"  
            */
        }
    }
    public class Text : DMLUIElement
    {
        public Text()
            : base()
        { DMLKeyword = DMLUIElementSyntax.Text; }
    }
    public class Line : DMLUIElement
    {
        public Line()
            : base()
        { DMLKeyword = DMLUIElementSyntax.Line; }
    }
    public class InputBlock : DMLUIElement
    {
        public InputBlock()
            : base()
        { 
            DMLKeyword = DMLUIElementSyntax.InputBlock;
        }
    }
    public class OutputBlock : DMLUIElement
    {
        public OutputBlock()
            : base()
        { 
            DMLKeyword = DMLUIElementSyntax.OutputBlock;
        }
    }
    public class ItemBlock : DMLUIElement
    {
        public ItemBlock()
            : base()
        { DMLKeyword = DMLUIElementSyntax.ItemBlock; }
    }
    public class MenuBlock : DMLUIElement
    {
        public MenuBlock()
            : base()
        { DMLKeyword = DMLUIElementSyntax.MenuBlock; }
    }
    public class PauseBlock : DMLUIElement
    {
        public PauseBlock()
            : base()
        { DMLKeyword = DMLUIElementSyntax.PauseBlock; }
    }
    public class YesNoBlock : DMLUIElement
    {
        public YesNoBlock()
            : base()
        { DMLKeyword = DMLUIElementSyntax.YesNoBlock; }
    }
}
