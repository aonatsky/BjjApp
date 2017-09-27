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
        this.weightDivisionModels = []
    }

    public eventId: string;
    public weightDivisionModels: WeightDivisionModel[];
}


export class CategorySimpleModel extends CategoryModelBase {
    
}

