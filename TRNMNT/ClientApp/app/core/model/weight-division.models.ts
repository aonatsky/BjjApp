export class WeightDivisionModelBase {
    weightDivisionId: AAGUID;
    name: string;
}

export class WeightDivisionSimpleModel extends WeightDivisionModelBase { 
   
}

export class WeightDivisionModel extends WeightDivisionModelBase {
    weight: number;
    description: string;
    categoryId: string;
        
    constructor() {
        super();
    }
}