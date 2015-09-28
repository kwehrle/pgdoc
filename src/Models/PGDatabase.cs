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
using MixERP.Net.Utilities.PgDoc.Processors;
using Npgsql;

namespace MixERP.Net.Utilities.PgDoc.Models
{
    public sealed partial class PgDatabase : PgBase
    {
        public string Comment { get; set; }
        public IEnumerable<PgDatabase.PgSetting> Settings { get; set; }
		public IEnumerable<PgSchema> Schemas { get; set; }
		public IEnumerable<PgFunction> Functions { get; set; }
		//public IEnumerable<PgMaterializedView> MaterializedViews { get; set; }
		public IEnumerable<PgTable> Tables { get; set; }
		public IEnumerable<PgTriggerFunction> TriggerFunctions { get; set; }
		public IEnumerable<PgView> Views { get; set; }
		public IEnumerable<PgSequence> Sequences { get; set; }
		public IEnumerable<PgType> Types { get; set; }


		public PgDatabase(string schemaPattern = ".*", string xSchemaPattern = "")
		{
			SchemaName = "..";
			Comment = Processor<PgTable>.GetDatabaseComment(); // PgDatabase is not derived from PgModel, so I've to use another class for calling static method
			Settings = new Processor<PgSetting>().Collect(new NpgsqlCommand("SHOW all;"));
			Schemas = new Processor<PgSchema>().GetBySchema(schemaPattern, xSchemaPattern);
			//MaterializedViews = new Processor<PgMaterializedView>().GetBySchema(schemaPattern, xSchemaPattern);
			Functions = new Processor<PgFunction>().GetBySchema(schemaPattern, xSchemaPattern);
			TriggerFunctions = new Processor<PgTriggerFunction>().GetBySchema(schemaPattern, xSchemaPattern);
			Sequences = new Processor<PgSequence>().GetBySchema(schemaPattern, xSchemaPattern);
			Types = new Processor<PgType>().GetBySchema(schemaPattern, xSchemaPattern);
			Views = new Processor<PgView>().GetBySchema(schemaPattern, xSchemaPattern);
			Tables = new Processor<PgTable>().GetBySchema(schemaPattern, xSchemaPattern);
			foreach (var table in Tables) {
				table.Indices = new Processor<PgTable.PgIndex>().GetByTable(table);
				table.Triggers = new Processor<PgTable.PgTrigger>().GetByTable(table);
				table.CheckConstraints = new Processor<PgTable.PgCheckConstraint>().GetByTable(table);
				table.Columns = new Processor<PgTable.PgColumn>().GetByTable(table);

				// search for all foreignKey and collect them into table.foreignKeys
				var foreignKeys = new List<PgTable.PgForeignKey>();
				foreach (var c in table.Columns) {
					if (c.foreignKey != null)
						foreignKeys.Add(c.foreignKey);
				}
				table.ForeignKeys = foreignKeys;
			}
		}
		public override PgBase Convert(System.Data.DataRow row)
		{
			throw new System.NotImplementedException();
		}
		public override string Parse(string template)
		{
			throw new System.NotImplementedException();
		}
    }
}
