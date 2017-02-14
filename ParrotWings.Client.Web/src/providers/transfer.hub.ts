import 'expose-loader?jQuery!jquery';
import 'expose-loader?$!jquery';
import 'ms-signalr-client';
import {EventEmitter, OpaqueToken} from '@angular/core';
import Proxy = SignalR.Hub.Proxy;
import Connection = SignalR.Hub.Connection;
import {TransferInfo} from '../model/transfer-info';

export const TransferHubToken = new OpaqueToken('TransferHubToken');

export interface ITransferHub {
	onTransferCreated: EventEmitter<TransferInfo>;
	createTransfer(userToId: string, amount: number): JQueryPromise<TransferInfo>;
	connect(): void;
	disconnect(): void;
}

export class TransferHub implements ITransferHub {
	private isConnected: boolean;
	private connection: Connection;
	private proxy: Proxy;
	public onTransferCreated: EventEmitter<TransferInfo> = new EventEmitter<TransferInfo>();

	constructor() {
		this.connection = $.hubConnection('/signalr');
		this.proxy = this.connection.createHubProxy('TransferHub');
		this.isConnected = false;
	}

	public connect(): void {
		if (this.isConnected)
			return;

		this.isConnected = true;

		this.connection.start({jsonp: true})
			.fail((error) => console.log('Could not connect', error));

		this.proxy.on('TransferCreated', (transferInfo: TransferInfo) => {
			transferInfo.isFresh = true;
			this.onTransferCreated.emit(transferInfo)
		});
	}

	public createTransfer(userToId: string, amount: number): JQueryPromise<TransferInfo> {
		return this.proxy.invoke('CreateTransfer', userToId, amount);
	}

	public disconnect(): void {
		this.isConnected = false;
		this.connection.stop();
	}
}