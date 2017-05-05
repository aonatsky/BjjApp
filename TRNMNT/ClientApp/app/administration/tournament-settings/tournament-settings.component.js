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
var data_service_1 = require("../../core/dal/contracts/data.service");
var core_1 = require("@angular/core");
var TournamentSettingsComponent = (function () {
    function TournamentSettingsComponent(dataService) {
        this.dataService = dataService;
        this.categoryColumns = [
            { displayName: "Id", propertyName: "categoryId", isEditable: false },
            { displayName: "Name", propertyName: "name", isEditable: true }
        ];
        this.weightDivisionColumns = [
            { displayName: "Id", propertyName: "weightDivisionId", isEditable: false },
            { displayName: "Name", propertyName: "name", isEditable: true }
        ];
    }
    TournamentSettingsComponent.prototype.ngOnInit = function () {
        this.refreshCategories();
        this.refreshWeightDivisions();
    };
    //Categories    
    TournamentSettingsComponent.prototype.refreshCategories = function () {
        var _this = this;
        this.dataService.getCategories().subscribe(function (data) { return _this.categories = data; });
    };
    TournamentSettingsComponent.prototype.addCategory = function (category) {
        var _this = this;
        this.dataService.addCategory(category).subscribe(function () { return _this.refreshCategories(); });
    };
    TournamentSettingsComponent.prototype.updateCategory = function (category) {
        var _this = this;
        this.dataService.updateCategory(category).subscribe(function () { return _this.refreshCategories(); });
    };
    TournamentSettingsComponent.prototype.deleteCategory = function (category) {
        var _this = this;
        this.dataService.deleteCategory(category).subscribe(function () { return _this.refreshCategories(); });
    };
    //WeightDivisions    
    TournamentSettingsComponent.prototype.refreshWeightDivisions = function () {
        var _this = this;
        this.dataService.getWeightDivisions().subscribe(function (data) { return _this.test(data); });
    };
    TournamentSettingsComponent.prototype.addWeightDivision = function (category) {
        var _this = this;
        this.dataService.addWeightDivision(category).subscribe(function () { return _this.refreshWeightDivisions(); });
    };
    TournamentSettingsComponent.prototype.updateWeightDivision = function (category) {
        var _this = this;
        this.dataService.updateWeightDivision(category).subscribe(function () { return _this.refreshWeightDivisions(); });
    };
    TournamentSettingsComponent.prototype.deleteWeightDivision = function (category) {
        var _this = this;
        this.dataService.deleteWeightDivision(category).subscribe(function () { return _this.refreshWeightDivisions(); });
    };
    TournamentSettingsComponent.prototype.test = function (data) {
        this.weightDivisions = data;
    };
    return TournamentSettingsComponent;
}());
TournamentSettingsComponent = __decorate([
    core_1.Component({
        selector: 'tournament-settings',
        templateUrl: './tournament-settings.component.html'
    }),
    __metadata("design:paramtypes", [data_service_1.DataService])
], TournamentSettingsComponent);
exports.TournamentSettingsComponent = TournamentSettingsComponent;
//# sourceMappingURL=tournament-settings.component.js.map