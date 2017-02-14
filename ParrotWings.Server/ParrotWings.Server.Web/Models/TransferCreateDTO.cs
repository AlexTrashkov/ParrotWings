using System;

namespace ParrotWings.Server.Web.Models
{
	public sealed class TransferCreateDTO
	{
		public Guid UserToId { get; set; }
		public decimal Amount { get; set; }
	}
}