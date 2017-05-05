"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
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
var Rx_1 = require("rxjs/Rx");
var data_service_1 = require("../contracts/data.service");
var fighter_model_1 = require("../../model/fighter.model");
var weight_division_model_1 = require("../../model/weight-division.model");
var category_model_1 = require("../../model/category.model");
var DataFakeService = (function (_super) {
    __extends(DataFakeService, _super);
    function DataFakeService() {
        var _this = _super.call(this) || this;
        _this.fighters = [
            new fighter_model_1.Fighter("4c571d9a-3398-4677-831d-3373d270ec11", "Marcelo", "Garcia", "Alliance", "1986-10-10", "Elite"),
        ];
        _this.weightDivisions = [
            new weight_division_model_1.WeightDivision("4c571d9a-3398-4677-831d-3373d270ec11", "Light", 60),
            new weight_division_model_1.WeightDivision("1e3602fc-687e-4ee7-a4a4-6202b3c0af54", "Middle", 80),
            new weight_division_model_1.WeightDivision("1e3602fc-687e-4ee7-a4a4-6202b3c0af54", "Heavy", 90)
        ];
        _this.categories = [
            new category_model_1.Category("4c571d9a-3398-4677-831d-3373d270ec12", "kids")
        ];
        return _this;
    }
    DataFakeService.prototype.getFigtersByFilter = function (filter) {
        throw new Error('Method not implemented.');
    };
    DataFakeService.prototype.addWeightDivision = function (weightDivision) {
        throw new Error('Method not implemented.');
    };
    DataFakeService.prototype.updateWeightDivision = function (weightDivision) {
        throw new Error('Method not implemented.');
    };
    DataFakeService.prototype.deleteWeightDivision = function (weightDivision) {
        throw new Error('Method not implemented.');
    };
    DataFakeService.prototype.addCategory = function (category) {
        throw new Error('Method not implemented.');
    };
    DataFakeService.prototype.updateCategory = function (category) {
        throw new Error('Method not implemented.');
    };
    DataFakeService.prototype.deleteCategory = function (category) {
        throw new Error('Method not implemented.');
    };
    DataFakeService.prototype.getCategories = function () {
        return Rx_1.Observable.of(this.categories);
    };
    DataFakeService.prototype.uploadFighterList = function (file) {
        return Rx_1.Observable.of(this.fighters);
    };
    DataFakeService.prototype.getFigters = function (filter) {
        return Rx_1.Observable.of(this.fighters);
    };
    DataFakeService.prototype.getWeightDivisions = function () {
        return Rx_1.Observable.of(this.weightDivisions);
    };
    return DataFakeService;
}(data_service_1.DataService));
DataFakeService = __decorate([
    core_1.Injectable(),
    __metadata("design:paramtypes", [])
], DataFakeService);
exports.DataFakeService = DataFakeService;
//# sourceMappingURL=data-fake.service.js.map