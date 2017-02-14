using System;
using JetBrains.Annotations;
using Microsoft.AspNet.Identity;
using ParrotWings.Server.Domain.Entities;
using ParrotWings.Server.Domain.Repositories;

namespace ParrotWings.Server.Domain.Services.Implementations
{
	public sealed class UserService : UserManager<User, Guid>
	{
		public UserService([NotNull] IUserRepository store) : base(store)
		{

		}
	}
}