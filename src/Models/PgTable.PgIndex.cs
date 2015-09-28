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
		public sealed class PgIndex : PgBase
		{
			public string TableName { get; set; }
			public string Owner { get; set; }
			public string Type { get; set; }
			public string AccessMethod { get; set; }
			public string Definition { get; set; }
			public bool IsClustered { get; set; }
			public bool IsValid { get; set; }
			public string Description { get; set; }

			public PgIndex() { }

			override public PgBase Convert(System.Data.DataRow row)
			{
				SchemaName = Conversion.TryCastString(row["nspname"]);
				TableName = Conversion.TryCastString(row["relname"]);
				Name = Conversion.TryCastString(row["index_name"]);
				Owner = Conversion.TryCastString(row["owner"]);
				Type = Conversion.TryCastString(row["type"]);
				AccessMethod = Conversion.TryCastString(row["access_method"]);
				Definition = Conversion.TryCastString(row["definition"]);
				IsClustered = Conversion.TryCastBoolean(row["is_clustered"]);
				IsValid = Conversion.TryCastBoolean(row["is_valid"]);
				Description = Conversion.TryCastString(row["description"]);

				return this as PgBase;
			}

			override public string Parse(string template)
			{
				string indicator;
				switch (Type[0]) {
					case 'P':
						indicator = "<i class='red key icon' title='Primary Key'></i>";
						break;
					case 'U':
						indicator = "<i class='grid layout yellow icon' title='Unique Index'></i>";
						break;
					default:
						indicator = "<i class='grid layout icon' title='Index'></i>";
						break;
				}

				return template
					.Replace("{{idx.name}}", Name)
					.Replace("{{idx.indexSchema}}", SchemaName)
					.Replace("{{idx.type}}", Type)
					.Replace("{{idx.indicator}}", indicator)
					.Replace("{{idx.owner}}", Owner)
					.Replace("{{idx.definition}}", Definition)
					.Replace("{{idx.accessMethod}}", AccessMethod)
					.Replace("{{idx.isClustered}}", IsClustered.ToString())
					.Replace("{{idx.isValid}}", IsValid.ToString())
					.Replace("{{idx.description}}", md.Transform(Description));

			}
		}
	}
}