using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using JetBrains.Annotations;
using ParrotWings.Server.Domain.Repositories;
using ParrotWings.Server.Web.Extensions;
using ParrotWings.Server.Web.Models;

namespace ParrotWings.Server.Web.Controllers
{
	[Authorize]
	public sealed class TransferController : ApiController
	{
		[NotNull] private readonly ITransferRepository _transferRepository;
		[NotNull] private readonly IUserRepository _userRepository;

		public TransferController(
			[NotNull] ITransferRepository transferRepository, 
			[NotNull]IUserRepository userRepository)
		{
			_transferRepository = transferRepository;
			_userRepository = userRepository;
		}

		public async Task<IReadOnlyCollection<TransferInfoDTO>> Get(
			TransferSortingOrder order = TransferSortingOrder.CreateAt,
			bool orderDescending = false,
			uint offset = 0,
			uint count = 100,
			[CanBeNull] DateTime? dateMin = null,
			[CanBeNull] DateTime? dateMax = null,
			[CanBeNull] decimal? amountMin = null,
			[CanBeNull] decimal? amountMax = null,
			[CanBeNull] string participantName = null)
		{
			var currentUserId = User.Identity.GetUserIdAsGuid();

			var currentUser = await _userRepository.GetByAsync(currentUserId);

			var transfers = await _transferRepository.ListBy(
				currentUserId,
				order,
				orderDescending,
				offset,
				count,
				dateMin,
				dateMax,
				amountMin,
				amountMax,
				participantName);

			//TODO move sorting into service

			TransferInfoDTO[] transferInfoDTOs = null;

			if (orderDescending)
			{
				if (order == TransferSortingOrder.Amount)
				{
					transferInfoDTOs = transfers
						.Select(transfer => new TransferInfoDTO(transfer, currentUser))
						.OrderBy(x => x.Amount)
						.ToArray();
				}

				if (order == TransferSortingOrder.CreateAt)
				{
					transferInfoDTOs = transfers
						.Select(transfer => new TransferInfoDTO(transfer, currentUser))
						.OrderBy(x => x.CreateDate)
						.ToArray();
				}
			}
			else
			{
				if (order == TransferSortingOrder.Amount)
				{
					transferInfoDTOs = transfers
						.Select(transfer => new TransferInfoDTO(transfer, currentUser))
						.OrderByDescending(x => x.Amount)
						.ToArray();
				}

				if (order == TransferSortingOrder.CreateAt)
				{
					transferInfoDTOs = transfers
						.Select(transfer => new TransferInfoDTO(transfer, currentUser))
						.OrderByDescending(x => x.CreateDate)
						.ToArray();
				}
			}

			return transferInfoDTOs;
		}
	}
}