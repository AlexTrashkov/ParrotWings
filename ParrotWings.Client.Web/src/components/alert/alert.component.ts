import {Component, OnInit} from '@angular/core';
import {AlertService} from '../../providers/alert.service';

@Component({
	selector: 'alert',
	template: require('./alert.component.html')
})

export class AlertComponent implements OnInit {
	message: any;

	constructor(private alertService: AlertService) {
	}

	ngOnInit() {
		this.alertService.getMessage().subscribe(message => {
			this.message = message;
		});
	}
}