export const ApiMethods = Object.freeze({
    tournament: {
        fighters: {
            getAll: "api/fighter",
            uploadlist: "api/fighter/uploadlist",
            getByFilter: "api/fighter/getFightersByFilter",
            getBrackets: "api/fighter/getBracketsByFilter"
        },
        weightDivisions: 'api/weightdivision/',
        categories: 'api/category',
    },
    log: "api/log"
});
