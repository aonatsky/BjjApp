export class ParticipantModelBase {
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
    public teamName: string;
    public categoryName: string;
    public weightDivisionName: string;
    public userId: string;
    public isMember: boolean;
}

