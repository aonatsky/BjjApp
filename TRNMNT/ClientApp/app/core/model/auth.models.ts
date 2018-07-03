export class AuthTokenModel {
  idToken: string;
  refreshToken: string;
}

export class RefreshTokenModel {
  refreshToken: string;
}

export class SocialLoginResultModel {
  authTokenModel: AuthTokenModel;
  isExistingUser: boolean;
  userData

}
