using System;
using JetBrains.Annotations;

namespace ParrotWings.Server.Web.Models
{
	public sealed class AccountInfoDTO
	{
		public Guid Id { get; set; }
		public DateTime CreateDate { get; set; }

		[NotNull]
		public string UserName { get; set; }

		[NotNull]
		public string Email { get; set; }

		public decimal CurrentBalance { get; set; }
	}
}