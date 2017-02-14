using System;

namespace ParrotWings.Server.Web.Models
{
	public sealed class UserInfoDTO
	{
		public Guid Id { get; set; }
		public string UserName { get; set; }
		public string Email { get; set; }
	}
}