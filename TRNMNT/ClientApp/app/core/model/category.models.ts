import { WeightDivisionModel } from './weight-division.models'


export class CategoryModelBase {
    name: string;
    categoryId?: AAGUID;
}


export class CategoryModel extends CategoryModelBase {


    constructor() {
        super();
        this.weightDivisionModels = [];
    }
    eventId: string;
    roundTime: number;
    weightDivisionModels: WeightDivisionModel[];
}


export class CategorySimpleModel extends CategoryModelBase {

}

