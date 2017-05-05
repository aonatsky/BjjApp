"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var core_1 = require("@angular/core");
var data_service_1 = require("../../core/dal/contracts/data.service");
var fighter_filter_component_1 = require("../../shared/fighter-filter/fighter-filter.component");
var FighterListComponent = (function () {
    function FighterListComponent(dataService) {
        this.dataService = dataService;
        this.fighterColumns = [
            { displayName: "First Name", propertyName: "firstName", isEditable: false },
            { displayName: "Last Name", propertyName: "lastName", isEditable: false },
            { displayName: "DOB", propertyName: "dateOfBirth", isEditable: false },
            { displayName: "Team", propertyName: "team", isEditable: false },
            { displayName: "Category", propertyName: "category", isEditable: false }
        ];
    }
    //ng
    FighterListComponent.prototype.ngOnInit = function () {
    };
    FighterListComponent.prototype.ngAfterContentInit = function () {
        // this.fighterFilter.currentFilterValue.
        this.refreshTable();
    };
    //events
    FighterListComponent.prototype.onFilterChanged = function () {
        this.refreshTable();
    };
    FighterListComponent.prototype.refreshTable = function () {
        var _this = this;
        this.dataService.getFigtersByFilter(this.fighterFilter.currentFilterValue).subscribe(function (data) { return _this.fighters = data; });
    };
    return FighterListComponent;
}());
__decorate([
    core_1.ViewChild(fighter_filter_component_1.FighterFilter),
    __metadata("design:type", fighter_filter_component_1.FighterFilter)
], FighterListComponent.prototype, "fighterFilter", void 0);
FighterListComponent = __decorate([
    core_1.Component({
        selector: 'fighterlist',
        templateUrl: './fighter-list.component.html',
        styleUrls: ['./fighter-list.component.css']
    }),
    __metadata("design:paramtypes", [data_service_1.DataService])
], FighterListComponent);
exports.FighterListComponent = FighterListComponent;
//# sourceMappingURL=fighter-list.component.js.map