import {NgModule} from '@angular/core';
import {AppComponent} from './app.component';
import {BrowserModule} from '@angular/platform-browser';
import {HttpModule, Http} from '@angular/http';
import {ApiClientToken, ApiClient} from '../providers/api.client';
import {RouterModule, PreloadAllModules}   from '@angular/router';
import {FormsModule} from '@angular/forms';
import {TranslateModule, TranslateLoader, TranslateStaticLoader} from 'ng2-translate';
import {LoginComponent} from '../components/login/login.component';
import {AppRoutes} from './app.routes';
import {APP_BASE_HREF} from '@angular/common';
import {RegisterComponent} from '../components/register/register.component';
import {AlertService} from '../providers/alert.service';
import {AlertComponent} from '../components/alert/alert.component';
import {HomeComponent} from '../components/home/home.component';
import {TransferInfoComponent} from '../components/transfer-info/transfer-info.component';
import {MomentModule} from 'angular2-moment';
import {TransferHubToken, TransferHub} from '../providers/transfer.hub';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {DatePickerModule} from 'ng2-datepicker/index';
import {CreateTransferComponent} from '../components/create-transfer/create-transfer.component';
import { Ng2CompleterModule } from "ng2-completer";

@NgModule({
	bootstrap: [AppComponent],
	declarations: [
		AppComponent,
		LoginComponent,
		RegisterComponent,
		HomeComponent,
		TransferInfoComponent,
		CreateTransferComponent,

		AlertComponent,
	],
	imports: [
		BrowserModule,
		HttpModule,
		FormsModule,
		MomentModule,
		DatePickerModule,
		Ng2CompleterModule,
		NgbModule.forRoot(),
		RouterModule.forRoot(AppRoutes, {useHash: true, preloadingStrategy: PreloadAllModules}),
		TranslateModule.forRoot({
			provide: TranslateLoader,
			useFactory: (http: Http) => new TranslateStaticLoader(http, 'i18n', '.json'),
			deps: [Http]
		})
	],
	providers: [
		{provide: APP_BASE_HREF, useValue: '/'},
		{provide: ApiClientToken, useClass: ApiClient},
		{provide: TransferHubToken, useClass: TransferHub},
		AlertService
	]
})
export class AppModule {
	constructor() {
	}
}