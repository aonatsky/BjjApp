import {RoundModel} from './round.models';

export class BracketModel {
    bracketId : AAGUID;
    roundModels: RoundModel[];
}

export class BracketArrayModel {
    [key: string]: BracketModel
}