using AutoMapper;
using ParrotWings.Server.Domain.Entities;
using ParrotWings.Server.Web.Models;

namespace ParrotWings.Server.Web
{
	public sealed class WebAutoMapperProfile : Profile
	{
		public WebAutoMapperProfile()
		{
			CreateMap<User, AccountInfoDTO>();
			CreateMap<User, UserInfoDTO>();
		}
	}
}