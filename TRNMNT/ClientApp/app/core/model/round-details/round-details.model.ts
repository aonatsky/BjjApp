import { RoundModel } from '../round.models';

export class RoundDetailsModel {
    roundId: string;
    AParticipantPenalties: number;
    secondParticipantPenalties: number;
    AParticipantAdvantages: number;
    secondParticipantAdvantages: number;
    AParticipantPoints: number;
    secondParticipantPoints: number;

    countdown: number;
    isStarted: boolean;
    isPaused: boolean;
    isCompleted: boolean;

    roundModel: RoundModel;
}