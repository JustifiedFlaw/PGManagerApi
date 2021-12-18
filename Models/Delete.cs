namespace PGManagerApi.Models
{
    public class Delete
    {
        public FieldTypes FieldTypes { get; set; } = new FieldTypes();
        public Row Where { get; set; }
    }
}