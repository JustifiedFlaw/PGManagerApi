using System;

namespace PGManagerApi.Models
{
    public class Table
    {
        public virtual string SchemaName { get; set; }
        public virtual string TableName { get; set; }

        public Table()
        {
        }

        public Table(string schemaName, string tableName)
        {
            this.SchemaName = schemaName;
            this.TableName = tableName;
        }

        public override bool Equals(object obj)
        {
            if (obj is Table)
            {
                var compareTo = obj as Table;
                return this.SchemaName == compareTo.SchemaName
                    && this.TableName == compareTo.TableName;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(SchemaName, TableName);
        }
    }
}