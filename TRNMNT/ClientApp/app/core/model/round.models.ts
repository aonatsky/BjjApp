import { ParticipantModelBase } from './participant.models';

export class RoundModel {
    roundId: AAGUID;
    weightDivisionId: string;
    AParticipant: ParticipantModelBase;
    BParticipant: ParticipantModelBase;
    nextRoundId: AAGUID;
    stage: number;
    order: number;
    roundType: number;
    roundTime: number;
    AParticipantResult: string;
    BParticipantResult: string;
    winnerParticipant: ParticipantModelBase;
}