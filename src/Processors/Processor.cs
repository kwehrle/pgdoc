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
using System.Data;
using Npgsql;

namespace MixERP.Net.Utilities.PgDoc.Processors
{
	using System.Collections.Generic;
	using MixERP.Net.Utilities.PgDoc.DBFactory;
	using MixERP.Net.Utilities.PgDoc.Helpers;
	using MixERP.Net.Utilities.PgDoc.Models;

	internal class Processor<T> where T : PgBase, new()
	{
		public static void AddPgTypeDefinitionFunction()
        {
            string sql = FileHelper.ReadSqlResource("pg_catalog.pg_get_typedef.sql");
            DbOperation.ExecuteNonQuery(new NpgsqlCommand(sql));
        }

		public static string GetDatabaseComment()
		{
			const string sql = "SELECT description FROM pg_shdescription INNER JOIN pg_database ON objoid = pg_database.oid WHERE datname = current_database();";
			using (NpgsqlCommand command = new NpgsqlCommand(sql)) {
				return Conversion.TryCastString(DbOperation.GetScalarValue(command));
			}
		}

		public IEnumerable<T> Collect(NpgsqlCommand command)
		{
			var collection = new Collection<T>();
			var table = DbOperation.GetDataTable(command);
			if (table != null && table.Rows != null) {
				foreach (DataRow row in table.Rows) {
					T obj = new T();
					collection.Add((T) obj.Convert(row));
				}
			}
			return collection;
		}

		internal IEnumerable<T> GetBySchema(string schemaPattern = ".*", string xSchemaPattern = "")
		{
			string sql = FileHelper.ReadSqlResource(string.Format("bySchema.{0}s.sql", typeof(T).Name.Substring(2).ToLower()));

			using (NpgsqlCommand command = new NpgsqlCommand(sql)) {
				command.Parameters.AddWithValue("@SchemaPattern", schemaPattern);
				command.Parameters.AddWithValue("@xSchemaPattern", xSchemaPattern);
				return Collect(command);
			}
		}
		internal IEnumerable<T> GetByTable(PgTable pgTable)
		{
			string sql = FileHelper.ReadSqlResource(string.Format("byTable.{0}.sql", typeof(T).Name.Substring(2).ToLower()));

			using (NpgsqlCommand command = new NpgsqlCommand(sql)) {
				command.Parameters.AddWithValue("@SchemaName", pgTable.SchemaName);
				command.Parameters.AddWithValue("@TableName", pgTable.Name);
				return Collect(command);
			}
		}
	}
}


