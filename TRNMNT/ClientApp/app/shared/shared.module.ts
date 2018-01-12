import { NgModule } from "@angular/core"
import { CoreModule } from "./../core/core.module"

import { LoginComponent } from './login/login.component'
import { RegisterComponent } from './register/register.component'
import { CategoryWithDivisionFilter } from "./category-with-division-filter/category-with-division-filter.component";
import { CrudComponent } from "./crud/crud.component";
import { FileUploadComponent } from "./file-upload/file-upload.component";



@NgModule({
    imports: [
        CoreModule,
    ],
    declarations: [
        LoginComponent,
        RegisterComponent,
        CrudComponent,
        FileUploadComponent,
        CategoryWithDivisionFilter
    ],

    providers: [],

    exports: [
        CrudComponent,
        FileUploadComponent,
        CategoryWithDivisionFilter
    ]
})

export class SharedModule {
}
