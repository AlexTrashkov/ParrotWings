using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNet.Identity;
using ParrotWings.Server.Domain.Entities;

namespace ParrotWings.Server.Domain.Repositories
{
	public interface IUserRepository : IUserStore<User, Guid>
	{
		[NotNull]
		[ItemNotNull]
		Task<User> GetByAsync(Guid userId);

		[NotNull]
		[ItemCanBeNull]
		Task<User> FindByAsync([NotNull] string email);

		[NotNull]
		[ItemCanBeNull]
		Task<User> FindByAsync([NotNull] string email, [NotNull] string password);

		[NotNull]
		[ItemNotNull]
		Task<IReadOnlyCollection<User>> ListByAsync([NotNull] string searchString, uint offset, uint count);
	}
}