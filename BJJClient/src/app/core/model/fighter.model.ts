export class Fighter {
    fighterID: AAGUID;
    firstName: string;
    lastName: string;
    team: string;
    weight: number;
    age:number;
    belt: string;


        constructor(firstName:string, lastName: string, team:string, weight: number, age:number, belt:string) { 
        this.firstName = firstName;
        this.lastName = lastName;
        this.team = team;
        this.weight = weight;
        this.age = age;
        this.belt = belt;
    }
}