namespace PGManagerApi.Models
{
    public class Update
    {
        public FieldTypes FieldTypes { get; set; } = new FieldTypes();
        public Row Row { get; set; } = new Row();
        public Row Where { get; set; }
    }
}