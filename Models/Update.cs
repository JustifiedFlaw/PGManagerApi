namespace PGManagerApi.Models
{
    public class Update
    {
        public Row Row { get; set; } = new Row();
        public Row Where { get; set; }
    }
}