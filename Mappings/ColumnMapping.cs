using FluentNHibernate.Mapping;
using PGManagerApi.Models;

namespace PGManagerApi.Mappings
{
    public class ColumnMapping : ClassMap<Column>
    {
        public ColumnMapping()
        {
            this.Schema("information_schema");
            this.Table("columns");

            this.CompositeId()
                .KeyProperty(x => x.SchemaName, "table_schema")
                .KeyProperty(x => x.TableName, "table_name")
                .KeyProperty(x => x.ColumnName, "column_name");
            
            this.Map(x => x.DataType, "data_type");
            this.Map(x => x.OrdinalPosition, "ordinal_position");
            this.Map(x => x.IsNullable, "is_nullable").CustomType(typeof(YesNoType));
            this.Map(x => x.CharacterMaximumLength, "character_maximum_length");
            this.Map(x => x.IsIdentity, "is_identity").CustomType(typeof(YesNoType));
            this.Map(x => x.IdentityGeneration, "identity_generation");
        }
    }
}