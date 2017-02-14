import {Component, OnInit, Inject, ViewEncapsulation} from '@angular/core';
import {IApiClient, ApiClientToken} from '../providers/api.client';
import {TranslateService} from 'ng2-translate';
import {Router} from '@angular/router';
import * as $ from 'jquery';

@Component({
	selector: 'app',
	template: require('./app.component.html'),
	styles: [require('./app.component.scss')],
	encapsulation: ViewEncapsulation.None,
})
export class AppComponent implements OnInit {

	constructor(@Inject(ApiClientToken) private apiClient: IApiClient,
							translate: TranslateService,
							private router: Router) {

		translate.setDefaultLang('en');
		translate.use('en');
	}

	ngOnInit(): void {
	}
}