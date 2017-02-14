using ParrotWings.Server.Domain.Extensions;
using Xunit;

namespace ParrotWings.Server.Domain.Tests
{
	public sealed class StringExtensionsTests
	{
		[Fact]
		public void IsValidEmail_MailIsValid_ReturnTrue()
		{
			//Arrange
			var mail = "foo@bar.baz";

			//Act
			var mailIsValid = mail.IsValidEmail();

			//Assert
			Assert.True(mailIsValid);
		}

		[Fact]
		public void IsValidEmail_MailIsInvalid_ReturnFalse()
		{
			//Arrange
			var mail = "foo.bar";

			//Act
			var mailIsValid = mail.IsValidEmail();

			//Assert
			Assert.False(mailIsValid);
		}
	}
}