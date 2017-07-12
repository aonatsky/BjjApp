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
        getToken: "api/auth/gettoken",
        refreshToken: "api/auth/refreshtoken"
        },
    log: "api/log",
    event: {
        getEvents: "api/event/getEvents",
        saveEvent: "api/event/saveEvent",
        getEvent: "api/event/getEvent",
        getEventsForOwner: "api/event/getEventsForOwner"
    }
});
