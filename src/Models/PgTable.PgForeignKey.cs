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
	public sealed partial class PgTable : PgBase
	{
		public sealed class PgForeignKey : PgBase
		{
			public string OriginColumnName { get; set; }
			public string TableName { get; set; }
			public string ColumnName { get; set; }

			public PgForeignKey() { }

			override public PgBase Convert(System.Data.DataRow row)
			{
				OriginColumnName = Conversion.TryCastString(row["column_name"]);
				Name = Conversion.TryCastString(row["constraint_name"]);
				SchemaName = Conversion.TryCastString(row["references_schema"]);
				TableName = Conversion.TryCastString(row["references_table"]);
				ColumnName = Conversion.TryCastString(row["references_field"]);
					
				return this as PgBase;
			}

			override public string Parse(string template)
			{
				return template
					.Replace("{{foreignkey.name}}", Name)
					.Replace("{{foreignkey.originColumnName}}", OriginColumnName)
					.Replace("{{foreignkey.schemaName}}", SchemaName)
					.Replace("{{foreignkey.tableName}}", TableName)
					.Replace("{{foreignkey.columnName}}", ColumnName);

			}
		}
	}
}