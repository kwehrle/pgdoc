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
using System.Linq;
using HtmlAgilityPack;

namespace MixERP.Net.Utilities.PgDoc.Helpers
{
    internal static class HtmlHelper
    {
        internal static List<string> GetMatch(string content)
        {
            if (!string.IsNullOrWhiteSpace(content))
            {
				var doc = new HtmlDocument();
				doc.LoadHtml(content);

				var nodes = doc.DocumentNode.SelectNodes("//comment()");

				if (nodes != null) {
					return (from node in nodes where !node.InnerText.StartsWith("<!DOCTYPE html>") select node.InnerText).ToList();
				}
			}

			return null;

        }

		/// <summary>
		/// find div with a specific id and remove it
		/// </summary>
		/// <param name="content"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		internal static string RemoveDiv(string content, string id)
		{
			if (!string.IsNullOrWhiteSpace(content)) {
				var doc = new HtmlDocument();
				doc.LoadHtml(content);

				var node = doc.DocumentNode.SelectSingleNode("//div[@id='" + id + "']");

				if (node != null) {
					node.ParentNode.RemoveChild(node); //node.Remove();
					return doc.DocumentNode.OuterHtml;
				}
			}

			return content;

		}
        internal static string RemoveComment(string content)
        {
            return content.Replace("<!--", "").Replace("-->", "").Replace(Environment.NewLine, "").Trim();
        }
    }
}