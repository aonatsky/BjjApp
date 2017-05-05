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
var logger_service_1 = require("../../../core/services/logger.service");
var api_methods_consts_1 = require("../consts/api-methods.consts");
var api_server_1 = require("../servers/api.server");
var DataApiService = (function (_super) {
    __extends(DataApiService, _super);
    function DataApiService(apiServer, logger) {
        var _this = _super.call(this) || this;
        _this.apiServer = apiServer;
        _this.logger = logger;
        return _this;
    }
    //Common
    DataApiService.prototype.getArray = function (response) {
        var result = response.json();
        if (result.length == 0) {
            return [];
        }
        return result;
    };
    DataApiService.prototype.handleErrorResponse = function (response) {
        var data = response;
        return Rx_1.Observable.throw(data);
    };
    //Categories
    DataApiService.prototype.getCategories = function () {
        var _this = this;
        return this.apiServer.get(api_methods_consts_1.ApiMethods.tournament.categories).map(function (r) { return _this.getArray(r); });
    };
    DataApiService.prototype.addCategory = function (category) {
        return this.apiServer.post(api_methods_consts_1.ApiMethods.tournament.categories, category);
    };
    DataApiService.prototype.updateCategory = function (category) {
        return this.apiServer.put(api_methods_consts_1.ApiMethods.tournament.categories, category);
    };
    DataApiService.prototype.deleteCategory = function (category) {
        return this.apiServer.delete(api_methods_consts_1.ApiMethods.tournament.categories, category);
    };
    //Fighters
    DataApiService.prototype.uploadFighterList = function (file) {
        return this.apiServer.postFile(api_methods_consts_1.ApiMethods.tournament.fighters.uploadlist, file);
    };
    DataApiService.prototype.getFigters = function (filter) {
        var _this = this;
        return this.apiServer.get(api_methods_consts_1.ApiMethods.tournament.fighters.getAll).map(function (r) { return _this.getArray(r); });
    };
    DataApiService.prototype.getFigtersByFilter = function (filter) {
        var _this = this;
        return this.apiServer.post(api_methods_consts_1.ApiMethods.tournament.fighters.getByFilter, filter).map(function (r) { return _this.getArray(r); });
    };
    //WeightDivisions
    DataApiService.prototype.getWeightDivisions = function () {
        var _this = this;
        return this.apiServer.get(api_methods_consts_1.ApiMethods.tournament.weightDivisions).map(function (r) { return _this.getArray(r); });
    };
    DataApiService.prototype.addWeightDivision = function (weightDivision) {
        return this.apiServer.post(api_methods_consts_1.ApiMethods.tournament.weightDivisions, weightDivision);
    };
    DataApiService.prototype.updateWeightDivision = function (weightDivision) {
        return this.apiServer.put(api_methods_consts_1.ApiMethods.tournament.weightDivisions, weightDivision);
    };
    DataApiService.prototype.deleteWeightDivision = function (weightDivision) {
        return this.apiServer.delete(api_methods_consts_1.ApiMethods.tournament.weightDivisions, weightDivision);
    };
    return DataApiService;
}(data_service_1.DataService));
DataApiService = __decorate([
    core_1.Injectable(),
    __metadata("design:paramtypes", [api_server_1.ApiServer, logger_service_1.LoggerService])
], DataApiService);
exports.DataApiService = DataApiService;
//# sourceMappingURL=data-api.service.js.map