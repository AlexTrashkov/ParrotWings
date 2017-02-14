using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using ParrotWings.Server.Domain.Entities;

namespace ParrotWings.Server.Domain.Services
{
	public delegate void TransferCreatedEventHandler([NotNull] Transfer createdTransfer);

	public interface ITransferService
	{
		[NotNull]
		Task<Transfer> CreateTransferAsync(Guid userFromId, Guid userToId, decimal transferAmount);
	}
}