﻿export class UserModelBase {
  firstName: string;
  lastName: string;
  email: string;
  dateOfBirth: Date;
  userId: string;
  teamId: AAGUID;
}

export class UserModelRegistration extends UserModelBase {
  constructor() {
    super();
    this.isTeamOwner = false;
  }
  password: string;
  isTeamOwner: boolean;
}

export class UserModel extends UserModelRegistration {
  roles: string[];
}

export class UserModelAthlete extends UserModelBase {
  teamMembershipApprovalStatus: string;
}
