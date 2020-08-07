using System;
using System.Collections.Generic;
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
using System.Collections.ObjectModel;

namespace MixERP.Net.Utilities.PgDoc.Models
{
    public sealed partial class PgTable : PgBase
    {
		public string Description { get; set; }
        public string Owner { get; set; }
        public long RowNumber { get; set; }
        public string Tablespace { get; set; }
		public IEnumerable<PgColumn> Columns { get; set; }
		public IEnumerable<PgTrigger> Triggers { get; set; }
		public IEnumerable<PgIndex> Indices { get; set; }
        public IEnumerable<PgCheckConstraint> CheckConstraints { get; set; }

		public IEnumerable<PgForeignKey> ForeignKeys { get; set; }
		public IEnumerable<PgForeignKey> Defaults { get; set; }

		public PgTable() { }
		override public PgBase Convert(System.Data.DataRow row)
		{ 
			RowNumber = Conversion.TryCastLong(row["row_number"]);
            Name = Conversion.TryCastString(row["object_name"]);
            SchemaName = Conversion.TryCastString(row["object_schema"]);
            Tablespace = Conversion.TryCastString(row["tablespace"]);
            Owner = Conversion.TryCastString(row["owner"]);
			Description = Conversion.TryCastString(row["description"]);
			return this as PgBase;
		}

		override public string Parse(string template)
		{
			return template
				.Replace("{{table.name}}", Name)
                .Replace("{{table.rowNumber}}", RowNumber.ToString())
				.Replace("{{table.schema}}", SchemaName)
				.Replace("{{table.owner}}", Program.omitOwner.Equals(Owner) ? Program.OwnerSubstitute : Owner)
				.Replace("{{table.space}}", Program.omitTablespace.Equals(Tablespace) ? Program.TablespaceSubstitute : Tablespace)
				.Replace("{{table.description}}", PgBase.TransformMarkDown(Description));				
		}
    }
}