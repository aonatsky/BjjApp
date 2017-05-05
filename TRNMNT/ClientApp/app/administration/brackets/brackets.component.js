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
var BracketsComponent = (function () {
    function BracketsComponent(_dataService) {
        this.fightersCount = 8;
        this.divFighter = "<div class=\"input-group fighter\">\n      <div class=\"form-control\"></div></div>";
        this.divleftToBottomSeparator = "<table class='bracket-separator left-to-bottom'><tr><td></td><td></td></td></tr><tr><td></td><td></td></td></tr></table>";
        this.divleftToTopSeparator = "<table class='bracket-separator left-to-top'><tr><td></td><td></td></td></tr><tr><td></td><td></td></td></tr></table>";
        this.divCenterToRightSeparator = "<table class='bracket-separator center-to-right'><tr><td></td><td></td></td></tr><tr><td></td><td></td></td></tr></table>";
        this.divCenterSeparator = "<table class='bracket-separator center'><tr><td></td><td></td></td></tr><tr><td></td><td></td></td></tr></table>";
        this.divBottomToRightSeparator = "<table class='bracket-separator bottom-to-right'><tr><td></td><td></td></td></tr><tr><td></td><td></td></td></tr></table>";
        this.divTopToRightSeparator = "<table class='bracket-separator top-to-right'><tr><td></td><td></td></td></tr><tr><td></td><td></td></td></tr></table>";
        this.divLeftToCenterSeparator = "<table class='bracket-separator left-to-center'><tr><td></td><td></td></td></tr><tr><td></td><td></td></td></tr></table>";
        this.dataService = _dataService;
    }
    //ng
    BracketsComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.dataService.getFigters(null).subscribe(function (data) { return _this.fighters = data; });
    };
    BracketsComponent.prototype.formatFighterName = function (fighter) {
        return fighter.firstName + " " + fighter.lastName;
    };
    BracketsComponent.prototype.getFighterDiv = function (fighter) {
        return "<div class=\"input-group fighter\">\n      <div class=\"form-control\">" + this.formatFighterName(fighter) + "</div></div>";
    };
    return BracketsComponent;
}());
BracketsComponent = __decorate([
    core_1.Component({
        selector: 'brackets',
        templateUrl: './brackets.component.html',
        styleUrls: ['./brackets.component.css'],
        encapsulation: core_1.ViewEncapsulation.None
    }),
    __metadata("design:paramtypes", [data_service_1.DataService])
], BracketsComponent);
exports.BracketsComponent = BracketsComponent;
//# sourceMappingURL=brackets.component.js.map