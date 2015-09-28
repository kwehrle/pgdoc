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
    public sealed class PgView : PgBase
    {
		public string Description { get; set; }
		public bool IsMaterialized { get; set; }
        public string Owner { get; set; }
        public long RowNumber { get; set; }
        public string Definition { get; set; }
        public string Tablespace { get; set; }

		public PgView(){}
		override public PgBase Convert(System.Data.DataRow row)
		{
			RowNumber = Conversion.TryCastLong(row["row_number"]);
			Name = Conversion.TryCastString(row["object_name"]);
			SchemaName = Conversion.TryCastString(row["object_schema"]);
			Tablespace = Conversion.TryCastString(row["tablespace"]);
			Owner = Conversion.TryCastString(row["owner"]);
			Definition = Conversion.TryCastString(row["definition"]);
			Description = Conversion.TryCastString(row["description"]);
			IsMaterialized = Conversion.TryCastBoolean(row["is_materialized"]);


			return this as PgBase;
		}
		override public string Parse(string template)
		{
			return template
				.Replace("{{view.name}}", Name)
                .Replace("{{view.schema}}", SchemaName)
                .Replace("{{view.rowNumber}}", RowNumber.ToString())
                .Replace("{{view.owner}}", Owner)
                .Replace("{{view.tablespace}}", Tablespace)
				.Replace("{{view.isMaterialized}}", IsMaterialized ? "YES" : "NO")
                .Replace("{{view.definition}}", Definition)
				.Replace("{{view.description}}", md.Transform(Description)
				);
		}
    }
}