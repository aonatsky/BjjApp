import { CategoryModelBase } from "./category.models";
import { WeightDivisionModel } from "./weight-division.models";
import { TeamModelBase } from "./team.model";

export class ParticipantModelBase {
    public firstName: string;
    public lastName: string;
    public dateOfBirth: Date;
    constructor() {
    }
}

export class ParticipantRegistrationModel extends ParticipantModelBase {
    public teamId: string;
    public categoryId: string;
    public weightDivisionId: string;
    public userId: string;
    public email: string;
    public phoneNumber: string;
    public promoCode: string;
}

export class ParticipantTableModel extends ParticipantModelBase {
    public teamName: string;
    public categoryName: string;
    public weightDivisionName: string;
    public teamId: string;
    public categoryId: string;
    public weightDivisionId: string;
    public userId: string;
    public isMember: boolean;
}

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

