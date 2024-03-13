export interface User {
    id: string,
    firstName: string,
    lastName: string,
    email: string,
    country: string,
    city: string,
    address: string,
    isActive: boolean,
    permissions: Array<string>
}