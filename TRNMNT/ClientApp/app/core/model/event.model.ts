
export class EventModel{
    constructor(
        
    ) { }

    public eventId: AAGUID;
    public title: string;
    public description: string;
    public address: string;
    public image: any;
    public eventDate: Date;
    public registrationStartTs: Date;
    public registrationEndTs: Date;

};