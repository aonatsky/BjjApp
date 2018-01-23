export class ParticipantModelBase {
    public participantId: string;
    public firstName: string;
    public lastName: string;
    public dateOfBirth: Date;
    constructor() {
    }
}

export class ParticipantRegistrationModel extends ParticipantModelBase {
    public teamId: string;
    public categoryId: string;
    public weightDivisionId: string;
    public userId: string;
    public email: string;
    public phoneNumber: string;
    public promoCode: string;
}

export class ParticipantTableModel extends ParticipantModelBase {
    public participantId: string;
    public teamName: string;
    public categoryName: string;
    public weightDivisionName: string;
    public teamId: string;
    public categoryId: string;
    public weightDivisionId: string;
    public userId: string;
    public isMember: boolean;
}

