import { Component, Input, OnInit } from "@angular/core";
import { BracketModel } from '../../../core/model/bracket.models';
import { ParticipantModelBase } from '../../../core/model/participant.models';
import { SelectItem } from "primeng/primeng";
import { BracketService } from '../../../core/services/bracket.service';
import { BracketResultModel } from '../../../core/model/bracket-result.model';


@Component({
    selector: 'bracket-result-set',
    templateUrl: './bracket-result-set.component.html',
})



export class BracketResultSetComponent {

    @Input() bracket: BracketModel;
    participants: ParticipantModelBase[] = [];
    participantSelectItems: SelectItem[] = [];
    firstPlaceParticipantId: AAGUID;
    secondPlaceParticipantId: AAGUID;
    thirdPlaceParticipantId: AAGUID;
    bracketResult: BracketResultModel;

    constructor(private bracketService: BracketService) {

    }

    ngOnInit() {
        for (var i = 0; i < this.bracket.roundModels.length; i++) {
            if (this.bracket.roundModels[i].AParticipant) {
                this.participantSelectItems.push(this.getSelectItem(this.bracket.roundModels[i].AParticipant));
            }
            if (this.bracket.roundModels[i].secondParticipant) {
                this.participantSelectItems.push(this.getSelectItem(this.bracket.roundModels[i].secondParticipant));
            }
        }
        this.firstPlaceParticipantId = this.participantSelectItems[0] ? this.participantSelectItems[0].value : '00000000-0000-0000-0000-000000000000';
        this.secondPlaceParticipantId = this.participantSelectItems[1] ? this.participantSelectItems[1].value : '00000000-0000-0000-0000-000000000000';
        this.thirdPlaceParticipantId = this.participantSelectItems[2] ? this.participantSelectItems[2].value : '00000000-0000-0000-0000-000000000000';
    }

    private getSelectItem(participant: ParticipantModelBase): SelectItem {
        return { label: participant.firstName + ' ' + participant.lastName, value: participant.participantId };
    }

    private setMedalists() {
        this.bracketResult = {
            bracketId: this.bracket.bracketId,
            firstPlaceParticipantId: this.firstPlaceParticipantId,
            secondPlaceParticipantId: this.secondPlaceParticipantId,
            thirdPlaceParticipantId: this.thirdPlaceParticipantId
        }
        this.bracketService.setBracketResult(this.bracketResult).subscribe();
    }

}




