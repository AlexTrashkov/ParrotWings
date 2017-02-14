using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using ParrotWings.Server.Domain.Entities;

namespace ParrotWings.Server.Domain.Repositories
{
	public enum TransferSortingOrder
	{
		CreateAt = 0,
		Amount = 1
	}

	public interface ITransferRepository
	{
		[NotNull]
		[ItemNotNull]
		Task<IReadOnlyCollection<Transfer>> ListBy(
			Guid executingUserid,
			TransferSortingOrder order,
			bool orderDescending,
			uint offset,
			uint count,
			[CanBeNull] DateTime? dateMin,
			[CanBeNull] DateTime? dateMax,
			[CanBeNull] decimal? amountMin,
			[CanBeNull] decimal? amountMax,
			[CanBeNull] string participantName);

		[NotNull]
		Task SaveTransferAndUpdateUsersBalancesAsync([NotNull] Transfer createdTransfer);
	}
}