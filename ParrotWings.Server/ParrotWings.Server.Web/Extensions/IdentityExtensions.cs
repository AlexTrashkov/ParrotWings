using System;
using System.Security.Principal;
using Microsoft.AspNet.Identity;

namespace ParrotWings.Server.Web.Extensions
{
	public static class IdentityExtensions
	{
		public static Guid GetUserIdAsGuid(this IIdentity value) => Guid.Parse(value.GetUserId());
	}
}