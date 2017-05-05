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
var http_1 = require("@angular/http");
var server_settings_service_1 = require("../server.settings.service");
var logger_service_1 = require("../../../core/services/logger.service");
var Observable_1 = require("rxjs/Observable");
var ApiServer = (function () {
    function ApiServer(serverSettings, loggerService, http) {
        this.serverSettings = serverSettings;
        this.loggerService = loggerService;
        this.http = http;
    }
    ApiServer.prototype.get = function (name) {
        var _this = this;
        return this.http.get(name).map(function (r) { return _this.processResponse(r); }).catch(function (error) { return _this.handleError(error); });
    };
    ApiServer.prototype.post = function (name, model) {
        var _this = this;
        var options = new http_1.RequestOptions({
            headers: new http_1.Headers({ 'Content-Type': 'application/json' })
        });
        var body = JSON.stringify(model);
        return this.http.post(name, body, options).map(function (r) { return _this.processResponse(r); }).catch(function (error) { return _this.handleError(error); });
    };
    ApiServer.prototype.put = function (name, model) {
        var _this = this;
        var options = new http_1.RequestOptions({
            headers: new http_1.Headers({ 'Content-Type': 'application/json' })
        });
        var body = JSON.stringify(model);
        return this.http.put(name, body, options).map(function (r) { return _this.processResponse(r); }).catch(function (error) { return _this.handleError(error); });
    };
    ApiServer.prototype.delete = function (name, model) {
        var _this = this;
        var options = new http_1.RequestOptions({
            headers: new http_1.Headers({ 'Content-Type': 'application/json' }),
            body: JSON.stringify(model)
        });
        return this.http.delete(name, options).map(function (r) { return _this.processResponse(r); }).catch(function (error) { return _this.handleError(error); });
    };
    ApiServer.prototype.postFile = function (name, file) {
        var _this = this;
        var formData = new FormData();
        formData.append("file", file);
        return this.http.post(name, formData).catch(function (error) { return _this.handleError(error); });
    };
    ApiServer.prototype.processResponse = function (response) {
        // add additional processing
        return response;
    };
    ApiServer.prototype.handleError = function (error) {
        // In a real world app, you might use a remote logging infrastructure
        var errMsg;
        if (error instanceof http_1.Response) {
            errMsg = error.status + " - " + (error.statusText || '') + " url: " + error.url;
        }
        else {
            errMsg = error.message ? error.message : error.toString();
        }
        console.error(errMsg);
        this.loggerService.logError(errMsg);
        return Observable_1.Observable.throw(errMsg);
    };
    return ApiServer;
}());
ApiServer = __decorate([
    core_1.Injectable(),
    __metadata("design:paramtypes", [server_settings_service_1.ServerSettingsService, logger_service_1.LoggerService, http_1.Http])
], ApiServer);
exports.ApiServer = ApiServer;
//# sourceMappingURL=api.server.js.map