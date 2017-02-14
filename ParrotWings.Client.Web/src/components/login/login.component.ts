import {Component, OnInit, Inject} from '@angular/core';
import {Router, ActivatedRoute} from '@angular/router';
import {IApiClient, ApiClientToken} from '../../providers/api.client';
import {AlertService} from '../../providers/alert.service';

@Component({
	template: require('./login.component.html')
})

export class LoginComponent implements OnInit {
	loading = false;
	model: LoginModel = {
		email: '',
		password: ''
	};

	constructor(private route: ActivatedRoute,
							private router: Router,
							private alertService: AlertService,
							@Inject(ApiClientToken) private apiClient: IApiClient) {
	}

	ngOnInit() {
	}

	login() {
		this.loading = true;
		this.apiClient.login(this.model.email, this.model.password)
			.then(
				data => {
					this.router.navigate(['home']);
				})
			.catch(
				error => {
					this.alertService.error(error.json());
					this.loading = false;
				});
	}
}

interface LoginModel {
	email: string;
	password: string;
}