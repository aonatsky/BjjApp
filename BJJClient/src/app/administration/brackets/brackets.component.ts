import { Component, OnInit, ViewChild,ViewEncapsulation } from '@angular/core';
import { DataService } from '../../core/dal/contracts/data.service'
import { Fight } from '../../core/model/fight.model'
import { Fighter } from '../../core/model/fighter.model'




@Component({
    selector: 'brackets',
    templateUrl: './brackets.component.html',
    styleUrls: ['./brackets.component.css'],
    encapsulation: ViewEncapsulation.None

})


export class BracketsComponent implements OnInit {

    fightersCount: number = 8;
    rowsCount: number[];
    columnsCount: number[];
    fighters: Fighter[]

    tableArray: string[][];

    fights: Fight[];
    private dataService: DataService;


    constructor(_dataService: DataService) {
        this.dataService = _dataService;
    }

    //ng
    ngOnInit() {

        this.dataService.getFigters(null).subscribe(data => this.fighters = data);
        this.rowsCount = this.getNumbersArray(this.getRowsCount());
        this.columnsCount = this.getNumbersArray(this.getColumnsCount());
        this.tableArray = this.populateTable(this.getTableArray(11, 7))

    }


    getRowsCount(): number {
        return this.fightersCount / 2 * 2;

    }
    getColumnsCount(): number {
        return (Math.log2(this.fightersCount / 2) * 4) + 3;


    }

    getNumbersArray(count: number): number[] {
        return Array(count).fill(0).map((x, i) => i); // [0,1,2,3,4]
    }

    getTableArray(width: number, height: number): string[][] {
        let tableArray = [];
        for (var i = 0; i < width; i++) {
            tableArray[i] = [];
            for (var j = 0; j < height; j++) {
                tableArray[i][j] = "";
            }
        }
        return tableArray;

    }

    populateTable(table: string[][]): string[][] {
        let result = table;
        let width = 11;
        let height = 7;
        let halfWidth = 11 - 1;
        for (var x = 0; x < width; x = x + 2) {
            let fightsCount = x == 0? 4 : 4/x;
            //result[x] = this.getRound(x,fightsCount,result[x])
            let ystart = x - 1 >= 0 ? x - 1 : 0;
            let yend = height - x;
            for (var y = ystart; y < yend; y = y + 2) {
                if (y%2 == 0) {
                    result[x][y] = `<div>FIGHTER` + x.toString() + y.toString() + `</div>`    
                }else{
                    result[x][yend - y] = `<div>FIGHTER` + x.toString() + y.toString() + `</div>`    
                }
                
            }

        }

        return result;

    }

    getRound(x: number, fightsCount: number, emptyRound: string[]) {
        var result = emptyRound;
        for (var i = 0; i < fightsCount; i++) {
            let y = 0;
            if (i % 2 == 0) {
                y = result.length - 1 - x * i;
            } else {
                y = x * i;
            }
            result[y] = `<div>FIGHTER` + x.toString() + y.toString() + `</div>`

        }
        return result;


    }

    
    divFighter = `<div class="input-group">
      <div class="form-control">Team1</div><span class="input-group-addon"><span class="badge pull-right"></span></span>
    </div>`;

      divleftBottomCorner = 
      `<table class='bracket-separator'><tr><td></td><td></td></td></tr><tr><td></td><td></td></td></tr></table>`;

}

