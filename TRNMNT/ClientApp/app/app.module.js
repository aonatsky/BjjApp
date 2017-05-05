"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var core_1 = require("@angular/core");
var http_1 = require("@angular/http");
var forms_1 = require("@angular/forms");
var platform_browser_1 = require("@angular/platform-browser");
var angular2_universal_1 = require("angular2-universal");
var app_component_1 = require("./components/app/app.component");
var app_routing_1 = require("./app.routing");
//Administration components
var navmenu_component_1 = require("./administration/navmenu/navmenu.component");
var home_component_1 = require("./administration/home/home.component");
var listupload_component_1 = require("./administration/listupload/listupload.component");
var fighter_list_component_1 = require("./administration/fighter-list/fighter-list.component");
var tournament_settings_component_1 = require("./administration/tournament-settings/tournament-settings.component");
//Shared
var file_upload_component_1 = require("./shared/file-upload/file-upload.component");
var dropdown_component_1 = require("./shared/dropdown/dropdown.component");
var fighter_filter_component_1 = require("./shared/fighter-filter/fighter-filter.component");
var datatable_1 = require("primeng/components/datatable/datatable");
var dialog_1 = require("primeng/components/dialog/dialog");
var shared_1 = require("primeng/components/common/shared");
var button_1 = require("primeng/components/button/button");
var inputtext_1 = require("primeng/components/inputtext/inputtext");
var crud_component_1 = require("./shared/crud/crud.component");
var api_providers_1 = require("./core/dal/api.providers");
var server_settings_service_1 = require("./core/dal/server.settings.service");
var logger_service_1 = require("./core/services/logger.service");
var AppModule = (function () {
    function AppModule() {
    }
    return AppModule;
}());
AppModule = __decorate([
    core_1.NgModule({
        bootstrap: [app_component_1.AppComponent],
        declarations: [
            app_component_1.AppComponent,
            navmenu_component_1.NavMenuComponent,
            home_component_1.HomeComponent,
            listupload_component_1.ListUploadComponent,
            dropdown_component_1.DropdownComponent,
            fighter_list_component_1.FighterListComponent,
            fighter_filter_component_1.FighterFilter,
            file_upload_component_1.FileUpload,
            tournament_settings_component_1.TournamentSettingsComponent,
            crud_component_1.CrudComponent
        ],
        imports: [
            app_routing_1.routing,
            angular2_universal_1.UniversalModule,
            shared_1.SharedModule,
            http_1.HttpModule,
            platform_browser_1.BrowserModule,
            forms_1.FormsModule,
            datatable_1.DataTableModule,
            dialog_1.DialogModule,
            inputtext_1.InputTextModule,
            button_1.ButtonModule
        ],
        providers: [
            app_routing_1.appRoutingProviders,
            api_providers_1.ApiProviders,
            logger_service_1.LoggerService,
            server_settings_service_1.ServerSettingsService
        ]
    })
], AppModule);
exports.AppModule = AppModule;
//# sourceMappingURL=app.module.js.map