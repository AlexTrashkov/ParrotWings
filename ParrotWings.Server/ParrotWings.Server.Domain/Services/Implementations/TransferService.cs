using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using ParrotWings.Server.Domain.Entities;
using ParrotWings.Server.Domain.Exceptions;
using ParrotWings.Server.Domain.Repositories;

namespace ParrotWings.Server.Domain.Services.Implementations
{
	internal sealed class TransferService : ITransferService
	{
		[NotNull] private readonly ITransferRepository _transferRepository;
		[NotNull] private readonly IUserRepository _userRepository;

		public TransferService(
			[NotNull] ITransferRepository transferRepository,
			[NotNull] IUserRepository userRepository)
		{
			_transferRepository = transferRepository;
			_userRepository = userRepository;
		}

		public async Task<Transfer> CreateTransferAsync(Guid userFromId, Guid userToId, decimal transferAmount)
		{
			if (transferAmount <= 0)
			{
				throw new IncorrectTransferAmountException();
			}

			var userFrom = await _userRepository.GetByAsync(userFromId);
			var userTo = await _userRepository.GetByAsync(userToId);

			if (userFrom.Equals(userTo))
			{
				throw new CannotCreateTransferForHimselfException();
			}

			if (userFrom.CurrentBalance < transferAmount)
			{
				throw new NotSufficientFundsException();
			}

			var createdTransfer = new Transfer(userFrom, userTo, transferAmount, userFrom.CurrentBalance, userTo.CurrentBalance);

			userFrom.CurrentBalance = userFrom.CurrentBalance - transferAmount;
			userTo.CurrentBalance = userTo.CurrentBalance + transferAmount;

			await _transferRepository.SaveTransferAndUpdateUsersBalancesAsync(createdTransfer);

			return createdTransfer;
		}
	}
}