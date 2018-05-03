import {RoundModel} from './round.models';

export class BracketModel {
    bracketId : AAGUID;
    title : string;
    roundModels: RoundModel[];
}

export class BracketArrayModel {
    [key: string]: BracketModel;
}

export class RefreshBracketModel {
    bracket: BracketModel;
    weightDivisionId: string;
}

