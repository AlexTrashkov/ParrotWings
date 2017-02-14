using Ninject.Modules;
using Ninject.Web.Common;
using ParrotWings.Server.Domain.Services;
using ParrotWings.Server.Domain.Services.Implementations;

namespace ParrotWings.Server.Domain
{
	public sealed class DomainModule : NinjectModule
	{
		public override void Load()
		{
			Bind<ITransferService>()
				.To<TransferService>()
				.InRequestScope();

			Bind<UserService>()
				.To<UserService>()
				.InRequestScope();

			Bind<ISecurityService>()
				.To<SecurityService>()
				.InRequestScope();
		}
	}
}