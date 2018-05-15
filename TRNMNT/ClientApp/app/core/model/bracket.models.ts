import {RoundModel} from './round.models';
import {MedalistModel} from './medalist.model';

export class BracketModel {
    bracketId : AAGUID;
    title : string;
    roundModels: RoundModel[];
    medalists: MedalistModel[];
}

export class BracketArrayModel {
    [key: string]: BracketModel;
}

export class RefreshBracketModel {
    bracket: BracketModel;
    weightDivisionId: string;
}

export class ChageWeightDivisionModel {
    synchronizationId: AAGUID;
    weightDivisionId: AAGUID;
}
