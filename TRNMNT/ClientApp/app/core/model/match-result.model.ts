export class MatchResultModel {
    roundId: AAGUID;
    winnerParticipantId: AAGUID;
    aParticipantPoints: number;
    aParticipantAdvantages: number;
    aParticipantPenalties: number;
    bParticipantPoints: number;
    bParticipantAdvantages: number;
    bParticipantPenalties: number;
    submissionType : number;
    roundResultType : number;
    completeTime: number;
}