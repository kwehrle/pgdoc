using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MixERP.Net.Utilities.PgDoc.Generators
{
	using MixERP.Net.Utilities.PgDoc.Helpers;
	using MixERP.Net.Utilities.PgDoc.Models;
	using MixERP.Net.Utilities.PgDoc.Parsers;
	using MixERP.Net.Utilities.PgDoc.Processors;

	internal static class StaticWriter
	{
		internal static readonly string TemplatePath = string.Format("PgDoc.Configs.Template.{0}", Program.TemplateSet); // MixERP.Net.Utilities.

		private static string _master = null;
		internal static string Master { get {
			if (_master == null) {
				var db = Program.db;
				const string hide = "hide";
				_master = FileHelper.ReadResource(string.Format("{0}.master.html", TemplatePath));
				// hide unnecessary menu items
				_master = _master.Replace("{{menu.schemas}}", db.Schemas.Count() > 0 ? string.Empty : hide);
				_master = _master.Replace("{{menu.tables}}", db.Tables.Count() > 0 ? string.Empty : hide);
				_master = _master.Replace("{{menu.sequences}}", db.Sequences.Count() > 0 ? string.Empty : hide);
				_master = _master.Replace("{{menu.views}}", db.Views.Count() > 0 ? string.Empty : hide);
				_master = _master.Replace("{{menu.functions}}", db.Functions.Count() > 0 ? string.Empty : hide);
				_master = _master.Replace("{{menu.types}}", db.Types.Count() > 0 ? string.Empty : hide);

				_master = _master.Replace("{{default.tablespace}}", Program.omitTablespace.Length == 0 ? string.Empty : string.Format("{0} Tablespace is '{1}'. ", Program.TablespaceSubstitute, Program.omitTablespace));
				_master = _master.Replace("{{default.owner}}", Program.omitOwner.Length == 0 ? string.Empty : string.Format("{0} Owner is '{1}'. ", Program.OwnerSubstitute, Program.omitOwner));

					_master = _master.Replace("{{generated}}", string.Format("{0:yyyy-MM-dd HH:mm} UTC", DateTime.UtcNow));
			}
			return _master;
		}
			set {}
		}
		public static string FillMaster(string title, string content, int level = 0)
		{
			var replacement = string.Empty;
			while (level-- > 0) {
				replacement += "../";
			}
			return Master.Replace("{{dir_level}}", replacement).Replace("{{doc.title}}", title).Replace("{{content}}", content).Replace("{{db.name}}", Program.db.Name);
		}

	}

	internal class BaseWriter<T> where T: PgBase
	{
		private static string typeName()
		{
			// get rid of the 'pg' prefix and convert to lower case
			return typeof(T).Name.Substring(2).ToLower();
		}

		protected readonly string IndexFileTemplate = string.Format("{0}.{1}s.html", StaticWriter.TemplatePath, typeName());
		protected readonly string FileTemplatePath = string.Format("{0}.{1}.html", StaticWriter.TemplatePath, typeName());
		protected readonly string IndexFileName = string.Format("{0}s.html", typeName());
		protected readonly string DirectoryOutputPath = string.Format("{0}s", typeName());


		virtual protected string BuildDocumentation(string content, IEnumerable<T> list, IEnumerable<string> matches) {
			return StaticWriter.FillMaster(string.Format("{0}s", typeName()), Parser<T>.Parse(content, matches, list));
		}
		virtual protected string BuildDocumentation(string content, T o, IEnumerable<string> matches) {
			return StaticWriter.FillMaster(string.Format("{0} {1}", typeName(), o.Name), o.Parse(content), 2);
		}

		/// <summary>
		/// Writes an Index-File in the root directory for all different kind of objects (tables, views, funtions, ...) without regarding the schema
		/// </summary>
		/// <param name="list"></param>
		public void WriteFile(IEnumerable<T> list)
		{
			var content = FileHelper.ReadResource(IndexFileTemplate);
			var matches = HtmlHelper.GetMatch(content);

			 if (matches != null) {
				 foreach (T o in list)
					 WriteFile(o);

				content = BuildDocumentation(content, list,matches);
				FileHelper.WriteFile(content, IndexFileName);
			}
		}

		/// <summary>
		/// Writes a single definition
		/// </summary>
		/// <param name="model"></param>
		internal void WriteFile(T model)
		{
			string content = FileHelper.ReadResource(FileTemplatePath);
			// for some pg-types I don't have a single objection definition html file. It's not necessary!
			if (content.Length > 0) {
				var matches = HtmlHelper.GetMatch(content);

				string targetPath = Path.Combine(DirectoryOutputPath, model.SchemaName, (typeName() == "function" ? model.Name + '.' + (model as PgFunction).oid : model.Name)+ ".html");
				content = BuildDocumentation(content, model, matches);
				content = content.Replace("{{db.name}}", Program.Database.ToUpperInvariant());
				FileHelper.WriteFile(content, targetPath);
			}
		}
	}
}
