using System.Net.Http.Headers;
using System.Web.Http;
using JetBrains.Annotations;
using Newtonsoft.Json.Serialization;
using Ninject.Web.WebApi.OwinHost;
using Owin;

namespace ParrotWings.Server.Web
{
	public static class WebApiConfig
	{
		public static void Register([NotNull] IAppBuilder app)
		{
			var config = new HttpConfiguration();
			config.MapHttpAttributeRoutes();
			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new {id = RouteParameter.Optional}
				);

			config.Formatters
				.JsonFormatter.SupportedMediaTypes
				.Add(new MediaTypeHeaderValue("text/html"));

			config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
			config.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;

			app.UseNinjectWebApi(config);
		}
	}
}