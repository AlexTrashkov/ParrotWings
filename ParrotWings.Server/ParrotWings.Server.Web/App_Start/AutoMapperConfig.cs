using AutoMapper;
using JetBrains.Annotations;
using Ninject;
using ParrotWings.Server.Storage;

namespace ParrotWings.Server.Web
{
	public static class AutoMapperConfig
	{
		public static void Register([NotNull] IKernel kernel)
		{
			Mapper.Initialize(x =>
			{
				x.AddProfile<StorageAutoMapperProfile>();
				x.AddProfile<WebAutoMapperProfile>();
			});

			kernel.Bind<IMapper>().ToConstant(Mapper.Instance);
		}
	}
}