import { ParticipantModelBase } from './participant.models';

export class MatchModel {
    matchId: AAGUID;
    weightDivisionId: string;
    categoryId: string;
    aParticipant: ParticipantModelBase;
    bParticipant: ParticipantModelBase;
    nextMatchId: AAGUID;
    round: number;
    order: number;
    matchType: number;
    matchTime: number;
    aParticipantResult: string;
    bParticipantResult: string;
    winnerParticipant: ParticipantModelBase;
}