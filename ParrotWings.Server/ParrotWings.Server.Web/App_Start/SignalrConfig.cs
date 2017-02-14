using System;
using System.Reflection;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.AspNet.SignalR.Ninject;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Ninject;
using Owin;

namespace ParrotWings.Server.Web
{
	public static class SignalrConfig
	{
		public static void Register(IAppBuilder app, IKernel kernel)
		{
			app.MapSignalR(new HubConfiguration
			{
				EnableDetailedErrors = true,
				EnableJSONP = true,
				Resolver = new NinjectDependencyResolver(kernel)
			});

			//TODO setup camelCase not working. need fix it
			var settings = new JsonSerializerSettings
			{
				ContractResolver = new SignalRContractResolver()
			};

			var serializer = JsonSerializer.Create(settings);
			kernel.Bind<JsonSerializer>().ToConstant(serializer);
		}

		private sealed class SignalRContractResolver : IContractResolver
		{
			private readonly Assembly _assembly;
			private readonly IContractResolver _camelCaseContractResolver;
			private readonly IContractResolver _defaultContractSerializer;

			public SignalRContractResolver()
			{
				_defaultContractSerializer = new DefaultContractResolver();
				_camelCaseContractResolver = new CamelCasePropertyNamesContractResolver();
				_assembly = typeof(Connection).Assembly;
			}

			public JsonContract ResolveContract(Type type)
			{
				if (type.Assembly.Equals(_assembly))
				{
					return _defaultContractSerializer.ResolveContract(type);
				}

				return _camelCaseContractResolver.ResolveContract(type);
			}
		}
	}
}