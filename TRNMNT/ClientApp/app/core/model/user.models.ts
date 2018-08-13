export class UserModelBase {
  firstName: string;
  lastName: string;
  email: string;
  dateOfBirth: Date;
  userId: AAGUID;
}

export class UserModelRegistration extends UserModelBase {
  password: string;
  isTeamOwner: boolean;
}

export class UserModel extends UserModelRegistration {
  role: string;
}

export class UserModelAthlete extends UserModelBase {
  teamMembershipApprovalStatus: string;
}
