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
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MixERP.Net.Utilities.PgDoc.Helpers;
using MixERP.Net.Utilities.PgDoc.Models;
using MixERP.Net.Utilities.PgDoc.Parsers;
using MixERP.Net.Utilities.PgDoc.Processors;

namespace MixERP.Net.Utilities.PgDoc.Generators
{
    internal class SchemaWriter: BaseWriter<PgSchema>
    {
		override protected string BuildDocumentation(string content, PgSchema schema, IEnumerable<string> matches)
		{
			var Tables = Program.db.Tables.Where(t => t.SchemaName == schema.Name); //new Processor<PgTable>().GetBySchema(schema.Name);
			var Views = Program.db.Views.Where(t => t.SchemaName == schema.Name); //new Processor<PgView>().GetBySchema(schema.Name);
			var Sequences = Program.db.Sequences.Where(t => t.SchemaName == schema.Name); //new Processor<PgSequence>().GetBySchema(schema.Name);
			var Functions = Program.db.Functions.Where(t => t.SchemaName == schema.Name); //new Processor<PgFunction>().GetBySchema(schema.Name);
			var Types = Program.db.Types.Where(t => t.SchemaName == schema.Name); //new Processor<PgType>().GetBySchema(schema.Name);
			content = content.Replace("{{DBName}}", Program.Database.ToUpperInvariant());
			content = content.Replace("{{SchemaName}}", schema.Name);

			content = Parser<PgTable>.Parse(content, matches, Tables);
			content = Parser<PgView>.Parse(content, matches, Views);
			content = Parser<PgSequence>.Parse(content, matches, Sequences);
			content = Parser<PgFunction>.Parse(content, matches, Functions);
			content = Parser<PgType>.Parse(content, matches, Types);

			foreach (PgTable table in Tables) {
				Console.WriteLine(" .. table \"{0}.{1}\".", schema.Name, table.Name);
				new TableWriter().WriteFile(table);
			}

			foreach (PgFunction function in Functions) {
				Console.WriteLine(" .. function \"{0}.{1}\".", schema.Name, function.Name);
				new BaseWriter<PgFunction>().WriteFile(function);
			}

			foreach (PgView view in Views) {
				Console.WriteLine(" .. view \"{0}.{1}\".", schema.Name, view.Name);
				new BaseWriter<PgView>().WriteFile(view);
			}
			foreach (PgType type in Types) {
				Console.WriteLine(" .. type \"{0}.{1}\".", schema.Name, type.Name);
				new BaseWriter<PgType>().WriteFile(type);
			}

			content = content.Replace("{{schema.description}}", md.Transform(schema.Description));

			return StaticWriter.FillMaster(schema.Name, content, 1);
		}
    }
}