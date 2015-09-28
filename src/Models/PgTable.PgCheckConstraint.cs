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
		public sealed class PgCheckConstraint : PgBase
		{
			public string TableName { get; set; }
			public bool Deferrable { get; set; }
			public bool Deferred { get; set; }
			public bool IsLocal { get; set; }
			public bool NoInherit { get; set; }
			public string Definition { get; set; }
			public string Description { get; set; }

			public PgCheckConstraint() { }

			override public PgBase Convert(System.Data.DataRow row)
			{
				SchemaName = Conversion.TryCastString(row["nspname"]);
				TableName = Conversion.TryCastString(row["relname"]);
				Name = Conversion.TryCastString(row["constraint_name"]);
				Definition = Conversion.TryCastString(row["definition"]);
				Description = Conversion.TryCastString(row["description"]);
				Deferrable = Conversion.TryCastBoolean(row["deferrable"]);
				Deferred = Conversion.TryCastBoolean(row["deferred"]);
				IsLocal = Conversion.TryCastBoolean(row["is_local"]);
				NoInherit = Conversion.TryCastBoolean(row["no_inherit"]);

				return this as PgBase;
			}

			override public string Parse(string template)
			{
				return template
					.Replace("{{check.name}}", Name)
					.Replace("{{check.schema}}", SchemaName)
					.Replace("{{check.table}}", TableName)
					.Replace("{{check.definition}}", Definition)
					.Replace("{{check.deferrable}}", Deferrable.ToString())
					.Replace("{{check.deferred}}", Deferred.ToString())
					.Replace("{{check.isLocal}}", IsLocal.ToString())
					.Replace("{{check.noInherit}}", NoInherit.ToString())
					.Replace("{{check.description}}", md.Transform(Description));
			}
		}
	}
}