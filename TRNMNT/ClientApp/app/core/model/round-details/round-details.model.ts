import { RoundModel } from '../round.models';

export class RoundDetailsModel {
    roundId: string;
    AParticipantPenalties: number;
    BParticipantPenalties: number;
    AParticipantAdvantages: number;
    BParticipantAdvantages: number;
    AParticipantPoints: number;
    BParticipantPoints: number;

    countdown: number;
    isStarted: boolean;
    isPaused: boolean;
    isCompleted: boolean;

    roundModel: RoundModel;
}