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
using System.Text;
using System.Linq;

namespace MixERP.Net.Utilities.PgDoc.Parsers
{
	using MixERP.Net.Utilities.PgDoc.Helpers;
	using MixERP.Net.Utilities.PgDoc.Models;

	internal static class Parser<T> where T : PgBase
    {
		internal static string Parse(string content, IEnumerable<string> matches, IEnumerable<T> objects)
        {
			var typeName = typeof(T).Name.Substring(2).ToLower();
			var repeater = typeName + " repeater:";
            foreach (string match in matches)
            {
                string comment = HtmlHelper.RemoveComment(match);

				if (comment.StartsWith(repeater))
                {
					comment = comment.Substring(repeater.Length);
					StringBuilder items = new StringBuilder();
					foreach (T obj in objects) {
						items.Append(obj.Parse(comment));
					} 
					content = content.Replace(match, items.ToString());
					if (items.Length == 0 && Program.RemoveEmpty) {
						content = HtmlHelper.RemoveDiv(content, typeName + "_container");
					}

					//content = content.Replace(match, objects.Aggregate(items, (current, fct) => current + fct.Parse(comment))); 
				}
            }
            return content;
        }
    }
}