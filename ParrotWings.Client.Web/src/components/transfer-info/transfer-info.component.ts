import {Component} from '@angular/core';
import {TransferInfo} from '../../model/transfer-info';

@Component({
	template: require('./transfer-info.component.html'),
	selector: 'transfer-info',
	inputs: ['transfer']
})

export class TransferInfoComponent {
	public transfer: TransferInfo;
}