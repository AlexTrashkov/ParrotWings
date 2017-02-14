using System;
using ParrotWings.Server.Domain.Entities;
using ParrotWings.Server.Domain.Exceptions;
using Xunit;

namespace ParrotWings.Server.Domain.Tests
{
	public sealed class TransferTests
	{
		[Fact]
		public void Constructor_UserToHaveMaxBalance_ThrowOverflowException()
		{
			//Arrange
			var userFrom = new User(Guid.NewGuid(), DateTime.UtcNow, "John Smith", "jonh@smith.me", "pass1", "salt1", 3);
			var userTo = new User(Guid.NewGuid(), DateTime.UtcNow, "Mark Walker", "mark@walker.me", "pass2", "salt2",
				decimal.MaxValue);

			//Act/Assert
			Assert.Throws<OverflowException>(
				() => new Transfer(userFrom, userTo, 3, 3, decimal.MaxValue));
		}

		[Fact]
		public void Constructor_UserFromBalanceBeforeTransferLessThanZero_ThrowUserBalanceLessThanZeroException()
		{
			//Arrange
			var userFrom = new User("John Smith", "jonh@smith.me", "pass1", "salt1");
			var userTo = new User("Mark Walker", "mark@walker.me", "pass2", "salt2");

			//Act/Assert
			Assert.Throws<UserBalanceLessThanZeroException>(
				() => new Transfer(userFrom, userTo, 3, -100, 3));
		}

		[Fact]
		public void Constructor_UserToBalanceBeforeTransferLessThanZero_ThrowUserBalanceLessThanZeroException()
		{
			//Arrange
			var userFrom = new User("John Smith", "jonh@smith.me", "pass1", "salt1");
			var userTo = new User("Mark Walker", "mark@walker.me", "pass2", "salt2");

			//Act/Assert
			Assert.Throws<UserBalanceLessThanZeroException>(
				() => new Transfer(userFrom, userTo, 3, 3, -100));
		}

		[Fact]
		public void Constructor_TransferAmountIsZero_ThrowIncorrectTransferAmountException()
		{
			//Arrange
			var userFrom = new User(Guid.NewGuid(), DateTime.UtcNow, "John Smith", "jonh@smith.me", "pass1", "salt1", 3);
			var userTo = new User(Guid.NewGuid(), DateTime.UtcNow, "Mark Walker", "mark@walker.me", "pass2", "salt2", 3);

			//Act/Assert
			Assert.Throws<IncorrectTransferAmountException>(
				() => new Transfer(userFrom, userTo, 0, 3, 3));
		}

		[Fact]
		public void Constructor_CorrectDataForTransfer_CreateTransferSuccessful()
		{
			//Arrange
			var userFrom = new User(Guid.NewGuid(), DateTime.UtcNow, "John Smith", "jonh@smith.me", "pass1", "salt1", 3);
			var userTo = new User(Guid.NewGuid(), DateTime.UtcNow, "Mark Walker", "mark@walker.me", "pass2", "salt2", 3);

			//Act
			var transfer = new Transfer(userFrom, userTo, 3, 3, 3);

			//Assert
			Assert.Equal(3, transfer.UserFromBalanceBeforeTransfer);
			Assert.Equal(3, transfer.UserToBalanceBeforeTransfer);

			Assert.Equal(0, transfer.UserFromBalanceAfterTransfer);
			Assert.Equal(6, transfer.UserToBalanceAfterTransfer);
		}
	}
}