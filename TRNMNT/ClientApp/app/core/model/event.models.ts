import { CategoryModel } from "./category.models"


export class EventModelBase {
    constructor() {
    }
    public eventId: AAGUID;
    public title: string;
    public eventDate: Date;
    public registrationStartTS: Date;
    public earlyRegistrationEndTS: Date;
    public registrationEndTS: Date;

};

export class EventModel extends EventModelBase {
    constructor() {
        super();
        this.categoryModels = [];
    }

    public description: string;
    public address: string;
    public imagePath: any;
    public urlPrefix: string;
    public descritpion : string;
    public tncFilePath: string;
    public cardNumber: string;
    public contactEmail : string;
    public contactPhone : string;
    public fbLink : string;
    public vkLink : string;
    public additionalData: string;
    public promoCodeEnabled: boolean;
    public promoCodeListPath: string
    public earlyRegistrationPrice: number;
    public lateRegistrationPrice: number;
    public earlyRegistrationPriceForMembers: number;
    public lateRegistrationPriceForMembers: number;

    public categoryModels: CategoryModel[]

};

export class EventPreviewModel extends EventModelBase {
    constructor() {
        super();
    }
};



