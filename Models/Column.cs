using System;

namespace PGManagerApi.Models
{
    public class Column
    {
        public virtual string SchemaName { get; set; }
        public virtual string TableName { get; set; }
        public virtual string ColumnName { get; set; }
        public virtual string DataType { get; set; }
        public virtual int OrdinalPosition { get; set; }
        public virtual bool IsNullable { get; set; }
        public virtual int? CharacterMaximumLength { get; set; }

        public virtual string GetTypeString()
        {
            var type = $"{this.DataType}";

            if (this.CharacterMaximumLength > 0)
            {
                type += $"({this.CharacterMaximumLength})";
            }

            type += " " + (this.IsNullable ? "NULL" : "NOT NULL");

            return type;
        }

        public override bool Equals(object obj)
        {
            if (obj is Column)
            {
                var compareTo = obj as Column;
                return this.SchemaName == compareTo.SchemaName
                    && this.TableName == compareTo.TableName
                    && this.ColumnName == compareTo.ColumnName;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(SchemaName, TableName, ColumnName);
        }
    }
}