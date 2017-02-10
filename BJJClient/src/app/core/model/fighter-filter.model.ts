import {AgeDivision} from './age-division.model';
import {BeltDivision} from './belt-division.model';
import {WeightDivision} from './weight-division.model';

export class FighterFilterModel {
    constructor(public weightDivision: WeightDivision, public beltDivision:BeltDivision, public ageDivision:AgeDivision) {
    }
}