import * as Interfaces from "src/app/interfaces/users/user.interface";

export class User implements Interfaces.User {

    public constructor(
        public id: string,
        public username: string,
        public firstName: string,
        public lastName: string,
        public gender: number,
        public birthdate: string,
        public nationalId: string,
        public registerDateTime: string,
        public isActive: boolean,
        public bio: string,
        public certifications: string,
        public email: string,
        public equipment: string,
        public language: string,
        public phone: string,
        public socialMediaLinks: string
    ) { }
}