export interface TransferInfo {
	transferId: string;
	currentUserId: string;
	partnerId: string;
	partnerName: string;
	partnerEmail: string;
	amount: number;
	createDate: Date;
	currentUserBalanceBeforeTransfer: number;
	currentUserBalanceAfterTransfer: number;
	isFresh: boolean;
}

export enum TransferSortingOrder {
	CreateAt = 0,
	Amount = 1
}