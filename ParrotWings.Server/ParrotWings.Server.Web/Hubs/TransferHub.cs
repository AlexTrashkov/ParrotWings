using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNet.SignalR;
using ParrotWings.Server.Domain.Services;
using ParrotWings.Server.Web.Extensions;
using ParrotWings.Server.Web.Models;

namespace ParrotWings.Server.Web.Hubs
{
	[Authorize]
	public sealed class TransferHub : Hub
	{
		[NotNull] private readonly ITransferService _transferService;

		public TransferHub([NotNull] ITransferService transferService)
		{
			_transferService = transferService;
		}

		public async Task<TransferInfoDTO> CreateTransfer(Guid userToId, decimal amount)
		{
			var userFromId = Context.User.Identity.GetUserIdAsGuid();
			var transfer = await _transferService.CreateTransferAsync(userFromId, userToId, amount);

			Clients
				.Group(userFromId.ToString())
				.TransferCreated(new TransferInfoDTO(transfer, transfer.UserFrom));

			Clients
				.Group(userToId.ToString())
				.TransferCreated(new TransferInfoDTO(transfer, transfer.UserTo));

			return new TransferInfoDTO(transfer, transfer.UserFrom);
		}

		public override Task OnConnected()
		{
			var userId = Context.User.Identity.GetUserIdAsGuid();
			return Groups.Add(Context.ConnectionId, userId.ToString());
		}

		public override Task OnReconnected()
		{
			var userId = Context.User.Identity.GetUserIdAsGuid();
			return Groups.Remove(Context.ConnectionId, userId.ToString());
		}
	}
}
