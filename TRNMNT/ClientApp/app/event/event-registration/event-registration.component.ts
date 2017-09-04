import { Component, ViewEncapsulation, ViewChild, ElementRef, OnInit, OnChanges } from '@angular/core';
import { NgForm } from '@angular/forms'
import { ActivatedRoute } from '@angular/router';
import { TeamService } from './../../core/services/team.service';
import { ParticipantService } from './../../core/services/participant.service';
import { CategoryService } from './../../core/services/category.service';
import { PaymentService } from './../../core/services/payment.service';
import { WeightDivisionService } from './../../core/services/weight-division.service';
import { ParticipantRegistrationModel, ParticipantModelBase } from './../../core/model/participant.models';
import { ParticipantRegistrationResultModel } from './../../core/model/result/participant-registration-result.model';
import { TeamModel } from './../../core/model/team.model';
import { PaymentDataModel } from './../../core/model/payment-data.model';
import { CategorySimpleModel } from './../../core/model/category.models';
import { WeightDivisionModel, WeightDivisionSimpleModel } from './../../core/model/weight-division.models';
import { LoggerService } from './../../core/services/logger.service';
import { RouterService } from './../../core/services/router.service';
import { Observable } from "rxjs/Observable";
import { SelectItem, MenuModule, MenuItem, Message } from 'primeng/primeng'

@Component({
    selector: 'event-registration',
    templateUrl: './event-registration.component.html',
    styleUrls: ['./event-registration.component.css'],
    encapsulation: ViewEncapsulation.None
})

export class EventRegistrationComponent implements OnInit, OnChanges {

    private eventId: string;
    private currentStep: number = 0;
    private participant: ParticipantRegistrationModel = new ParticipantRegistrationModel();
    private categories: CategorySimpleModel[] = [];
    private weightDivisions: WeightDivisionSimpleModel[] = [];
    private categorySelectItems: SelectItem[];
    private weightDivisionsSelectItems: SelectItem[];
    private teamSelectItems: SelectItem[] = [];
    private teams: TeamModel[] = [];
    private tncAccepted: boolean = false;
    private paymentDataModel: PaymentDataModel;
    private messages: Message[] = []
    @ViewChild('formPrivat') formPrivat: ElementRef

    constructor(
        private routerService: RouterService,
        private loggerService: LoggerService,
        private route: ActivatedRoute,
        private weightDivisionService: WeightDivisionService,
        private categoryService: CategoryService,
        private teamService: TeamService,
        private participantService: ParticipantService,
        private paymentService: PaymentService

    ) {

    }

    ngOnInit() {
        this.route.params.subscribe(p => {
            this.eventId = p['id'];
            this.participant.eventId = this.eventId;
            this.loadData();
        });
    }

    ngOnChanges() {
        debugger;
        this.formPrivat.nativeElement.click();
    }

    private loadData() {
        Observable.forkJoin(this.teamService.getTeams(), this.categoryService.getCategoriesForEvent(this.eventId))
            .subscribe(data => this.initData(data));
    }

    private initData(data) {
        this.teams = data[0];
        this.categories = data[1];
        this.initCategoryDropdown();
        this.initTeamDropdown();

    }



    private getDefaultDateOfBirth() {
        let date = new Date();
        date.setFullYear(date.getFullYear() - 20);
        return date;
    }

    private initTeamDropdown()
    {
        this.teamSelectItems = [];
        for (var i = 0; i < this.teams.length; i++) {
            let team = this.teams[i];
            this.teamSelectItems.push({ label: team.name, value: team.teamId })
        }
    }

    private initCategoryDropdown() {
        this.categorySelectItems = [];
        for (var i = 0; i < this.categories.length; i++) {
            let category = this.categories[i];
            this.categorySelectItems.push({ label: category.name, value: category.categoryId })
        }
    }
    
    private initWeightDivisionDropdown(event) {
        this.weightDivisionService.getWeightDivisionsByCategory(event.value).subscribe(w => {
            this.weightDivisions = w;
            this.weightDivisionsSelectItems = [];
            for (var i = 0; i < this.weightDivisions.length; i++) {
                let weightDivision = this.weightDivisions[i];
                this.weightDivisionsSelectItems.push({ label: weightDivision.name, value: weightDivision.weightDivisionId })
            }
        })

    }

    private getTeam() {
        return this.teams.filter(t => t.teamId == this.participant.teamId)[0].name;
    }

    private getCategory() {
        return this.categories.filter(c => c.categoryId == this.participant.categoryId)[0].name;
    }

    private getWeightDivision() {
        return this.weightDivisions.filter(wd => wd.weightDivisionId == this.participant.weightDivisionId)[0].name;
    }

    private showMessage(message: string) {
        this.messages.push({ severity: 'error', summary: 'Error', detail: message });
    };



    private goToPayment() {
        // todo click payment form

        this.participantService.processParticipantRegistration(this.participant).subscribe((r: ParticipantRegistrationResultModel) => {
            if (!r.success) {
                this.showMessage(r.reason);
            } else {
                debugger;
                this.paymentDataModel = r.paymentData;
                this.formPrivat.nativeElement.submit();
                this.routerService.navigateByUrl("event/event-registration-complete/" + this.eventId);
            }
        });
        
    }

   

    private nextStep() {
        this.currentStep++;
    }

    private previousStep() {
        this.currentStep--;
    }

    
}

