import { CategoryModel } from "./category.model"


export class EventModel{
    constructor(
        
    ) { }

    public eventId: AAGUID;
    public title: string;
    public description: string;
    public address: string;
    public image: any;
    public date: Date;
    public registrationStartTS: Date;
    public registrationEndTS: Date;
    public statusId: number;
    public urlPrefix: string;
    public categories: CategoryModel[]

};