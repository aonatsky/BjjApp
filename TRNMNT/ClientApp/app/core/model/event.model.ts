import { CategoryModel } from "./category.model"


export class EventModel{
    constructor() {
        this.categories = [];
    }

    public eventId: AAGUID;
    public title: string;
    public description: string;
    public address: string;
    public image: any;
    public eventDate: Date;
    public registrationStartTS: Date;
    public registrationEndTS: Date;
    public statusId: number;
    public urlPrefix: string;
    public descritpion : string;
    public tncFilePath: string;
    public cardNumber: string;
    public contactEmail : string;
    public contactPhone : string;
    public fbLink : string;
    public vkLink : string;
    public additionalData: string;
    public categories: CategoryModel[]

};