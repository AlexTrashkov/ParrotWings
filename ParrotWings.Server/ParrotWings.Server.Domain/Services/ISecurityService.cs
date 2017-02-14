using JetBrains.Annotations;

namespace ParrotWings.Server.Domain.Services
{
	public interface ISecurityService
	{
		[NotNull]
		string CreateSalt();

		[NotNull]
		string CalculateHash([NotNull] string password, [NotNull] string salt);
	}
}