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
    public Descritpion : string;
    public Address : string;
    public TNCFilePath: string;
    public CardNumber: string;
    public ContactEmail : string;
    public ContactPhone : string;
    public FBLink : string;
    public VKLink : string;
    public AdditionalData: string;
    public categories: CategoryModel[]

};