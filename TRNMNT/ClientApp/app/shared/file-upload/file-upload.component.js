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
var FileUpload = (function () {
    /**
     *
     */
    function FileUpload() {
        this.onUpload = new core_1.EventEmitter();
    }
    FileUpload.prototype.addFile = function () {
        var fi = this.fileInput.nativeElement;
        if (fi.files && fi.files[0]) {
            var fileToUpload = fi.files[0];
            this.onUpload.emit(fileToUpload);
        }
    };
    return FileUpload;
}());
__decorate([
    core_1.Input(),
    __metadata("design:type", String)
], FileUpload.prototype, "actionUrl", void 0);
__decorate([
    core_1.ViewChild("fileInput"),
    __metadata("design:type", Object)
], FileUpload.prototype, "fileInput", void 0);
__decorate([
    core_1.Output(),
    __metadata("design:type", core_1.EventEmitter)
], FileUpload.prototype, "onUpload", void 0);
FileUpload = __decorate([
    core_1.Component({
        selector: 'file-upload',
        templateUrl: './file-upload.component.html'
    }),
    __metadata("design:paramtypes", [])
], FileUpload);
exports.FileUpload = FileUpload;
//# sourceMappingURL=file-upload.component.js.map