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
    public sealed class PgSequence : PgBase
    {
		public string Owner { get; set; }
		public long RowNumber { get; set; }
        public string DataType { get; set; }
        public string Increment { get; set; }
        public string Description { get; set; }
        public string StartValue { get; set; }
        public string MinimumValue { get; set; }
        public string MaximumValue { get; set; }

		public PgSequence() { }

		override public PgBase Convert(System.Data.DataRow row)
		{
			RowNumber = Conversion.TryCastInteger(row["row_number"]);
			SchemaName = Conversion.TryCastString(row["sequence_schema"]);
			Name = Conversion.TryCastString(row["sequence_name"]);
			DataType = Conversion.TryCastString(row["data_type"]);
			Increment = Conversion.TryCastString(row["increment"]);
			Description = Conversion.TryCastString(row["description"]);
			Owner = Conversion.TryCastString(row["owner"]);
			StartValue = Conversion.TryCastString(row["start_value"]);
			MinimumValue = Conversion.TryCastString(row["minimum_value"]);
			MaximumValue = Conversion.TryCastString(row["maximum_value"]);

			return this as PgBase;
		}
		override public string Parse(string template)
		{
			return template
				.Replace("{{sequ.name}}", Name)
				.Replace("{{sequ.rowNumber}}", RowNumber.ToString())
				.Replace("{{sequ.schema}}", SchemaName)
				.Replace("{{sequ.owner}}", Program.omitOwner.Equals(Owner) ? Program.OwnerSubstitute : Owner)
				.Replace("{{sequ.dataType}}", DataType)
				.Replace("{{sequ.startValue}}", StartValue)
				.Replace("{{sequ.increment}}", Increment)
				.Replace("{{sequ.minimumValue}}", MinimumValue)
				.Replace("{{sequ.maximumValue}}", MaximumValue)
				.Replace("{{sequ.description}}", PgBase.TransformMarkDown(Description));				
		}
	}
}