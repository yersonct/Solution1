export interface IPerson {
    id: number,
    name: string,
    lastName: string,
    document: string,
    phone: string,
    email: string,
    active: boolean
}

export interface IRegisterRequest {
    username: string;
    password: string;
    person: IPerson;
}
