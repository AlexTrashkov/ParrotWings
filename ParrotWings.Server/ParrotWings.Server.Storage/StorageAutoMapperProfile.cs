using AutoMapper;
using ParrotWings.Server.Domain.Entities;
using ParrotWings.Server.Storage.DTO;

namespace ParrotWings.Server.Storage
{
	public sealed class StorageAutoMapperProfile : Profile
	{
		public StorageAutoMapperProfile()
		{
			CreateMap<Transfer, TransferDTO>();
			CreateMap<TransferDTO, Transfer>();

			CreateMap<User, UserDTO>();
			CreateMap<UserDTO, User>();
		}
	}
}