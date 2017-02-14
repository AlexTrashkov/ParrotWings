using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using JetBrains.Annotations;
using ParrotWings.Server.Domain.Entities;
using ParrotWings.Server.Domain.Repositories;
using ParrotWings.Server.Storage.DTO;

namespace ParrotWings.Server.Storage.Repositories
{
	internal sealed class TransferRepository : ITransferRepository
	{
		[NotNull] private readonly ParrotWingsDataContext _dataContext;
		[NotNull] private readonly IMapper _mapper;

		public TransferRepository([NotNull] ParrotWingsDataContext dataContext, [NotNull] IMapper mapper)
		{
			_mapper = mapper;
			_dataContext = dataContext;
		}

		public async Task<IReadOnlyCollection<Transfer>> ListBy(
			Guid executingUserid,
			TransferSortingOrder order,
			bool orderDescending,
			uint offset,
			uint count,
			DateTime? dateMin,
			DateTime? dateMax,
			decimal? amountMin,
			decimal? amountMax,
			string participantName)
		{
			var query = _dataContext.Transfers
				.Include(x => x.UserTo)
				.Include(x => x.UserFrom)
				.Where(x =>
					(x.UserFrom.Id == executingUserid || x.UserTo.Id == executingUserid) &&
					(dateMin == null || x.CreateDate >= dateMin.Value) &&
					(dateMax == null || x.CreateDate <= dateMax.Value) &&
					(amountMin == null || x.Amount >= amountMin.Value) &&
					(amountMax == null || x.Amount <= amountMax.Value) &&

					(participantName == null ||

					 (x.UserTo.Id != executingUserid &&
					  (x.UserTo.UserName.ToLower().Contains(participantName.ToLower()) ||
					   x.UserTo.Email.ToLower().Contains(participantName.ToLower()))) ||

					 (x.UserFrom.Id != executingUserid &&
					  (x.UserFrom.UserName.ToLower().Contains(participantName.ToLower()) ||
					   x.UserFrom.Email.ToLower().Contains(participantName.ToLower())))));

			IQueryable<TransferDTO> queryWithOrder;
			switch (order)
			{
				case TransferSortingOrder.Amount:
					queryWithOrder = orderDescending
						? query.OrderByDescending(x => x.Amount)
						: query.OrderBy(x => x.Amount);
					break;

				case TransferSortingOrder.CreateAt:
					queryWithOrder = orderDescending
						? query.OrderByDescending(x => x.CreateDate)
						: query.OrderBy(x => x.CreateDate);
					break;

				default:
					throw new ArgumentException(nameof(order));
			}

			var resultDto = await queryWithOrder
				.Skip((int) offset)
				.Take((int) count)
				.ToArrayAsync();

			return resultDto
				.Select(_mapper.Map<TransferDTO, Transfer>)
				.ToArray();
		}

		public async Task SaveTransferAndUpdateUsersBalancesAsync(Transfer createdTransfer)
		{
			using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
			{
				var createdTransferDto = _mapper.Map<Transfer, TransferDTO>(createdTransfer);

				_dataContext.Transfers.Add(createdTransferDto);
				_dataContext.Entry(createdTransferDto.UserTo).State = EntityState.Detached;
				_dataContext.Entry(createdTransferDto.UserFrom).State = EntityState.Detached;

				var userFromDto = await _dataContext.Users.FirstAsync(x => x.Id == createdTransfer.UserFrom.Id);
				userFromDto.CurrentBalance = createdTransfer.UserFrom.CurrentBalance;

				var userToDto = await _dataContext.Users.FirstAsync(x => x.Id == createdTransfer.UserTo.Id);
				userToDto.CurrentBalance = createdTransfer.UserTo.CurrentBalance;

				await _dataContext.SaveChangesAsync();
				transactionScope.Complete();
			}
		}
	}
}