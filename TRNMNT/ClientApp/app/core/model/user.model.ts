export class UserModel {

    constructor(
        public userId: AAGUID,
        public firstName: string,
        public lastName: string,
        public email: string,
        public role: string) {
    }
}