import {Routes} from '@angular/router';
import {LoginComponent} from '../components/login/login.component';
import {RegisterComponent} from '../components/register/register.component';
import {HomeComponent} from '../components/home/home.component';
import {CreateTransferComponent} from '../components/create-transfer/create-transfer.component';

export const AppRoutes: Routes = [
	{path: 'home', component: HomeComponent},
	{path: 'login', component: LoginComponent},
	{path: 'register', component: RegisterComponent},
	{path: 'create-transfer', component: CreateTransferComponent},
	{path: 'create-transfer/:userToEmail/:amount', component: CreateTransferComponent},

	{path: '**', redirectTo: 'home'}
];