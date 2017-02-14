using Microsoft.AspNet.Identity;
using Ninject.Modules;
using Ninject.Web.Common;
using ParrotWings.Server.Domain.Repositories;
using ParrotWings.Server.Storage.Repositories;

namespace ParrotWings.Server.Storage
{
	public sealed class StorageModule : NinjectModule
	{
		public override void Load()
		{
			Bind<ParrotWingsDataContext>()
				.To<ParrotWingsDataContext>()
				.InRequestScope();

			Bind<IUserRepository>()
				.To<UserRepository>()
				.InRequestScope();

			Bind<ITransferRepository>()
				.To<TransferRepository>()
				.InRequestScope();

			Bind<IPasswordHasher>()
				.To<PasswordHasher>()
				.InSingletonScope();
		}
	}
}