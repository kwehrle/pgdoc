/********************************************************************************
Copyright (C) Binod Nepal, Mix Open Foundation (http://mixof.org).

This file is part of MixERP.

MixERP is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

MixERP is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with MixERP.  If not, see <http://www.gnu.org/licenses/>.
***********************************************************************************/
namespace MixERP.Net.Utilities.PgDoc.Models
{
    public sealed class PgType : PgBase
    {
		public long RowNumber { get; set; }
        public string BaseType { get; set; }
        public string Owner { get; set; }
        public string Collation { get; set; }
        public string Default { get; set; }
        public string Type { get; set; }
        public string StoreType { get; set; }
        public bool NotNull { get; set; }
        public string Description { get; set; }
        public string Definition { get; set; }

		public PgType() { }
		override public PgBase Convert(System.Data.DataRow row)
		{ 
            RowNumber = Conversion.TryCastLong(row["row_number"]);
            SchemaName = Conversion.TryCastString(row["schema_name"]);
            Name = Conversion.TryCastString(row["type_name"]);
            BaseType = Conversion.TryCastString(row["base_type"]);
            Owner = Conversion.TryCastString(row["owner"]);
            Collation = Conversion.TryCastString(row["collation"]);
            Default = Conversion.TryCastString(row["default"]);
            Type = Conversion.TryCastString(row["type"]);
            StoreType = Conversion.TryCastString(row["store_type"]);
            NotNull = Conversion.TryCastBoolean(row["not_null"]);
            Definition = Conversion.TryCastString(row["definition"]);
			Description = Conversion.TryCastString(row["description"]);
			return this as PgBase;
 		}
		override public string Parse(string template)
		{
			return template
				.Replace("{{type.name}}", Name)
                .Replace("{{type.rowNumber}}", RowNumber.ToString())
                .Replace("{{type.schema}}", SchemaName)
                .Replace("{{type.baseType}}", BaseType)
                .Replace("{{type.owner}}", Owner)
                .Replace("{{type.collation}}", Collation)
                .Replace("{{type.default}}", Default)
                .Replace("{{type.type}}", Type)
                .Replace("{{type.storeType}}", StoreType)
                .Replace("{{type.definition}}", Definition)
                .Replace("{{type.notNull}}", NotNull.ToString())
				.Replace("{{type.description}}", md.Transform(Description));
		}
    }
}