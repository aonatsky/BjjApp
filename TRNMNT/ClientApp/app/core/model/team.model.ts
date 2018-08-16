
export class TeamModelBase {
    teamId: AAGUID;
    name: string;
    federationApprovalStatus: string;
    approvalStatus: string;
};

export class TeamModel extends TeamModelBase {
    constructor() {
        super();
    }
    description: string;
};
export class TeamRegistrationModel extends TeamModel {
    constructor() {
        super();
    }
    contactName : string;
    contactPhone: string;
    contactEmail: string;
    returnUrl: string;
}

export class TeamModelFull extends TeamRegistrationModel {
    
}
