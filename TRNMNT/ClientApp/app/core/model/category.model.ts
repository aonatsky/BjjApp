import {WeightDivision} from "./weight-division.model"

export class Category {

    constructor() {
    }

    public name: string
    public categoryId: AAGUID;
    public weightDivisions: WeightDivision[];
}
