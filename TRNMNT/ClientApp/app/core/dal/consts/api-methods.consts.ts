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
        refreshToken: "api/auth/refreshtoken",
        register: "api/auth/register"
        },
    log: "api/log",
    event: {
        event: "api/event",
        getEvents: "api/event/getEvents",
        saveEvent: "api/event/saveEvent",
        uploadImage: "api/event/uploadEventImage",
        uploadTnc: "api/event/uploadEventTnc",
        getEvent: "api/event/getEvent",
        getEventsForOwner: "api/event/getEventsForOwner",
        getEventIdByUrl: "api/event/getEventIdByUrl",
        getEventByUrl: "api/event/getEventByUrl",
        createEvent: "api/event/createEvent"
    },
    team: {
        team: "api/team"
    },
    weightDivision: {
        weightDivision: "api/weightdivision"
    },
    category: {
        category: "api/category"
    }
});
