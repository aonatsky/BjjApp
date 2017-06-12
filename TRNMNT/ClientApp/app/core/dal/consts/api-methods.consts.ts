export const ApiMethods = Object.freeze({
    tournament: {
        fighters: {
            fighter: "api/fighter",
            uploadlist: "api/fighter/uploadlist",
            getByFilter: "api/fighter/getFightersByFilter",
            getBrackets: "api/fighter/getBracketsFile"
        },
        weightDivisions: 'api/weightdivision/',
        categories: 'api/category',
    },
    auth: {
        getToken: "api/gettoken",
        refreshToken: "api/refreshtoken"
        },
    log: "api/log"
});
