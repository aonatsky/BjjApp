﻿export class UserModel {
  constructor(
    public userId: AAGUID,
    public firstName: string,
    public lastName: string,
    public email: string,
    public role: string,
    public dateOfBirth: Date
  ) {}
}

export class UserRegistrationModel {
  firstName: string;
  lastName: string;
  password: string;
  email: string;
  dateOfBirth: Date;
}