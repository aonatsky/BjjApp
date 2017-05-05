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
var fighter_filter_model_1 = require("../../core/model/fighter-filter.model");
var core_1 = require("@angular/core");
var dropdown_component_1 = require("../dropdown/dropdown.component");
var data_service_1 = require("../../core/dal/contracts/data.service");
var default_values_1 = require("../../core/consts/default-values");
var Observable_1 = require("rxjs/Observable");
var FighterFilter = (function () {
    function FighterFilter(dataService) {
        this.dataService = dataService;
        this.weightDivisionDDLOptions = [];
        this.categoryDDLOptions = [];
        this.onFilterChanged = new core_1.EventEmitter();
        this.onFilterLoaded = new core_1.EventEmitter();
    }
    FighterFilter.prototype.ngOnInit = function () {
        var _this = this;
        Observable_1.Observable.forkJoin(this.dataService.getCategories(), this.dataService.getWeightDivisions())
            .subscribe(function (data) { return _this.initFilter(data); });
    };
    //Events
    FighterFilter.prototype.weightSelect = function (value) {
        if (value.name == default_values_1.DefaultValues.DROPDOWN_NAME_ANY) {
            this.currentFilterValue.weightDivisions = this.weightDivisions;
        }
        else {
            this.currentFilterValue.weightDivisions = this.weightDivisions.filter(function (wd) { return wd.weightDivisionId == value.id; });
        }
        this.onFilterChanged.emit(this.currentFilterValue);
    };
    FighterFilter.prototype.categorySelect = function (value) {
        if (value.name == default_values_1.DefaultValues.DROPDOWN_NAME_ANY) {
            this.currentFilterValue.categories = this.categories;
        }
        else {
            this.currentFilterValue.categories = this.categories.filter(function (c) { return c.categoryId == value.id; });
        }
        this.onFilterChanged.emit(this.currentFilterValue);
    };
    //Private methods
    FighterFilter.prototype.initFilter = function (data) {
        var _this = this;
        var defaultDDLOption = new dropdown_component_1.DropDownListOption(default_values_1.DefaultValues.DROPDOWN_ID_ANY, default_values_1.DefaultValues.DROPDOWN_NAME_ANY);
        this.categories = data[0];
        this.weightDivisions = data[1];
        this.weightDivisionDDLOptions.push(defaultDDLOption);
        this.categoryDDLOptions.push(defaultDDLOption);
        this.categories.map(function (c) { return _this.categoryDDLOptions.push(new dropdown_component_1.DropDownListOption(c.categoryId, c.name)); });
        this.weightDivisions.map(function (wd) { return _this.weightDivisionDDLOptions.push(new dropdown_component_1.DropDownListOption(wd.weightDivisionId, wd.name)); });
        this.currentFilterValue = new fighter_filter_model_1.FighterFilterModel(this.weightDivisions, this.categories);
        this.onFilterChanged.emit(this.currentFilterValue);
    };
    return FighterFilter;
}());
__decorate([
    core_1.Output(),
    __metadata("design:type", core_1.EventEmitter)
], FighterFilter.prototype, "onFilterChanged", void 0);
__decorate([
    core_1.Output(),
    __metadata("design:type", core_1.EventEmitter)
], FighterFilter.prototype, "onFilterLoaded", void 0);
__decorate([
    core_1.Output(),
    __metadata("design:type", fighter_filter_model_1.FighterFilterModel)
], FighterFilter.prototype, "currentFilterValue", void 0);
FighterFilter = __decorate([
    core_1.Component({
        selector: 'fighter-filter',
        templateUrl: 'fighter-filter.component.html',
        styleUrls: ['fighter-filter.component.css']
    }),
    __metadata("design:paramtypes", [data_service_1.DataService])
], FighterFilter);
exports.FighterFilter = FighterFilter;
//# sourceMappingURL=fighter-filter.component.js.map