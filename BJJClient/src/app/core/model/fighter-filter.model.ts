import {AgeDivision} from './age-division.model';
import {BeltDivision} from './belt-division.model';
import {WeightDivision} from './weight-division.model';

export class FighterFilterModel {
    constructor(public weightDivisions: WeightDivision[], public beltDivisions:BeltDivision[], public ageDivisions:AgeDivision[]) {
    }
}