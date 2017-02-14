using System;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;

namespace ParrotWings.Server.Storage.DTO
{
	[Table("Transfers")]
	internal sealed class TransferDTO
	{
		public Guid Id { get; set; }
		public DateTime CreateDate { get; set; }

		[NotNull]
		public UserDTO UserFrom { get; set; }
		public Guid UserFromId { get; set; }

		[NotNull]
		public UserDTO UserTo { get; set; }
		public Guid UserToId { get; set; }

		public decimal Amount { get; set; }
		public decimal UserFromBalanceBeforeTransfer { get; set; }
		public decimal UserToBalanceBeforeTransfer { get; set; }
	}
}