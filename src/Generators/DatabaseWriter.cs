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
using System.Collections.Generic;
using System.Text;
using MixERP.Net.Utilities.PgDoc.Helpers;
using MixERP.Net.Utilities.PgDoc.Models;
using MixERP.Net.Utilities.PgDoc.Processors;


namespace MixERP.Net.Utilities.PgDoc.Generators
{
    internal class DatabaseWriter: BaseWriter<PgDatabase>
    {
		public void Run(PgDatabase db) 
		{
			base.WriteFile(db);

			Console.WriteLine("Writing schemas.");
			new SchemaWriter().WriteFile(db.Schemas);

			Console.WriteLine("Writing tables.");
			new TableWriter().WriteFile(db.Tables);

			Console.WriteLine("Writing views.");
			new BaseWriter<PgView>().WriteFile(db.Views);

			Console.WriteLine("Writing sequences.");
			new BaseWriter<PgSequence>().WriteFile(db.Sequences);

			Console.WriteLine("Writing functions.");
			new BaseWriter<PgFunction>().WriteFile(db.Functions);

			Console.WriteLine("Writing types.");
			new BaseWriter<PgType>().WriteFile(db.Types);
		}

		//override protected string BuildDocumentation(string content, IEnumerable<PgDatabase> dbs, IEnumerable<string> matches)
		//{
		//	throw new Exception("do not use this method");
		//}
		override protected string BuildDocumentation(string content, PgDatabase db, IEnumerable<string> matches = null)
		{
			content = content.Replace("{{db.comment}}", md.Transform(db.Comment));

			foreach (string match in matches)
			{
				string comment = HtmlHelper.RemoveComment(match);

				if (comment.StartsWith("PostgreSQLDatabase"))
				{
					comment = comment.Substring("PostgreSQLDatabase".Length);

					StringBuilder items = new StringBuilder();
					foreach (PgDatabase.PgSetting setting in db.Settings) {
						items.Append(setting.Parse(comment));
					}

					content = content.Replace(match, items.ToString());
				}

			}
			return StaticWriter.FillMaster(db.Name.ToUpper(), content); 
		}
    }
}