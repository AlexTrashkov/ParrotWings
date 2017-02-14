using System;
using AutoMapper;
using ParrotWings.Server.Domain.Entities;
using ParrotWings.Server.Storage.DTO;
using Xunit;

namespace ParrotWings.Server.Storage.Tests
{
	public sealed class StorageAutoMapperProfileTests
	{
		private readonly IMapper _mapper;

		public StorageAutoMapperProfileTests()
		{
			Mapper.Initialize(x => x.AddProfile<StorageAutoMapperProfile>());
			_mapper = Mapper.Instance;
		}

		[Fact]
		public void MapUserToUserDto_DtoHasNoChanges_UsersAreEqual()
		{
			//Arrange
			var userOrig = new User(Guid.NewGuid(), DateTime.UtcNow, "John Smith", "jonh@smith.me", "pass1", "salt1", 10);

			//Act
			var userDto = _mapper.Map<User, UserDTO>(userOrig);
			var userCopy = _mapper.Map<UserDTO, User>(userDto);

			//Assert
			Assert.Equal(userOrig, userCopy);
		}

		[Fact]
		public void MapTransferToTransferDto_DtoHasNoChanges_TransfersAreEqual()
		{
			//Arrange
			var userFrom = new User(Guid.NewGuid(), DateTime.UtcNow, "John Smith", "jonh@smith.me", "pass1", "salt1", 3);
			var userTo = new User(Guid.NewGuid(), DateTime.UtcNow, "Mark Walker", "mark@walker.me", "pass2", "salt2", 3);
			var transferOrig = new Transfer(Guid.NewGuid(), DateTime.UtcNow, userFrom, userTo, 3, 3, 3);

			//Act
			var transferDto = _mapper.Map<Transfer, TransferDTO>(transferOrig);
			var transferCopy = _mapper.Map<TransferDTO, Transfer>(transferDto);

			//Assert
			Assert.Equal(transferOrig, transferCopy);
		}
	}
}