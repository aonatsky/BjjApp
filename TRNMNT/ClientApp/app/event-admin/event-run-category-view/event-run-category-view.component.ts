import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BracketArrayModel, RefreshBracketModel} from '../../core/model/bracket.models';
import { BracketService } from '../../core/services/bracket.service';
import { WeightDivisionService } from '../../core/services/weight-division.service';
import { RunEventHubService } from '../../core/hubservices/run-event.hub.service';
import { WeightDivisionSimpleModel } from '../../core/model/weight-division.models';
import { animate, state, trigger, transition, style } from '@angular/animations';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs';


@Component({
    selector: 'event-run-category-view',
    templateUrl: './event-run-category-view.component.html',
    animations: [
        trigger('visibilityChanged', [
            state('true', style({ opacity: 1 })),
            state('false', style({ opacity: 0, display: 'none' })),
            transition('1 => 0', animate('0.05s')),
            transition('0 => 1', animate('0.5s'))
        ])
    ],
})
export class EventRunCategoryViewComponent implements OnInit, OnDestroy {

    private categoryId: string;
    private weightDivisions: LocalWeightDivisionModel[];
    private bracketsList: BracketArrayModel;
    private localSubscriptions: Subscription[];

    @Input() animationInterval: number = 10000;

    constructor(
        private route: ActivatedRoute,
        private bracketService: BracketService,
        private weightDivisionService: WeightDivisionService,
        private runEventHubService: RunEventHubService) {
        this.localSubscriptions = [];
    }

    ngOnInit() {
        this.localSubscriptions.push(this.runEventHubService.onRoundComplete().subscribe((model) => {
            console.log('RECIEVED', model);
            this.copyArrayWithSubstitution(model);
        }));
        this.route.params.subscribe(p => {
            this.categoryId = p['id'];

            this.localSubscriptions.push(this.weightDivisionService.getWeightDivisionsByCategory(this.categoryId)
                .subscribe((data) => {
                    this.weightDivisions = data.map((w, i) => new LocalWeightDivisionModel(w, i == 0));
                    this.startInfiniteAnimationLoop();
                }));

            this.localSubscriptions.push(this.bracketService.getBracketsByCategory(this.categoryId).subscribe((data) => {
                this.bracketsList = data;
                this.runEventHubService.joinMultipleWeightDivisionGroups(Object.keys(this.bracketsList));
                this.startInfiniteAnimationLoop();
            }));
        });
    }

    private copyArrayWithSubstitution(model: RefreshBracketModel) {
        let newObj = {};
        let bracketArrayModel = this.bracketsList;
        for (let weightDivisionId in bracketArrayModel) {
            if (bracketArrayModel.hasOwnProperty(weightDivisionId)) {

                if (weightDivisionId.toUpperCase() == model.weightDivisionId.toUpperCase()) {
                    newObj[model.weightDivisionId] = model.bracket;
                } else {
                    newObj[weightDivisionId] = this.bracketsList[weightDivisionId];
                }
            }
        }
        this.bracketsList = newObj;
    }

    private getDivisionNameById(id: string) : string {
        let model = this.getDivisionById(id);
        if (!!model && !!model.name) {
            return model.name;
        }
        return '';
    }

    private getDivisionById(id: string): LocalWeightDivisionModel {
        let model = this.weightDivisions.find((d) => d.weightDivisionId == id);
        if (!!model) {
            return model;
        }
        return <LocalWeightDivisionModel>{};
    }

    private startInfiniteAnimationLoop() {
        if (this.bracketsList != null && this.weightDivisions != null) {
            this.localSubscriptions.push(Observable.of(null)
                .concatMap(() => Observable.timer(this.animationInterval))
                .do(() => this.runAnimation())
                .repeat()
                .subscribe());
        }
    }

    private runAnimation() {
        let length = this.weightDivisions.length;
        let index = this.weightDivisions.findIndex(d => d.isVisible);
        if (index < 0) {
            console.error('invalid index no visible elements');
        }
        let nextIndex = index < length - 1 ? index + 1 : 0;

        this.weightDivisions.map(d => d.isVisible = false);
        this.weightDivisions[nextIndex].isVisible = true;
    }

    ngOnDestroy(): void {
        this.localSubscriptions.map(s => s.unsubscribe());
    }
}

class LocalWeightDivisionModel extends WeightDivisionSimpleModel {
    isVisible: boolean;

    constructor(model: WeightDivisionSimpleModel, isVisible: boolean) {
        super();
        this.name = model.name;
        this.weightDivisionId = model.weightDivisionId;
        this.isVisible = isVisible;
    }
}
