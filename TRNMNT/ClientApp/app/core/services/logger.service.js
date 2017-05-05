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
var app_config_1 = require("../../app.config");
var api_methods_consts_1 = require("../dal/consts/api-methods.consts");
var http_1 = require("@angular/http");
var LoggerService = (function () {
    function LoggerService(http) {
        this.http = http;
        this.logs = [];
        this.logLevel = Object.freeze({
            info: "INFO",
            warn: "WARN",
            error: "ERROR",
            debug: "DEBUG"
        });
    }
    LoggerService.prototype.logInfo = function (msg) {
        this.postLog(new LogModel(this.logLevel.info, msg));
        this.log("INFO: " + msg);
    };
    LoggerService.prototype.logDebug = function (msg) {
        this.postLog(new LogModel(this.logLevel.debug, msg));
        this.log("DEBUG: " + msg);
    };
    LoggerService.prototype.logError = function (msg) {
        this.postLog(new LogModel(this.logLevel.error, msg));
        this.log("ERROR: " + msg, true);
    };
    LoggerService.prototype.log = function (msg, isErr) {
        if (isErr === void 0) { isErr = false; }
        if (app_config_1.AppConfig.isDebug) {
            this.logs.push(msg);
            isErr ? console.error(msg) : console.log(msg);
        }
    };
    LoggerService.prototype.postLog = function (log) {
        var options = new http_1.RequestOptions({
            headers: new http_1.Headers({ 'Content-Type': 'application/json' })
        });
        var body = JSON.stringify(log);
        this.http.post(api_methods_consts_1.ApiMethods.log, body, options).subscribe();
    };
    return LoggerService;
}());
LoggerService = __decorate([
    core_1.Injectable(),
    __metadata("design:paramtypes", [http_1.Http])
], LoggerService);
exports.LoggerService = LoggerService;
var LogModel = (function () {
    function LogModel(Level, message) {
        this.Level = Level;
        this.message = message;
    }
    return LogModel;
}());
exports.LogModel = LogModel;
//# sourceMappingURL=logger.service.js.map