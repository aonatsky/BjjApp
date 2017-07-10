import {WeightDivisionModel} from "./weight-division.model"

export class CategoryModel {

    constructor() {
        this.weightDivisions = []
    }

    public name: string
    public categoryId? : AAGUID;
    public weightDivisions: WeightDivisionModel[];
}
