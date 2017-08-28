import { WeightDivisionModel } from "./weight-division.models"


export class CategoryModelBase {
    constructor() {

    }
    public name: string
    public categoryId?: AAGUID;
}


export class CategoryModel extends CategoryModelBase {

    constructor() {
        super();
        this.weightDivisions = []
    }

    public eventId: string;
    public weightDivisions: WeightDivisionModel[];
}


export class CategorySimpleModel extends CategoryModelBase {
    
}

