using System;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;

namespace ParrotWings.Server.Storage.DTO
{
	[Table("Users")]
	internal sealed class UserDTO
	{
		public Guid Id { get; set; }
		public DateTime CreateDate { get; set; }

		[NotNull]
		public string UserName { get; set; }

		[NotNull]
		public string Email { get; set; }

		[NotNull]
		public string HashedPass { get; set; }

		[NotNull]
		public string Salt { get; set; }

		public decimal CurrentBalance { get; set; }
	}
}