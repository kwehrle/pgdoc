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
using MixERP.Net.Utilities.PgDoc.Helpers;

namespace MixERP.Net.Utilities.PgDoc.Generators
{
    internal class AssetWriter
    {
		static string BasePath = string.Format("PgDoc.Configs.Template.{0}.", Program.TemplateSet); // MixERP.Net.Utilities.
		private void Write(string src, string dest) {
			FileHelper.WriteResourceToOutPutDirectory(string.Format("{0}{1}", BasePath, src), dest);
		}

        internal AssetWriter()
        {
            Console.WriteLine("Generating stylesheets and scripts.");
            
            Write("Scripts.jquery.min.js", "Scripts/jquery.min.js");
            Write("Scripts.prism.min.js", "Scripts/prism.min.js");
            Write("Scripts.semantic.min.js", "Scripts/semantic.min.js");
            
			Write("Stylesheets.pgdoc.css", "Stylesheets/pgdoc.css");
            Write("Stylesheets.prism.min.css", "Stylesheets/prism.min.css");
            Write("Stylesheets.semantic.min.css", "Stylesheets/semantic.min.css");

            Write("Stylesheets.themes.default.assets.fonts.icons.eot", "Stylesheets/themes/default/assets/fonts/icons.eot");
            Write("Stylesheets.themes.default.assets.fonts.icons.otf", "Stylesheets/themes/default/assets/fonts/icons.otf");
            Write("Stylesheets.themes.default.assets.fonts.icons.svg", "Stylesheets/themes/default/assets/fonts/icons.svg");
            Write("Stylesheets.themes.default.assets.fonts.icons.ttf", "Stylesheets/themes/default/assets/fonts/icons.ttf");
            Write("Stylesheets.themes.default.assets.fonts.icons.woff", "Stylesheets/themes/default/assets/fonts/icons.woff");
            Write("Stylesheets.themes.default.assets.images.flags.png", "Stylesheets/themes/default/assets/images/flags.png");
        }
    }
}