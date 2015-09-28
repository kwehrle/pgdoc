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
	public sealed partial class PgDatabase 
	{
		public sealed class PgSetting : PgBase
		{
			public string Description { get; set; }
			public string Setting { get; set; }

			public PgSetting() { }

			override public PgBase Convert(System.Data.DataRow row)
			{
				Name = Conversion.TryCastString(row["name"]);
				Setting = Conversion.TryCastString(row["setting"]);
				Description = Conversion.TryCastString(row["description"]);

				return this as PgBase;
			}
			public override string Parse(string template)
			{
				return template
					.Replace("{{property.name}}", Name)
					.Replace("{{property.setting}}", Setting)
					.Replace("{{property.description}}", Description);

			}
		}
	}

 }