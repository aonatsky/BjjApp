import {Category} from './category.model';
import {WeightDivision} from './weight-division.model';

export class FighterFilterModel {
    constructor(public weightDivisions: WeightDivision[], public categories:Category[]) {
    }
}