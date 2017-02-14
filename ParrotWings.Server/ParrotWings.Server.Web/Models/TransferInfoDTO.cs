using System;
using ParrotWings.Server.Domain.Entities;

namespace ParrotWings.Server.Web.Models
{
	public sealed class TransferInfoDTO
	{
		public Guid TransferId { get; }
		public Guid CurrentUserId { get; }

		public Guid PartnerId { get; }
		public string PartnerName { get; }
		public string PartnerEmail { get; }

		public DateTime CreateDate { get; }
		public decimal Amount { get; }

		public decimal CurrentUserBalanceBeforeTransfer { get; }
		public decimal CurrentUserBalanceAfterTransfer { get; }

		public TransferInfoDTO(Transfer transfer, User user)
		{
			TransferId = transfer.Id;
			CreateDate = transfer.CreateDate;

			if (transfer.UserFrom.Equals(user))
			{
				PartnerId = transfer.UserTo.Id;
				PartnerName = transfer.UserTo.UserName;
				PartnerEmail = transfer.UserTo.Email;

				Amount = -transfer.Amount;

				CurrentUserId = transfer.UserFrom.Id;
				CurrentUserBalanceBeforeTransfer = transfer.UserFromBalanceBeforeTransfer;
				CurrentUserBalanceAfterTransfer = transfer.UserFromBalanceAfterTransfer;
			}

			else if (transfer.UserTo.Equals(user))
			{
				PartnerId = transfer.UserFrom.Id;
				PartnerName = transfer.UserFrom.UserName;
				PartnerEmail = transfer.UserFrom.Email;

				Amount = transfer.Amount;

				CurrentUserId = transfer.UserTo.Id;
				CurrentUserBalanceBeforeTransfer = transfer.UserToBalanceBeforeTransfer;
				CurrentUserBalanceAfterTransfer = transfer.UserToBalanceAfterTransfer;
			}
			else
			{
				throw new ArgumentException();
			}
		}
	}
}