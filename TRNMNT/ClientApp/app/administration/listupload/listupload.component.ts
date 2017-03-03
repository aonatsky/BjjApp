import { Component } from '@angular/core';
import { Http } from '@angular/http';

@Component({
    selector: 'listupload',
    templateUrl: './listupload.component.html'
})


export class ListUploadComponent {
    public weightDivisions: weightDivision[];
    public dropdownValues: DropdownValue[];

    
    constructor(){
       this.weightDivisions = [new weightDivision(1,"light")]
   }
}

class DropdownValue {
  value:string;
  label:string;
}


class weightDivision {
    id : string;
    name: string;

    constructor (id,name){
        this.id = id;
        this.name = name;
    }
}

interface WeatherForecast {
    dateFormatted: string;
    temperatureC: number;
    temperatureF: number;
    summary: string;
}
