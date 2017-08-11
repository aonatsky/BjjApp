export class WeightDivisionModel {
    weightDivisionId: AAGUID;
    name: string;
    weight: number;
    description: string;
    categoryId: string;

    /**
     *
     */
    constructor(name: string, id?: AAGUID ) { 
        this.weightDivisionId = id;
        this.name = name;
    }
}