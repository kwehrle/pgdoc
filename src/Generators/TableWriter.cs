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
using System.Collections.Generic;
using MixERP.Net.Utilities.PgDoc.Helpers;
using MixERP.Net.Utilities.PgDoc.Models;
using MixERP.Net.Utilities.PgDoc.Processors;
using MixERP.Net.Utilities.PgDoc.Parsers;

namespace MixERP.Net.Utilities.PgDoc.Generators
{
    internal class TableWriter: BaseWriter<PgTable>
    {
		override protected string BuildDocumentation(string content, PgTable table, IEnumerable<string> matches)
		{
			content = Parser<PgTable.PgForeignKey>.Parse(content, matches, table.ForeignKeys);
			//content = DefaultParser.Parse(content, matches, table.Columns);

			content = Parser<PgTable.PgCheckConstraint>.Parse(content, matches, table.CheckConstraints);
			content = Parser<PgTable.PgIndex>.Parse(content, matches, table.Indices);
			content = Parser<PgTable.PgColumn>.Parse(content, matches, table.Columns);
			content = Parser<PgTable.PgTrigger>.Parse(content, matches, table.Triggers);

			return StaticWriter.FillMaster("Table " + table.Name, content.Replace(content, table.Parse(content)), 2);
		}
	}
}