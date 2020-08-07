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
		public sealed class PgTrigger : PgBase
		{
			public string Condition { get; set; }
			public string Description { get; set; }
			public string EventName { get; set; }
			public int Order { get; set; }
			public string Orientation { get; set; }
			public string TargetFunctionName { get; set; }
			public string TargetFunctionOid { get; set; }
			public string TargetFunctionSchema { get; set; }
			public string TargetTableName { get; set; }
			public string TargetTableSchema { get; set; }
			public string Timing { get; set; }

			public PgTrigger() { }

			override public PgBase Convert(System.Data.DataRow row)
			{
				SchemaName = Conversion.TryCastString(row["trigger_schema"]);
				Name = Conversion.TryCastString(row["trigger_name"]);
				TargetTableSchema = Conversion.TryCastString(row["event_object_schema"]);
				TargetTableName = Conversion.TryCastString(row["event_object_table"]);
				EventName = Conversion.TryCastString(row["event_manipulation"]);
				Timing = Conversion.TryCastString(row["action_timing"]);
				Condition = Conversion.TryCastString(row["action_condition"]);
				Order = Conversion.TryCastInteger(row["action_order"]);
				Orientation = Conversion.TryCastString(row["action_orientation"]);
				TargetFunctionSchema = Conversion.TryCastString(row["target_function_schema"]);
				TargetFunctionName = Conversion.TryCastString(row["target_function_name"]);
				TargetFunctionOid = Conversion.TryCastString(row["target_function_oid"]);
				Description = Conversion.TryCastString(row["description"]);

				return this as PgBase;
			}

			override public string Parse(string template)
			{
				string eventIcon = string.Empty, timeIcon = string.Empty, orientationIcon = (Orientation == "ROW" ? "reply all" : "reply");

				switch (EventName) {
					case "INSERT":
						eventIcon = "plus";
						break;
					case "UPDATE":
						eventIcon = "write";
						break;
					case "TRUNCATE":
						eventIcon = "remove";
						break;
					case "DELETE":
						eventIcon = "minus";
						break;
				};

				switch (Timing) {
					case "AFTER":
						timeIcon = "right";
						break;
					case "BEFORE":
						timeIcon = "left";
						break;
					case "INSTEAD OF":
						timeIcon = "up";
						break;
				};
				return template
					.Replace("{{trigger.name}}", Name)
					.Replace("{{trigger.schema}}", SchemaName)
					.Replace("{{trigger.tableSchema}}", TargetTableSchema)
					.Replace("{{trigger.tableName}}", TargetTableName)
					.Replace("{{trigger.eventName}}", string.Format("<i class='icon {0}' title='{1}'></i>", eventIcon, EventName))
					.Replace("{{trigger.timing}}", string.Format("<i class='icon chevron circle {0}' title='{1}'></i>", timeIcon, Timing))
					.Replace("{{trigger.condition}}", Condition)
					.Replace("{{trigger.order}}", Order.ToString())
					.Replace("{{trigger.orientation}}", string.Format("<i class='icon {0}' title='{1}'></i>", orientationIcon, Orientation))
					.Replace("{{trigger.functionSchema}}", TargetFunctionSchema)
					.Replace("{{trigger.functionName}}", TargetFunctionName)
					.Replace("{{trigger.functionOid}}", TargetFunctionOid)
					.Replace("{{trigger.description}}", PgBase.TransformMarkDown(Description));
			}
		}
	}
}