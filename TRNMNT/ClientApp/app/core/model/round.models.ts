import {ParticipantModelBase} from './participant.models';

export class RoundModel {
    roundId: AAGUID;
    firstParticipant: ParticipantModelBase;
    secondParticipant: ParticipantModelBase;
    nextRoundId: AAGUID;
    stage: number;
    roundType : number;
}