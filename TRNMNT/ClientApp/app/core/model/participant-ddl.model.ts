import { TeamModelBase } from "./team.model";
import { CategoryModelBase } from "./category.models";
import { WeightDivisionModel } from "./weight-division.models";

export class ParticipantDdlModel {
    public categories: CategoryModelBase[];
    public weightDivisions: WeightDivisionModel[];
    public teams: TeamModelBase[];

    constructor() {
        this.categories = [];
        this.weightDivisions = [];
        this.teams = [];
    }
}