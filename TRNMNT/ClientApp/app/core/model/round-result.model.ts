import { ParticipantModelBase } from './participant.models';

export class RoundResultModel {
    roundId: AAGUID;
    winnerParticipantId: AAGUID;
    AParticipantPoints: number;
    AParticipantAdvantages: number;
    AParticipantPenalties: number;
    BParticipantPoints: number;
    BParticipantAdvantages: number;
    BParticipantPenalties: number;
    submissionType : number;
    roundResultType : number;
    completeTime: number;
}