import { Category } from "./category.model"


export class EventModel{
    constructor(
        
    ) { }

    public eventId: AAGUID;
    public title: string;
    public description: string;
    public address: string;
    public image: any;
    public date: Date;
    public registrationStartDate: Date;
    public registrationEndDate: Date;
    public categories: Category[]

};