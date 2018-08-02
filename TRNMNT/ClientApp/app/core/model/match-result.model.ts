export class MatchResultModel {
    matchId: AAGUID;
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