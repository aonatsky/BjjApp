export class WeightDivision {
    weightClassId: number;
    name: string;
    weight: number;

    /**
     *
     */
    constructor(id:number, name: string, weight: number) { 
        this.weightClassId = id;
        this.name = name;
        this.weight = weight;
    }
}