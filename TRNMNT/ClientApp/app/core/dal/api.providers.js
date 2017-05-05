"use strict";
var api_server_1 = require("./servers/api.server");
var data_api_service_1 = require("./api/data-api.service");
var data_service_1 = require("./contracts/data.service");
exports.ApiProviders = [
    {
        provide: data_service_1.DataService,
        useClass: data_api_service_1.DataApiService
    },
    {
        provide: api_server_1.ApiServer,
        useClass: api_server_1.ApiServer
    }
];
//# sourceMappingURL=api.providers.js.map