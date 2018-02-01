import { NgModule } from '@angular/core'
import { CoreModule } from './../core/core.module'
import { LoginComponent } from './login/login.component'
import { RegisterComponent } from './register/register.component'
import { CategoryWithDivisionFilter } from './category-with-division-filter/category-with-division-filter.component';
import { CrudComponent } from './crud/crud.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LoaderComponent } from './loader/loader';
import {BracketComponent} from './bracket/bracket.component';


@NgModule({
    imports: [
        CoreModule,
        BrowserAnimationsModule
    ],
    declarations: [
        LoginComponent,
        RegisterComponent,
        CrudComponent,
        CategoryWithDivisionFilter,
        LoaderComponent,
        BracketComponent
    ],

    providers: [],

    exports: [
        CrudComponent,
        CategoryWithDivisionFilter,
        LoaderComponent,
        BracketComponent
    ]
})

export class SharedModule {
}
