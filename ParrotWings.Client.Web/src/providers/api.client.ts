import {Http} from '@angular/http';
import {OpaqueToken, Inject} from '@angular/core';
import 'rxjs/Rx';
import 'rxjs/add/operator/map'
import {AccountInfo} from '../model/account-info';
import {TransferSortingOrder, TransferInfo} from '../model/transfer-info';
import {UserInfo} from '../model/user-info';

export const ApiClientToken = new OpaqueToken('ApiClientToken');

export interface IApiClient {
	getAccountInfo(): Promise<AccountInfo>;
	register(username: string, email: string, password: string): Promise<any>;
	login(email: string, password: string): Promise<any>;
	logout(): Promise<any>;

	getUserByEmail(email: string):Promise<UserInfo>;
	getUsers(searchString: string, offset: number, count: number):Promise<UserInfo[]>;

	getTransfers(sortingOrder: TransferSortingOrder,
							 orderDescending: boolean,
							 offset?: number,
							 count?: number,
							 dateMin?: Date,
							 dateMax?: Date,
							 amountMin?: number,
							 amountMax?: number,
							 participantName?: string): Promise<TransferInfo[]>
}

export class ApiClient implements IApiClient {
	private baseUri: string;

	public constructor(@Inject(Http) private http: Http) {
		this.baseUri = "/api";
	}

	getAccountInfo(): Promise<AccountInfo> {
		let requestString = `${this.baseUri}/account/info`;

		return this.http
			.get(requestString)
			.toPromise()
			.then(response => response.json());
	}

	logout(): Promise<any> {
		let requestString = `${this.baseUri}/account/logout`;

		return this.http
			.post(requestString, {})
			.toPromise();
	}

	public register(username: string, email: string, password: string): Promise<any> {
		let requestString = `${this.baseUri}/account/register`;

		return this.http
			.post(requestString, {
				username: username,
				email: email,
				password: password
			})
			.toPromise();
	}

	public login(email: string, password: string): Promise<any> {
		let requestString = `${this.baseUri}/account/login`;

		return this.http
			.post(requestString, {
				email: email,
				password: password
			})
			.toPromise();
	}

	getUsers(searchString: string, offset: number, count: number): Promise<UserInfo[]> {
		let requestString = `${this.baseUri}/user?`;
		requestString += `searchString=${searchString}`;
		requestString += `offset=${offset}`;
		requestString += `count=${count}`;

		return this.http
			.get(requestString)
			.toPromise()
			.then(response => response.json());
	}

	getUserByEmail(email: string): Promise<UserInfo> {
		let requestString = `${this.baseUri}/user?email=${email}`;

		return this.http
			.get(requestString)
			.toPromise()
			.then(response => response.json());
	}

	public getTransfers(sortingOrder: TransferSortingOrder,
											orderDescending: boolean,
											offset?: number,
											count?: number,
											dateMin?: Date,
											dateMax?: Date,
											amountMin?: number,
											amountMax?: number,
											participantName?: string): Promise<TransferInfo[]> {

		let requestString = `${this.baseUri}/transfer?`;

		requestString += `order=${sortingOrder}`;
		requestString += `&orderDescending=${orderDescending}`;

		if (offset && offset > 0) {
			requestString += `&offset=${offset}`;
		}

		if (count && count > 0) {
			requestString += `&count=${count}`;
		}

		if (dateMin) {
			requestString += `&dateMin=${dateMin}`;
		}

		if (dateMax) {
			requestString += `&dateMax=${dateMax}`;
		}

		if (amountMin) {
			requestString += `&amountMin=${amountMin}`;
		}

		if (amountMax) {
			requestString += `&amountMax=${amountMax}`;
		}

		if (participantName) {
			requestString += `&participantName=${participantName}`;
		}

		return this.http
			.get(requestString)
			.toPromise()
			.then(response => response.json());
	}
}