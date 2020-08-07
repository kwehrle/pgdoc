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
using System.Reflection;

namespace MixERP.Net.Utilities.PgDoc.Helpers
{
    internal static class FileHelper
    {
        internal static void CopyStream(Stream input, Stream output)
        {
            // Insert null checking here for production
            byte[] buffer = new byte[8192];

            int bytesRead;
            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, bytesRead);
            }
        }

        internal static void EmptyOutputDirectory()
        {
            Directory.Delete(Program.OutputDirectory, true);
        }

        internal static string GetApplicationRootPath()
        {
            string applicationPath = Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);

            if (string.IsNullOrEmpty(applicationPath))
            {
                return null;
            }

			var directoryInfo = new DirectoryInfo(applicationPath).Parent;
			return (directoryInfo == null || directoryInfo.Parent == null) ? string.Empty : directoryInfo.Parent.FullName;
		}

        internal static bool IsOutputDirectoryEmpty()
        {
            return (!Directory.Exists(Program.OutputDirectory)) ? true : !Directory.EnumerateFileSystemEntries(Program.OutputDirectory).Any();
        }

        internal static string ReadFile(string relativePath)
        {
            string path = string.Format("{0}{1}", GetApplicationRootPath(), relativePath);
            return File.Exists(path) ? File.ReadAllText(path) : string.Empty;
        }


        internal static string ReadSqlResource(string name)
        {
			return ReadResource(string.Format("PgDoc.Configs.SQL.{0}", name));
        }

        internal static string ReadResource(string resourceName)
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName)) {
                if (stream != null) {
                    using (StreamReader reader = new StreamReader(stream)) {
                        return reader.ReadToEnd();
                    }
                }
            }
            return string.Empty;
        }

        internal static void WriteFile(string content, string relativePath)
        {
            string templatePath = Path.Combine(Program.OutputDirectory, relativePath);

            FileInfo file = new FileInfo(templatePath);

            if (file.Directory != null)
            {
                file.Directory.Create();
            }

            File.WriteAllText(file.FullName, content);
        }

        internal static void WriteResourceToOutPutDirectory(string resourceName, string fileName)
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName)) {
				if (stream != null) {
					string templatePath = Path.Combine(Program.OutputDirectory, fileName);
					FileInfo file = new FileInfo(templatePath);

					if (file.Directory != null) {
						file.Directory.Create();
					}

					using (Stream output = File.Create(file.FullName)) {
						CopyStream(stream, output);
					}
				}
            }
        }
    }
}