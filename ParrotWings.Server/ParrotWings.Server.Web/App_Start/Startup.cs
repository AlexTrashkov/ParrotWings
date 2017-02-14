using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Ninject;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Ninject;
using Owin;
using ParrotWings.Server.Domain;
using ParrotWings.Server.Storage;
using Ninject.Web.Common.OwinHost;

[assembly: OwinStartup(typeof(ParrotWings.Server.Web.OwinStartup))]

namespace ParrotWings.Server.Web
{
	public sealed class OwinStartup
	{
		public void Configuration(IAppBuilder app)
		{
			app.UseCookieAuthentication(new CookieAuthenticationOptions
			{
				AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
				LoginPath = new PathString("/Account/Login"),
			});

			var kernel = new StandardKernel(new DomainModule(), new StorageModule());
			app.UseNinjectMiddleware(() => kernel);

			SignalrConfig.Register(app, kernel);
			AutoMapperConfig.Register(kernel);
			WebApiConfig.Register(app);
			StaticFilesConfig.Register(app);
		}
	}
}