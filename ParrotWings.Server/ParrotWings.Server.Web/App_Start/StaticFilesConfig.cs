using System;
using System.IO;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;

namespace ParrotWings.Server.Web
{
	public static class StaticFilesConfig
	{
		public static void Register(IAppBuilder app)
		{
			var wwwrootDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot");

			var fileServerOptions = new FileServerOptions()
			{
				EnableDefaultFiles = true,
				EnableDirectoryBrowsing = false,
				RequestPath = new PathString(string.Empty),
				FileSystem = new PhysicalFileSystem(wwwrootDirectory)
			};

			fileServerOptions.StaticFileOptions.ServeUnknownFileTypes = true;
			app.UseFileServer(fileServerOptions);
		}
	}
}