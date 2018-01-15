
export class TeamModelBase {
    public teamId: AAGUID;
    public name: string;
};

export class TeamModel extends TeamModelBase {
    constructor() {
        super();
    }

    public description: string;
};
