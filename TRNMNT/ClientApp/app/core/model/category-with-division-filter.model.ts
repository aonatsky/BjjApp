
export class CategoryWithDivisionFilterModel {
    constructor(public weightDivisionId: string, public categoryId: string, public isMembersOnly: boolean = false) {
    }
}