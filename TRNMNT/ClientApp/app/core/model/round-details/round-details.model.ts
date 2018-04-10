import { RoundModel } from '../round.models';

export class RoundDetailsModel {
    roundId: string;
    firstPlayerPenalty: number;
    secondPlayerPenalty: number;
    firstPlayerAdvantage: number;
    secondPlayerAdvantage: number;
    firstPlayerPoints: number;
    secondPlayerPoints: number;

    countdown: number;
    isStarted: boolean;
    isPaused: boolean;
    isCompleted: boolean;

    roundModel: RoundModel;
}