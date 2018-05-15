import { Injectable } from "@angular/core"
import { Observable } from 'rxjs/Rx';
import { SignalRHubService } from "../dal/signalr/signalr-hub.service";
import { TransportType, HubConnection } from "@aspnet/signalr";
import { RefreshBracketModel, ChageWeightDivisionModel } from "../model/bracket.models";
import { RoundModel } from '../model/round.models';


@Injectable()
export class RunEventHubService {
	private hubConnection: HubConnection;
	private roundStartEventName: string = "RoundStart";
	private roundCompleteEventName: string = "RoundComplete";
    private weightDivisionChangeEventName: string = "WeightDivisionChanged";

	isConnected: boolean = false;

	constructor(private signalRService: SignalRHubService) {
        this.hubConnection = this.signalRService.createConnection("/runevent");
	}

    joinWeightDivisionGroup(weightDivisionId: string, previousWeightDivisionId?: string): Promise<void> {
		let id = weightDivisionId;
		if (this.isConnected) {
			if (!!previousWeightDivisionId) {
				this.signalRService.leaveGroup(previousWeightDivisionId);
			}
            this.signalRService.joinGroup(id);
            return new Promise(resolve => resolve());
		} else {
			this.signalRService.onConnected().subscribe(() => this.signalRService.joinGroup(id));
			this.signalRService.onDisconnected().subscribe(() => this.signalRService.leaveGroup(id));
			return this.connect();
		}
	}

	joinOperatorGroup(userId:string) {
		if (this.isConnected) {
			this.signalRService.joinGroup(userId);
		} else {
			this.signalRService.onConnected().subscribe(() => this.signalRService.joinGroup(userId));
			this.signalRService.onDisconnected().subscribe(() => this.signalRService.leaveGroup(userId));
			this.connect();
		}
	}


	joinMultipleWeightDivisionGroups(weightDivisionIds: string[]) {
		if (!this.isConnected) {
			this.signalRService.onConnected().subscribe(() => this.joinGroups(weightDivisionIds));
			this.signalRService.onDisconnected().subscribe(() => this.leaveGroups(weightDivisionIds));
			this.connect();
		}
	}

	joinGroups(groupNames: string[]) {
		for (var groupName of groupNames) {
			this.signalRService.joinGroup(groupName);
		}
	}

	leaveGroups(groupNames: string[]) {
		for (var groupName of groupNames) {
			this.signalRService.leaveGroup(groupName);
		}
	}
	
	fireRoundStart(roundDetails: RoundModel): void {
		this.signalRService.fireEvent(this.roundStartEventName, roundDetails);
	}

	onRoundStart(): Observable<RoundModel> {
		return this.signalRService.subscribeOnEvent(this.roundStartEventName);
    }

    onWeightDivisionChange(): Observable<RefreshBracketModel> {
        return this.signalRService.subscribeOnEvent(this.weightDivisionChangeEventName);
    }

    fireWeightDivisionChange(model: ChageWeightDivisionModel): void {
        this.signalRService.fireEvent(this.weightDivisionChangeEventName, model);
    }

	fireRoundComplete(weightDivisionId: string): void {
		this.signalRService.fireEvent(this.roundCompleteEventName, weightDivisionId);
	}

	onRoundComplete(): Observable<RefreshBracketModel> {
		return this.signalRService.subscribeOnEvent(this.roundCompleteEventName);
	}

    connect(): Promise<void>{
		if (this.isConnected) {
            return new Promise((resolve, reject) => reject());
		}
		this.signalRService.bindReconnection();
		const promise = this.signalRService.start();
        this.isConnected = true;
        return promise;

    }

	disconnect() {
		const promise = this.signalRService.stop();
        this.isConnected = false;
	    return promise;
	}
}