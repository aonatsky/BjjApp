export class Fighter {
    fighterID: AAGUID;
    firstName: string;
    lastName: string;
    team: string;
    weight: number;

    /**
     *
     */
    constructor(firstName:string, lastName: string, team:string, weight: number) { 
        this.firstName = firstName;
        this.lastName = lastName;
        this.team = team;
        this.weight = weight;
    }
}