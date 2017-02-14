using System;
using JetBrains.Annotations;
using ParrotWings.Server.Domain.Exceptions;

namespace ParrotWings.Server.Domain.Entities
{
	public sealed class Transfer
	{
		public Guid Id { get; }
		public DateTime CreateDate { get; }

		[NotNull]
		public User UserFrom { get; }

		[NotNull]
		public User UserTo { get; }

		public decimal Amount { get; }

		public decimal UserFromBalanceBeforeTransfer { get; }
		public decimal UserFromBalanceAfterTransfer { get; }

		public decimal UserToBalanceBeforeTransfer { get; }
		public decimal UserToBalanceAfterTransfer { get; }

		public Transfer(
			[NotNull] User userFrom,
			[NotNull] User userTo,
			decimal amount,
			decimal userFromBalanceBeforeTransfer,
			decimal userToBalanceBeforeTransfer)
			: this(
				Guid.NewGuid(),
				DateTime.UtcNow,
				userFrom,
				userTo,
				amount,
				userFromBalanceBeforeTransfer,
				userToBalanceBeforeTransfer)
		{
		}

		public Transfer(
			Guid id,
			DateTime createDate,
			[NotNull] User userFrom,
			[NotNull] User userTo,
			decimal amount,
			decimal userFromBalanceBeforeTransfer,
			decimal userToBalanceBeforeTransfer)
		{
			Id = id;
			CreateDate = createDate;

			if (userFrom.Equals(userTo))
			{
				throw new CannotCreateTransferForHimselfException();
			}

			UserFrom = userFrom;
			UserTo = userTo;

			if (amount <= 0)
			{
				throw new IncorrectTransferAmountException();
			}

			Amount = amount;

			if (userFromBalanceBeforeTransfer < 0)
			{
				throw new UserBalanceLessThanZeroException();
			}

			if (userFromBalanceBeforeTransfer < amount)
			{
				throw new NotSufficientFundsException();
			}

			UserFromBalanceBeforeTransfer = userFromBalanceBeforeTransfer;
			UserFromBalanceAfterTransfer = userFromBalanceBeforeTransfer - amount;

			if (userToBalanceBeforeTransfer < 0)
			{
				throw new UserBalanceLessThanZeroException();
			}

			UserToBalanceBeforeTransfer = userToBalanceBeforeTransfer;
			UserToBalanceAfterTransfer = userToBalanceBeforeTransfer + amount;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hash = 17;

				hash = hash * 23 + Id.GetHashCode();
				hash = hash * 23 + CreateDate.GetHashCode();
				hash = hash * 23 + UserFrom.GetHashCode();
				hash = hash * 23 + UserTo.GetHashCode();
				hash = hash * 23 + Amount.GetHashCode();
				hash = hash * 23 + UserFromBalanceBeforeTransfer.GetHashCode();
				hash = hash * 23 + UserFromBalanceAfterTransfer.GetHashCode();
				hash = hash * 23 + UserToBalanceBeforeTransfer.GetHashCode();
				hash = hash * 23 + UserToBalanceAfterTransfer.GetHashCode();

				return hash;
			}
		}

		public override bool Equals(object value)
		{
			var valueAsTransfer = value as Transfer;

			if (valueAsTransfer == null)
			{
				return false;
			}

			if (ReferenceEquals(this, valueAsTransfer))
			{
				return true;
			}

			return Equals(valueAsTransfer);
		}

		public bool Equals([CanBeNull] Transfer value)
		{
			if (value == null)
			{
				return false;
			}

			return
				Id.Equals(value.Id) &&
				CreateDate.Equals(value.CreateDate) &&
				UserFrom.Equals(value.UserFrom) &&
				UserTo.Equals(value.UserTo) &&
				Amount.Equals(value.Amount) &&
				UserFromBalanceBeforeTransfer.Equals(value.UserFromBalanceBeforeTransfer) &&
				UserFromBalanceAfterTransfer.Equals(value.UserFromBalanceAfterTransfer) &&
				UserToBalanceBeforeTransfer.Equals(value.UserToBalanceBeforeTransfer) &&
				UserToBalanceAfterTransfer.Equals(value.UserToBalanceAfterTransfer);
		}
	}
}