import { NgModule } from "@angular/core"
import { RoundPanelComponent } from './round-panel/round-panel.component'
import { RoundPanelViewComponent } from './round-panel-view/round-panel-view.component'
import { CoreModule } from './../core/core.module'

@NgModule({

    imports: [
        CoreModule
    ],
    declarations: [
        RoundPanelComponent,
        RoundPanelViewComponent
    ],

    providers: [],

    exports: [
        RoundPanelComponent,
        RoundPanelViewComponent
    ]
})

export class RoundModule {
}