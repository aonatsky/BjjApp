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
var CrudComponent = (function () {
    function CrudComponent() {
        this.entities = [];
        this.title = "";
        this.columns = [];
        this.readonly = false;
        this.onAdd = new core_1.EventEmitter();
        this.onUpdate = new core_1.EventEmitter();
        this.onDelete = new core_1.EventEmitter();
        this.entityToEdit = new Object();
    }
    CrudComponent.prototype.ngOnInit = function () {
    };
    CrudComponent.prototype.showDialogToAdd = function () {
        this.newEntity = true;
        this.entityToEdit = new Object();
        this.displayDialog = true;
    };
    CrudComponent.prototype.showDialogToEdit = function (entity) {
        this.newEntity = false;
        this.entityToEdit = entity;
        this.displayDialog = true;
    };
    CrudComponent.prototype.save = function () {
        if (this.newEntity) {
            this.onAdd.emit(this.entityToEdit);
        }
        else {
            this.onUpdate.emit(this.entityToEdit);
        }
        this.displayDialog = false;
    };
    CrudComponent.prototype.delete = function () {
        this.onDelete.emit(this.entityToEdit);
        this.displayDialog = false;
    };
    CrudComponent.prototype.onRowSelect = function (event) {
        if (!this.readonly) {
            this.showDialogToEdit(event.data);
        }
    };
    CrudComponent.prototype.isIdColumn = function (column) {
        return column.propertyName.endsWith('Id');
    };
    return CrudComponent;
}());
__decorate([
    core_1.Input(),
    __metadata("design:type", Array)
], CrudComponent.prototype, "entities", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", String)
], CrudComponent.prototype, "title", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", Array)
], CrudComponent.prototype, "columns", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", Boolean)
], CrudComponent.prototype, "readonly", void 0);
__decorate([
    core_1.Output(),
    __metadata("design:type", core_1.EventEmitter)
], CrudComponent.prototype, "onAdd", void 0);
__decorate([
    core_1.Output(),
    __metadata("design:type", core_1.EventEmitter)
], CrudComponent.prototype, "onUpdate", void 0);
__decorate([
    core_1.Output(),
    __metadata("design:type", core_1.EventEmitter)
], CrudComponent.prototype, "onDelete", void 0);
CrudComponent = __decorate([
    core_1.Component({
        selector: 'crud',
        templateUrl: "./crud.component.html",
        styleUrls: ['./crud.component.css']
    })
], CrudComponent);
exports.CrudComponent = CrudComponent;
//# sourceMappingURL=crud.component.js.map