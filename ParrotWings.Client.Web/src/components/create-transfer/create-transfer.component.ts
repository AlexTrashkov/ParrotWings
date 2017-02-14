import {Component, OnInit, Inject} from '@angular/core';
import {Router, ActivatedRoute} from '@angular/router';
import {ApiClientToken, IApiClient} from '../../providers/api.client';
import {TransferHubToken, TransferHub} from '../../providers/transfer.hub';
import {AccountInfo} from '../../model/account-info';
import {CompleterService, CompleterData} from 'ng2-completer';
import {UserInfo} from '../../model/user-info';

@Component({
	selector: 'create-transfer',
	template: require('./create-transfer.component.html')
})
export class CreateTransferComponent implements OnInit {
	private dataService: CompleterData;

	accountInfo: AccountInfo = {
		id: '',
		createDate: undefined,
		userName: '',
		email: '',
		currentBalance: 0
	};

	userToInfo: UserInfo = {
		id: '',
		email: '',
		userName: ''
	};

	amount: number = 0;
	userEmail: string = '';

	constructor(private router: Router,
							private activatedRoute: ActivatedRoute,
							private completerService: CompleterService,
							@Inject(ApiClientToken) private apiClient: IApiClient,
							@Inject(TransferHubToken) private transferHub: TransferHub) {
		this.dataService = completerService.remote('/api/user?searchString=', 'userName,email', 'email');

		this.dataService.subscribe(() => this.tryResolveUser());
	}
	ngOnInit() {
		this.apiClient.getAccountInfo()
			.then(accountInfo => {
				this.accountInfo = accountInfo;

				this.transferHub.connect();
				this.transferHub.onTransferCreated.subscribe(() => {
					this.apiClient.getAccountInfo()
						.then(accountInfo => {
							this.accountInfo = accountInfo;
						});
				});
			})
			.catch(()=>this.router.navigate(['login']))

		this.activatedRoute.params.subscribe((params: any) => {
			if (params.userToEmail) {
				this.userEmail = params.userToEmail;
				this.tryResolveUser();
			}
			if (params.amount) {
				this.amount = params.amount;
			}
		});
	}

	tryResolveUser() {
		this.userToInfo = null;
		this.apiClient.getUserByEmail(this.userEmail)
			.then(userInfo => {
					this.userToInfo = userInfo;
				}
			);
	}

	createTransfer() {
		this.transferHub.createTransfer(this.userToInfo.id, this.amount)
			.then(()=>this.router.navigate(['home']));
	}

	logout() {
		this.transferHub.disconnect();
		this.apiClient.logout()
			.then(()=>this.router.navigate(['login']));
	}

	isNumber(value) {
		if ((undefined === value) || (null === value)) {
			return false;
		}
		if (typeof value == 'number') {
			return true;
		}
		return !isNaN(value - 0);
	}
}