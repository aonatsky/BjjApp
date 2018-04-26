
export class TeamModelBase {
    teamId: AAGUID;
    name: string;
};

export class TeamModel extends TeamModelBase {
    constructor() {
        super();
    }

    description: string;
};
