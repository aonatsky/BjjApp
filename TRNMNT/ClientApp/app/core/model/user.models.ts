export class UserModelBase {
  firstName: string;
  lastName: string;
  email: string;
  dateOfBirth: Date;
  userId: string;
  teamId: AAGUID;
}

export class UserModelRegistration extends UserModelBase {
  password: string;
  isTeamOwner: boolean;
  constructor() {
    super();
    this.isTeamOwner = false;
  }
}

export class UserModel extends UserModelRegistration {
  roles: string[];
  isParticipantForEvent: boolean;
}

export class UserModelAthlete extends UserModel {
  teamMembershipApprovalStatus: string;
  teamName: string;
  isFederationMember: boolean;
}
