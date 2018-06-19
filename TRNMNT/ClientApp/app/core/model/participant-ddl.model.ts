import { TeamModelBase } from './team.model';
import { CategoryModelBase } from './category.models';
import { WeightDivisionModel } from './weight-division.models';

export class ParticipantDdlModel {
    categories: CategoryModelBase[];
    weightDivisions: WeightDivisionModel[];
    teams: TeamModelBase[];

    constructor() {
        this.categories = [];
        this.weightDivisions = [];
        this.teams = [];
    }
}