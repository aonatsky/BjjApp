"use strict";
var json_typescript_mapper_1 = require("json-typescript-mapper");
var JsonService = (function () {
    function JsonService() {
    }
    JsonService.deserialize = function (Clazz, json) {
        return json_typescript_mapper_1.deserialize(Clazz, json);
    };
    return JsonService;
}());
exports.JsonService = JsonService;
//# sourceMappingURL=json.service.js.map