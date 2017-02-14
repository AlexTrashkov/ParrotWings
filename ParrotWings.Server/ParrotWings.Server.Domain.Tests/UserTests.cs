using System;
using ParrotWings.Server.Domain.Entities;
using ParrotWings.Server.Domain.Exceptions;
using Xunit;

namespace ParrotWings.Server.Domain.Tests
{
	public sealed class UserTests
	{
		[Fact]
		public void Constructor_IncorrectEmail_ThrowInvalidEmailException()
		{
			Assert.Throws<InvalidEmailException>(
				() => new User("John Smith", "jonh!!!smith.me", "pass1", "salt1"));
		}

		[Fact]
		public void Constructor_BalanceLessThanZero_UserBalanceLessThanZeroException()
		{
			Assert.Throws<UserBalanceLessThanZeroException>(
				() => new User(Guid.NewGuid(), DateTime.UtcNow, "John Smith", "jonh@smith.me", "pass1", "salt1", -10));
		}

		[Fact]
		public void Constructor_CorrectEmailAndBalance_CreateUserSuccessful()
		{
			// ReSharper disable once ObjectCreationAsStatement
			new User(Guid.NewGuid(), DateTime.UtcNow, "John Smith", "jonh@smith.me", "pass1", "salt1", 10);
		}
	}
}