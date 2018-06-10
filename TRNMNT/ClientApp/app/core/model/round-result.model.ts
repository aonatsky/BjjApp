import { ParticipantModelBase } from './participant.models';

export class RoundResultModel {
    roundId: AAGUID;
    winnerParticipantId: AAGUID;
    AParticipantPoints: number;
    AParticipantAdvantages: number;
    AParticipantPenalties: number;
    secondParticipantPoints: number;
    secondParticipantAdvantages: number;
    secondParticipantPenalties: number;
    submissionType : number;
    roundResultType : number;
    completeTime: number;
}