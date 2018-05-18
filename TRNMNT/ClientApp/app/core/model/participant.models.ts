﻿export class ParticipantModelBase {
    participantId: string;
    firstName: string;
    lastName: string;
    dateOfBirth: Date;
    teamName: string;
    constructor() {
    }
}

export class ParticipantRegistrationModel extends ParticipantModelBase {
    teamId: string;
    categoryId: string;
    weightDivisionId: string;
    userId: string;
    email: string;
    phoneNumber: string;
    promoCode: string;
}

export class ParticipantTableModel extends ParticipantModelBase {
    participantId: string;
    teamName: string;
    categoryName: string;
    weightDivisionName: string;
    teamId: string;
    categoryId: string;
    weightDivisionId: string;
    userId: string;
    isMember: boolean;
}

export class ParticipantInAbsoluteDivisionMobel {
    participantId: string;
    firstName: string;
    lastName: string;
    teamName: string;
    weightDivisionName: string;
    isSelectedIntoDivision: boolean;
}

