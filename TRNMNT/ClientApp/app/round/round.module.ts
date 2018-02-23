import { NgModule } from "@angular/core"
import { RoundPanelComponent } from './round-panel/round-panel.component'
import { CoreModule } from './../core/core.module'

@NgModule({

    imports: [
        CoreModule
    ],
    declarations: [
        RoundPanelComponent
    ],

    providers: [],

    exports: [
        RoundPanelComponent
    ]
})

export class RoundModule {
}