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
    public class PgFunction : PgBase
    {
		public string Arguments { get; set; }
        public string Description { get; set; }
        public string FunctionType { get; set; }
        public string Owner { get; set; }
        public string ResultType { get; set; }
        public long RowNumber { get; set; }
        public string FunctionDefinition { get; set; }
        public int oid { get; set; }

		public PgFunction() { }

		override public PgBase Convert(System.Data.DataRow row)
		{
            oid = Conversion.TryCastInteger(row["oid"]);
            FunctionDefinition = Conversion.TryCastString(row["definition"]);
            RowNumber = Conversion.TryCastLong(row["row_number"]);
            Name = Conversion.TryCastString(row["function_name"]);
            SchemaName = Conversion.TryCastString(row["object_schema"]);
            Arguments = Conversion.TryCastString(row["arguments"]);
            ResultType = Conversion.TryCastString(row["result_type"]);
            FunctionType = Conversion.TryCastString(row["function_type"]);
            Owner = Conversion.TryCastString(row["owner"]);
			Description = Conversion.TryCastString(row["description"]);
			return this as PgBase;
		}

		override public string Parse(string template)
		{
			return template
				.Replace("{{fct.name}}", Name)
                .Replace("{{fct.triggerSchema}}", SchemaName)
                .Replace("{{fct.schema}}", SchemaName)
                .Replace("{{fct.arguments}}", Arguments.Replace(", ", "<br />"))
                .Replace("{{fct.rowNumber}}", RowNumber.ToString())
                .Replace("{{fct.owner}}", Owner)
                .Replace("{{fct.resultType}}", ResultType)
                .Replace("{{fct.functionType}}", FunctionType)
                .Replace("{{fct.oid}}", oid.ToString())
                .Replace("{{fct.definition}}", FunctionDefinition)
                .Replace("{{fct.description}}", md.Transform(Description));
		}
    }

	public class PgTriggerFunction : PgFunction { 
	}
}