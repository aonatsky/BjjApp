export class ParticipantModel {

    public participantId: AAGUID;
    public firstName: string;
    public lastName: string;
    public team: string;
    public dateOfBirth: Date;
    public isApproved: boolean;
    public categoryId: string;
    public weightDivisionId: string;
    public eventId: AAGUID;
    public userId: string;
    constructor() { }

}