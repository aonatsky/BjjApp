import { RoundModel } from '../round.models';

export class RoundDetailsModel {
    roundId: string;
    firstParticipantPenalties: number;
    secondParticipantPenalties: number;
    firstParticipantAdvantages: number;
    secondParticipantAdvantages: number;
    firstParticipantPoints: number;
    secondParticipantPoints: number;

    countdown: number;
    isStarted: boolean;
    isPaused: boolean;
    isCompleted: boolean;

    roundModel: RoundModel;
}