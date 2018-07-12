import { NgModule } from '@angular/core'
import { RoundPanelComponent } from './round-panel/round-panel.component'
import { RoundPanelViewComponent } from './round-panel-view/round-panel-view.component'
import { CoreModule } from '../core/core.module'
import { CompleteRoundComponent } from './complete-round/complete-round.component';


@NgModule({

    imports: [
        CoreModule
    ],
    declarations: [
        CompleteRoundComponent,
        RoundPanelComponent,
        RoundPanelViewComponent
    ],

    providers: [],

    exports: [
        CompleteRoundComponent,
        RoundPanelComponent,
        RoundPanelViewComponent
    ]
})

export class RoundModule {
}