// ====================================================
// More Templates: https://www.ebenmonney.com/templates
// Email: support@ebenmonney.com
// ====================================================

import { Permission } from './permission.model';


export class Role {

    constructor(name?: string, description?: string, permissions?: Permission[]) {

        this.Name = name;
        this.description = description;
        this.permissions = permissions;
    }

    public Id: number;
    public Name: string;
    public description: string;
    public usersCount: string;
    public permissions: Permission[];
    public checked: boolean;
}
