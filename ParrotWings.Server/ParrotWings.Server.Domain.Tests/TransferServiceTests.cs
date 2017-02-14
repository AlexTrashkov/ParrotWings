using System;
using System.Threading.Tasks;
using Moq;
using ParrotWings.Server.Domain.Entities;
using ParrotWings.Server.Domain.Exceptions;
using ParrotWings.Server.Domain.Repositories;
using ParrotWings.Server.Domain.Services;
using ParrotWings.Server.Domain.Services.Implementations;
using Xunit;

namespace ParrotWings.Server.Domain.Tests
{
	public sealed class TransferServiceTests
	{
		[Fact]
		public async Task CreateTransfer_UserFromHasNotSufficientFunds_ThrowNotSufficientFundsException()
		{
			//Arrange
			var userFrom = new User(Guid.NewGuid(), DateTime.UtcNow, "John Smith", "jonh@smith.me", "pass1", "salt1", 3);
			var userTo = new User(Guid.NewGuid(), DateTime.UtcNow, "Mark Walker", "mark@walker.me", "pass2", "salt2", 3);

			var userRepositoryMock = new Mock<IUserRepository>();
			userRepositoryMock
				.Setup(x => x.GetByAsync(userFrom.Id))
				.Returns(Task.Run(() => userFrom));

			userRepositoryMock
				.Setup(x => x.GetByAsync(userTo.Id))
				.Returns(Task.Run(() => userTo));

			var transferRepositoryMock = new Mock<ITransferRepository>();

			ITransferService transferService = new TransferService(
				transferRepositoryMock.Object,
				userRepositoryMock.Object);

			//Act
			var task = transferService.CreateTransferAsync(userFrom.Id, userTo.Id, 5);

			//Assert
			await Assert.ThrowsAsync<NotSufficientFundsException>(() => task);
			Assert.Equal(3, userFrom.CurrentBalance);
			Assert.Equal(3, userTo.CurrentBalance);
		}

		[Fact]
		public async Task CreateTransfer_TransferAmountIsZero_ThrowIncorrectTransferAmountException()
		{
			//Arrange
			var userFrom = new User(Guid.NewGuid(), DateTime.UtcNow, "John Smith", "jonh@smith.me", "pass1", "salt1", 3);
			var userTo = new User(Guid.NewGuid(), DateTime.UtcNow, "Mark Walker", "mark@walker.me", "pass2", "salt2", 3);

			var userRepositoryMock = new Mock<IUserRepository>();
			userRepositoryMock
				.Setup(x => x.GetByAsync(userFrom.Id))
				.Returns(Task.Run(() => userFrom));

			userRepositoryMock
				.Setup(x => x.GetByAsync(userTo.Id))
				.Returns(Task.Run(() => userTo));

			var transferRepositoryMock = new Mock<ITransferRepository>();

			ITransferService transferService = new TransferService(
				transferRepositoryMock.Object,
				userRepositoryMock.Object);

			//Act
			var task = transferService.CreateTransferAsync(userFrom.Id, userTo.Id, 0);

			//Assert
			await Assert.ThrowsAsync<IncorrectTransferAmountException>(() => task);
			Assert.Equal(3, userFrom.CurrentBalance);
			Assert.Equal(3, userTo.CurrentBalance);
		}

		[Fact]
		public async Task CreateTransfer_UserFromHasSufficientFundsAndTransferAmountIsNotZero_CreateTransferSuccessful()
		{
			//Arrange
			var userFrom = new User(Guid.NewGuid(), DateTime.UtcNow, "John Smith", "jonh@smith.me", "pass1", "salt1", 3);
			var userTo = new User(Guid.NewGuid(), DateTime.UtcNow, "Mark Walker", "mark@walker.me", "pass2", "salt2", 3);

			var userRepositoryMock = new Mock<IUserRepository>();
			userRepositoryMock
				.Setup(x => x.GetByAsync(userFrom.Id))
				.Returns(Task.Run(() => userFrom));

			userRepositoryMock.Setup(x => x.GetByAsync(userTo.Id))
				.Returns(Task.Run(() => userTo));

			var transferRepositoryMock = new Mock<ITransferRepository>();

			Transfer savedTransfer = null;
			Func<Transfer, Task> saveTransferFunction = x => Task.Run(() => savedTransfer = x);

			transferRepositoryMock.Setup(x => x.SaveTransferAndUpdateUsersBalancesAsync(It.IsAny<Transfer>()))
				.Returns(saveTransferFunction);

			ITransferService transferService = new TransferService(
				transferRepositoryMock.Object,
				userRepositoryMock.Object);

			//Act
			await transferService.CreateTransferAsync(userFrom.Id, userTo.Id, 3);

			//Assert
			Assert.Equal(0, userFrom.CurrentBalance);
			Assert.Equal(6, userTo.CurrentBalance);

			Assert.NotNull(savedTransfer);

			Assert.Equal(3, savedTransfer.UserFromBalanceBeforeTransfer);
			Assert.Equal(3, savedTransfer.UserToBalanceBeforeTransfer);

			Assert.Equal(0, savedTransfer.UserFromBalanceAfterTransfer);
			Assert.Equal(6, savedTransfer.UserToBalanceAfterTransfer);
		}
	}
}