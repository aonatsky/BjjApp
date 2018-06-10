import { ParticipantModelBase } from './participant.models';

export class RoundModel {
    roundId: AAGUID;
    weightDivisionId: string;
    AParticipant: ParticipantModelBase;
    secondParticipant: ParticipantModelBase;
    nextRoundId: AAGUID;
    stage: number;
    order: number;
    roundType: number;
    roundTime: number;
    AParticipantResult: string;
    secondParticipantResult: string;
    winnerParticipant: ParticipantModelBase;
}