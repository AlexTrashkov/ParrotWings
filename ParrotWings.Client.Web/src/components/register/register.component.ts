import {Component, Inject} from '@angular/core';
import {Router} from '@angular/router';
import {IApiClient, ApiClientToken} from '../../providers/api.client';
import {AlertService} from '../../providers/alert.service';

@Component({
	template: require('./register.component.html')
})

export class RegisterComponent {
	loading = false;
	model: RegisterModel = {
		email: '',
		username: '',
		password: '',
		confirmPassword: ''
	};

	constructor(private router: Router,
							private alertService: AlertService,
							@Inject(ApiClientToken) private apiClient: IApiClient) {
	}

	register() {
		if (this.model.password !== this.model.confirmPassword) {
			this.alertService.error('Passwords are not equal');
			return;
		}

		this.loading = true;
		this.apiClient.register(this.model.username, this.model.email, this.model.password)
			.then(
				data => {
					this.alertService.success('Registration successful', true);
					this.router.navigate(['/login']);
				})
			.catch(
				(error) => {
					this.loading = false;
					this.alertService.error(error.json());
				});
	}
}

interface RegisterModel {
	username: string;
	email: string;
	password: string;
	confirmPassword: string;
}
