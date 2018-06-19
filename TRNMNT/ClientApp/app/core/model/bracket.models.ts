import { MedalistModel } from './medalist.model';
import { MatchModel } from './match.models';

export class BracketModel {
    bracketId: AAGUID;
    title: string;
    matchModels: MatchModel[];
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
