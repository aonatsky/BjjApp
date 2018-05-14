import { ParticipantModelBase } from './participant.models';

export class RoundModel {
    roundId: AAGUID;
    weightDivisionId: string;
    firstParticipant: ParticipantModelBase;
    secondParticipant: ParticipantModelBase;
    nextRoundId: AAGUID;
    stage: number;
    order: number;
    roundType: number;
    roundTime: number;
    firstParticipantResult: string;
    secondParticipantResult: string;
    winnerParticipant: ParticipantModelBase;
}