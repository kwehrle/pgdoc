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
	public sealed partial class PgTable: PgBase
	{
		public sealed class PgColumn : PgBase
		{
			public int OrdinalPosition { get; set; }
			public string TableName { get; set; }
			public string DefaultValue { get; set; }
			public string DataType { get; set; }
			public bool IsNullable { get; set; }
			public int MaxLength { get; set; }
			public string Description { get; set; }
			public bool IsPrimaryKey { get; set; }
			public string PrimaryKeyConstraintName { get; set; }
			public PgForeignKey foreignKey { get; set; }

			public PgColumn() { }
			override public PgBase Convert(System.Data.DataRow row)
			{
				SchemaName = Conversion.TryCastString(row["table_schema"]);
				TableName = Conversion.TryCastString(row["table_name"]);
				Name = Conversion.TryCastString(row["column_name"]);
				OrdinalPosition = Conversion.TryCastInteger(row["ordinal_position"]);
				DefaultValue = Conversion.TryCastString(row["column_default"]);
				DataType = Conversion.TryCastString(row["data_type"]);
				IsNullable = Conversion.TryCastBoolean(row["is_nullable"]);
				MaxLength = Conversion.TryCastInteger(row["character_maximum_length"]);
				Description = Conversion.TryCastString(row["description"]);
				PrimaryKeyConstraintName = Conversion.TryCastString(row["key"]);
				if (Conversion.TryCastString(row["constraint_name"]).Length > 0) {
					foreignKey = new PgForeignKey();
					foreignKey.Convert(row);
				}
				
				IsPrimaryKey = !string.IsNullOrWhiteSpace(PrimaryKeyConstraintName);
				return this as PgBase;
			}
			override public string Parse(string template)
			{
				string indicator = "<i class='disabled ellipsis vertical icon'></i>";
				string keyIndicatorCssClass = string.Empty;

				if (IsPrimaryKey) {
					keyIndicatorCssClass = " class='error'";
					indicator = "<i class='red key icon' title='Primary Key'></i>";
				}
				if (foreignKey != null) {
					keyIndicatorCssClass = " class='warning'";
					indicator = "<i class='yellow location arrow icon' title='Foreign Key'></i>";
				}
				//string nullable = string.Format("<input type='checkbox' disabled='disabled'{0} title='This is a {1}NULLABLE column.' />", IsNullable ? " checked='checked'" : "", IsNullable ? "" : "NON ");
				string nullable = string.Format("<i class='radio{0} icon' title='{1}'></i>", IsNullable ? " selected": string.Empty, IsNullable ? "nullable" : "not nullable");

				var dt = DataType;
				if (MaxLength > 0)
					dt += string.Format(" ({0})", MaxLength);

				return template
					.Replace("{{col.name}}", Name)
						.Replace("{{col.ordinalPosition}}", OrdinalPosition.ToString())
						.Replace("{{col.isNullable}}", nullable)
						.Replace("{{col.dataType}}", dt)
						.Replace("{{col.indicator}}", indicator)
						.Replace("{{col.defaultValue}}", DefaultValue)
						.Replace("{{col.keyIndicatorCssClass}}", keyIndicatorCssClass)
						.Replace("{{col.description}}", PgBase.TransformMarkDown(Description));
			}
		}
	}
}