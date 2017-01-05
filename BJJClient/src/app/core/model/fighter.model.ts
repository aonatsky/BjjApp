export class Fighter {
    FighterID: AAGUID;
    FirstName: string;
    LastName: string;
    Team: string;
    Weight: number;

    /**
     *
     */
    constructor(firstName:string, lastName: string, team:string, weight: number) { 
        this.FirstName = firstName;
        this.LastName = lastName;
        this.Team = team;
        this.Weight = weight;
    }
}