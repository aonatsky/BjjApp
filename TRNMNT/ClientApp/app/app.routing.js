"use strict";
var router_1 = require("@angular/router");
var home_component_1 = require("./administration/home/home.component");
var fighter_list_component_1 = require("./administration/fighter-list/fighter-list.component");
var tournament_settings_component_1 = require("./administration/tournament-settings/tournament-settings.component");
var appRoutes = [
    { path: '', redirectTo: 'home', pathMatch: 'full' },
    { path: 'home', component: home_component_1.HomeComponent },
    { path: 'settings', component: tournament_settings_component_1.TournamentSettingsComponent },
    { path: 'fighter-list', component: fighter_list_component_1.FighterListComponent },
    { path: '**', redirectTo: 'home' }
];
exports.appRoutingProviders = [];
exports.routing = router_1.RouterModule.forRoot(appRoutes);
//# sourceMappingURL=app.routing.js.map