"use strict";
var Fighter = (function () {
    function Fighter(fighterID, firstName, lastName, team, dateOfBirth, category) {
        this.fighterID = fighterID;
        this.firstName = firstName;
        this.lastName = lastName;
        this.team = team;
        this.dateOfBirth = dateOfBirth;
        this.category = category;
    }
    return Fighter;
}());
exports.Fighter = Fighter;
//# sourceMappingURL=fighter.model.js.map