import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TeamService } from './../../core/services/team.service';
import { CategoryService } from './../../core/services/category.service';
import { WeightDivisionService } from './../../core/services/weight-division.service';
import { ParticipantModel } from './../../core/model/participant.model';
import { TeamModel } from './../../core/model/team.model';
import { CategoryModel } from './../../core/model/category.model';
import { WeightDivisionModel } from './../../core/model/weight-division.model';
import { LoggerService } from './../../core/services/logger.service';
import { RouterService } from './../../core/services/router.service';
import { Observable } from "rxjs/Observable";
import { SelectItem } from 'primeng/primeng'

@Component({
    selector: 'participate',
    templateUrl: './participate.component.html',
    styleUrls: ['./participate.component.css']
})

export class ParticipateComponent {

    private eventId: string;
    private participant: ParticipantModel = new ParticipantModel();
    private categories: CategoryModel[] = [];
    private weightDivisions: WeightDivisionModel[] = [];
    private categorySelectItems: SelectItem[];
    private weightDivisionsSelectItems: SelectItem[];
    private existingTeams: TeamModel[] = [];
    private teamSuggestions: TeamModel[] = [];
    private tncAccepted: boolean = false;

    constructor(
        private routerService: RouterService,
        private loggerService: LoggerService,
        private route: ActivatedRoute,
        private weightDivisionService: WeightDivisionService,
        private categoryService: CategoryService,
        private teamService: TeamService,

    ) {

    }

    ngOnInit() {
        this.route.params.subscribe(p => {
            this.eventId = p['id'];
            this.participant.eventId = this.eventId;
        });
        this.loadData();
    }


    private loadData() {
        Observable.forkJoin(this.teamService.getTeams(), this.categoryService.getCategories(this.eventId))
            .subscribe(data => this.initData(data));
    }

    private initData(data) {
        this.existingTeams = data[0];
        this.categories = data[1];
        this.initCategoryDropdown();
    }


    private teamSearch(event) {
        this.teamSuggestions = this.existingTeams.filter((team: TeamModel) => team.name.toLowerCase().startsWith(event.query.trim().toLowerCase()))
    };

    private getDefaultDateOfBirth() {
        let date = new Date();
        date.setFullYear(date.getFullYear() - 20);
        return date;
    }

    private initCategoryDropdown() {
        this.categorySelectItems = [];
        for (var i = 0; i < this.categories.length; i++) {
            let category = this.categories[i];
            this.categorySelectItems.push({ label: category.name, value: category.categoryId })
        }
    }
    
    private initWeightDivisionDropdown(event) {
        this.weightDivisionService.getWeightDivisions(event.value).subscribe(w => {
            this.weightDivisions = w;
            this.weightDivisionsSelectItems = [];
            for (var i = 0; i < this.weightDivisions.length; i++) {
                let weightDivision = this.weightDivisions[i];
                this.weightDivisionsSelectItems.push({ label: weightDivision.name, value: weightDivision.weightDivisionId })
            }
        })

    }

    private createParticipant() {
        
    }

}

