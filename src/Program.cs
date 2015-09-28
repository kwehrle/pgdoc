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
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MixERP.Net.Utilities.PgDoc.Generators;
using MixERP.Net.Utilities.PgDoc.Helpers;
using MixERP.Net.Utilities.PgDoc.Models;
using MixERP.Net.Utilities.PgDoc.Parsers;
using MixERP.Net.Utilities.PgDoc.Processors;

namespace MixERP.Net.Utilities.PgDoc
{
    internal class Program
    {
        internal static readonly string AppName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
		internal static PgDatabase db; // a global Reference to the database object, important for PgSchema to filter its contents
        internal static string Database;
		internal static int Port = 5432;
		internal static string UserId = "postgres";
		internal static string Password;
		internal static string Server = "localhost";
		internal static string TemplateSet = "kwrl";

        //internal static string DisqusName;
		internal static bool RemoveEmpty = false;
        internal static bool Overwrite = false;
        internal static string OutputDirectory;
		internal static string SchemaPattern = "%"; // Regex-Pattern for schemas to include
		internal static string xSchemaPattern = string.Empty; // Regex-Pattern for schemas to exclude
		
        internal static bool Build(string[] args)
        {
            foreach (string argument in args)
            {
                if (argument.StartsWith("-s"))
                {
                    Server = ArgumentParser.Parse(argument, "-s");
					ExtractPort();
					continue;
                }

                if (argument.StartsWith("-d"))
                {
                    Database = ArgumentParser.Parse(argument, "-d");
					continue;
				}
				
				if (argument.StartsWith("-ow")) {
					Overwrite = true;
					continue;
				}

                if (argument.StartsWith("-u"))
                {
                    UserId = ArgumentParser.Parse(argument, "-u");
					continue;
				}

                if (argument.StartsWith("-p"))
                {
                    Password = ArgumentParser.Parse(argument, "-p");
					continue;
				}

                if (argument.StartsWith("-o"))
                {
                    OutputDirectory = ArgumentParser.Parse(argument, "-o");
					continue;
				}

				//if (argument.StartsWith("-q"))
				//{
				//	DisqusName = ArgumentParser.Parse(argument, "-q");
				//}

				if (argument.StartsWith("-re")) {
					RemoveEmpty = true;
					continue;
				}

				// kwrl: -t == template set to use for generating html (standard = mixerp)
				if (argument.StartsWith("-t")){
					TemplateSet = ArgumentParser.Parse(argument, "-t");
					continue;
				}

				// kwrl: is == include schemata; format: "-is=RegExpPattern" sample: "-is=(public|def.*)
				if (argument.StartsWith("-is")) {
					SchemaPattern = ArgumentParser.Parse(argument, "-is");
					continue;
				}
				if (argument.StartsWith("-xs")) {
					xSchemaPattern = ArgumentParser.Parse(argument, "-xs");
					continue;
				}
            }


            if (string.IsNullOrWhiteSpace(Server))
            {
                Console.WriteLine("Invalid or missing parameter \"-s\" for PostgreSQL Server.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(Database))
            {
                Console.WriteLine("Invalid or missing parameter \"-d\" for PostgreSQL Database.");
				return false;
			}

            if (string.IsNullOrWhiteSpace(UserId))
            {
                Console.WriteLine("Invalid or missing parameter \"-u\" for PostgreSQL User.");
				return false;
			}

            if (string.IsNullOrWhiteSpace(Password))
            {
                Console.WriteLine("Invalid or missing parameter \"-p\" for PostgreSQL User Password.");
				return false;
			}

            if (string.IsNullOrWhiteSpace(OutputDirectory))
            {
                Console.WriteLine("Invalid or missing parameter \"-o\" for documentation output directory.");
				return false;
			}
			return true;
        }

		//internal static string ReadPassword()
		//{
		//	string password = string.Empty;

		//	ConsoleKeyInfo info = Console.ReadKey(true);

		//	while (info.Key != ConsoleKey.Enter)
		//	{
		//		if (info.Key != ConsoleKey.Backspace)
		//		{
		//			Console.Write("*");

		//			password += info.KeyChar;
		//		}

		//		else if (info.Key == ConsoleKey.Backspace)
		//		{
		//			if (!string.IsNullOrEmpty(password))
		//			{
		//				// remove one character from the list of password characters
		//				password = password.Substring(0, password.Length - 1);

		//				// get the location of the cursor
		//				int pos = Console.CursorLeft;

		//				// move the cursor to the left by one character
		//				Console.SetCursorPosition(pos - 1, Console.CursorTop);

		//				// replace it with space
		//				Console.Write(" ");

		//				// move the cursor to the left by one character again
		//				Console.SetCursorPosition(pos - 1, Console.CursorTop);
		//			}
		//		}

		//		info = Console.ReadKey(true);
		//	}


		//	// add a new line because user pressed enter at the end of their password
		//	Console.WriteLine();

		//	return password;
		//}

        private static bool WriteQuestion(string question)
        {
            Console.Write(question);

            string result = Console.ReadLine();

            return result == null || new string[]{"Y", "YES", "OK", "OKAY"}.Contains(result.ToUpperInvariant());
        }

		private static void ExtractPort() {
			// if there's a colon, I will extract the port
			var ServerParts = Server.Split(':');
			Server = ServerParts[0];
			if (ServerParts.Length == 1 || !int.TryParse(ServerParts[1], out Port)) {
				Port = 5432;
			}
		}

        private static void DisplayHelpInfo()
        {
            Console.WriteLine("Generates HTML documentation from PostgreSQL database.\n");
            Console.WriteLine("Usage: {0} -s=[server[:port]] -d=[database] -u=[pg_user] -p=[pwd] -o=[output_dir] -is=[include_schemas] -xs=[exclude_schemas] -re\n", AppName);
			//Console.WriteLine("WARNING: No parameter supplied.");
			//Console.WriteLine();

			//if (!WriteQuestion("Would you like to provide parameters now? [Yes/No*]"))
			//{
			//	Console.WriteLine();
			//	Console.WriteLine("No");
			//	Console.WriteLine();
			//	return;
			//}

			//Console.WriteLine();
			//Console.WriteLine("Yes");
			//Server = GetParameter("PostgreSQL Server host name or IP address:", "localhost");
			//ExtractPort();

			//Database = GetParameter("Enter the name of your PostgreSQL Database:", "mixerp");
			//UserId = GetParameter("Enter PostgreSQL Database UserId:", "postgres");
			//Password = GetPassword();
			//string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\" +
			//			  Database + "-docs";
			//OutputDirectory = GetParameter("Output directory to generate documentation to:", path);

			//Run();
        }

		//private static string GetParameter(string message, string defaultValue)
		//{
		//	Console.WriteLine();
		//	Console.Write(message);

		//	if (!string.IsNullOrWhiteSpace(defaultValue))
		//	{
		//		SendKeys.SendWait(defaultValue);
		//	}

		//	string parameter = Console.ReadLine();

		//	if (string.IsNullOrWhiteSpace(parameter))
		//	{
		//		GetParameter(message, defaultValue);
		//	}

		//	return parameter;
		//}

		//private static string GetPassword()
		//{
		//	Console.WriteLine();
		//	Console.Write("Enter password for user \"{0}\":", UserId);
		//	string password = ReadPassword();

		//	if (string.IsNullOrWhiteSpace(password))
		//	{
		//		GetPassword();
		//	}

		//	return password;
		//}

        [STAThread]
        private static void Main(string[] args)
        {
            AppDomain.CurrentDomain.AssemblyResolve += DependencyHandler.ResolveEventHandler;
			//AppName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

			if ((args.Length.Equals(0)) ||
				args.Length.Equals(1) && (args[0].Contains("/?") || args[0].Contains("--help"))) {
				DisplayHelpInfo();
			}
			else {
				if (Build(args)) {
					Run();
				}
			}

#if DEBUG
            Console.WriteLine("\nPress any key to continue ...");
            Console.ReadKey();
#endif
        }

		private static void CheckDirectory() {
			if (!FileHelper.IsOutputDirectoryEmpty()) {
				if (!Program.Overwrite) {
					Console.WriteLine("WARNING: The output directory is not empty.");
					bool result = WriteQuestion("Do you want to empty this directory?[Yes/No*]");

					if (!result) {
						throw new Exception("");
					}
				}
				FileHelper.EmptyOutputDirectory();
			}
			Console.WriteLine("{0} initialized.", AppName);
		}

        private static void Run()
        {
            try
            {
				// 1. Step: Read all Information of the selected Database tailored to the desired schemas
				Processor<PgType>.AddPgTypeDefinitionFunction();
				Program.db = new PgDatabase(Program.SchemaPattern, Program.xSchemaPattern) {
					Name = Program.Database
				};

				// 2. Step: now I could probably write and will check if directory is empty or not
				CheckDirectory();

				// 3. Step: Generate HTML-Documentation with Templates
				Console.WriteLine("MixERP Documentation Generator.");
				//StaticWriter.InitMaster(Program.db);

				new DatabaseWriter().Run(Program.db);
				new AssetWriter();

                Console.WriteLine("\n{0} completed.", AppName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nERROR: Cannot create documentation:\n{0}", ex.Message);
            }
        }
    }
}