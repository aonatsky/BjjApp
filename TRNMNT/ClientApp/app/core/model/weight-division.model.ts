export class WeightDivision {
    weightDivisionId: AAGUID;
    name: string;
    weight: number;
    description: string;

    /**
     *
     */
    constructor(id:AAGUID, name: string, weight: number) { 
        this.weightDivisionId = id;
        this.name = name;
        this.weight = weight;
    }
}