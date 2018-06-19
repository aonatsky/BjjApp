import { MatchModel } from './match.models';

export class MatchDetailsModel {
    matchId: string;
    aParticipantPenalties: number;
    bParticipantPenalties: number;
    aParticipantAdvantages: number;
    bParticipantAdvantages: number;
    aParticipantPoints: number;
    bParticipantPoints: number;
    countdown: number;
    isStarted: boolean;
    isPaused: boolean;
    isCompleted: boolean;

    matchModel: MatchModel;
}