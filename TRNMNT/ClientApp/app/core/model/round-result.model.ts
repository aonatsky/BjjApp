import { ParticipantModelBase } from './participant.models';

export class RoundResultModel {
    roundId: AAGUID;
    winnerParticipantId: AAGUID;
    firstParticipantPoints: number;
    firstParticipantAdvantages: number;
    firstParticipantPenalties: number;
    secondParticipantPoints: number;
    secondParticipantAdvantages: number;
    secondParticipantPenalties: number;
    submissionType : number;
    roundResultType : number;
    completeTime: number;
}