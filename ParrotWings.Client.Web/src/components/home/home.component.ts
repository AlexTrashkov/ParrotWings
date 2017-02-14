import {Component, OnInit, Inject} from '@angular/core';
import {Router} from '@angular/router';
import {ApiClientToken, IApiClient} from '../../providers/api.client';
import {AccountInfo} from '../../model/account-info';
import {TransferInfo, TransferSortingOrder} from '../../model/transfer-info';
import {TransferHubToken, TransferHub} from '../../providers/transfer.hub';
import moment = require('moment');

@Component({
	selector: 'home',
	template: require('./home.component.html')
})
export class HomeComponent implements OnInit {
	accountInfo: AccountInfo = {
		id: '',
		createDate: undefined,
		userName: '',
		email: '',
		currentBalance: 0
	};

	isLoading: boolean = false;
	transfers: TransferInfo[] = [];

	sortingOrder: TransferSortingOrder = TransferSortingOrder.Amount;
	orderDescending: boolean = false;
	offset?: number = 0;
	count?: number = 50;
	dateMin?: any = null;
	dateMax?: any = null;
	amountMin?: number = null;
	amountMax?: number = null;
	participantName?: string = null;

	constructor(private router: Router,
							@Inject(ApiClientToken) private apiClient: IApiClient,
							@Inject(TransferHubToken) private transferHub: TransferHub) {
	}

	ngOnInit() {
		this.apiClient.getAccountInfo()
			.then(accountInfo => {
				this.accountInfo = accountInfo;

				this.refreshTransfers();

				this.transferHub.connect();
				this.transferHub.onTransferCreated.subscribe(transferInfo => {
					this.transfers.unshift(transferInfo);

					this.apiClient.getAccountInfo()
						.then(accountInfo => {
							this.accountInfo = accountInfo;
						});
				});
			})
			.catch(()=>this.router.navigate(['login']))
	}

	updateSortingOrder(sortingOrder: TransferSortingOrder) {
		this.sortingOrder = sortingOrder;
		this.refreshTransfers();
	}

	updateSortingDirection(orderDescending: boolean) {
		this.orderDescending = orderDescending;
		this.refreshTransfers();
	}

	refreshTransfers() {
		this.transfers = [];
		this.isLoading = true;

		const dateMinFormatted =
			this.dateMin
				? (<any>this.dateMin).formatted
				: null;

		const dateMaxFormatted =
			this.dateMax
				? (<any>this.dateMax).formatted
				: null;

		this.apiClient.getTransfers(
			this.sortingOrder,
			this.orderDescending,
			this.offset,
			this.count,
			dateMinFormatted,
			dateMaxFormatted,
			this.amountMin,
			this.amountMax,
			this.participantName)
			.then(transfers=> {
				this.transfers = transfers;
				this.isLoading = false;
			});
	}

	logout() {
		this.transferHub.disconnect();
		this.apiClient.logout()
			.then(()=>this.router.navigate(['login']));
	}

	transferSortingOrder = TransferSortingOrder; //export enum
}