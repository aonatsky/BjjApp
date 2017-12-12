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
        updateEvent: "api/event/updateEvent",
        uploadImage: "api/event/uploadEventImage",
        uploadTnc: "api/event/uploadEventTnc",
        uploadPromoCodeList: "api/event/uploadPromoCode",
        getEvent: "api/event/getEvent",
        getEventBaseInfo: "api/event/getEventBaseInfo",
        getEventsForOwner: "api/event/getEventsForOwner",
        getEventInfo: "api/event/getEventInfo",
        createEvent: "api/event/createEvent"
    },
    team: {
        getTeams: "api/team/getTeams"
    },
    weightDivision: {
        weightDivision: "api/weightdivision",
        getWeightDivisionsByCategory: "api/weightdivision/getWeightDivisionsByCategory"
    },
    category: {
        category: "api/category",
        getCategoriesForEvent: "api/category/getCategoriesForEvent"
    },
     participant: {
         participant: "api/participant/",
         processParticipantRegistration: "api/participant/processParticipantRegistration/",
         isParticipantExist: "api/participant/isParticipantExist/",
         participantsTable: "api/participant/participantsTable/",
    },
     payment: {
         payment: "api/payment",
         getPaymentData: "api/payment/GetPaymentDataForParticipant"

     }
});
