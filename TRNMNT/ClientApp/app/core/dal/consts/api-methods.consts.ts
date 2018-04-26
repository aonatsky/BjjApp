export const ApiMethods = Object.freeze({
    tournament: {
        fighters: {
            fighter: 'api/fighter',
            uploadlist: 'api/fighter/uploadlist',
            getByFilter: 'api/fighter/getFightersByFilter',
            getBrackets: 'api/fighter/getBracketsFile'
        },
        weightDivisions: 'api/weightdivision/',
        categories: 'api/category',
    },
    auth: {
        getToken: 'api/auth/gettoken',
        refreshToken: 'api/auth/updateToken',
        register: 'api/auth/register'
        },
    log: 'api/log',
    event: {
        event: 'api/event',
        getEvents: 'api/event/getEvents',
        updateEvent: 'api/event/updateEvent',
        uploadImage: 'api/event/uploadEventImage',
        uploadTnc: 'api/event/uploadEventTnc',
        uploadPromoCodeList: 'api/event/uploadPromoCode',
        getEvent: 'api/event/getEvent',
        getEventBaseInfo: 'api/event/getEventBaseInfo',
        getEventsForOwner: 'api/event/getEventsForOwner',
        getEventInfo: 'api/event/getEventInfo',
        createEvent: 'api/event/createEvent'
    },
    team: {
        getTeams: 'api/team/getTeams'
    },
    weightDivision: {
        weightDivision: 'api/weightdivision',
        getWeightDivisionsByCategory: 'api/weightdivision/getWeightDivisionsByCategory',
        getWeightDivisionsByEvent: 'api/weightdivision/getWeightDivisionsByEvent'
    },
    category: {
        category: 'api/category',
        getCategoriesForCurrentEvent: 'api/category/getCategoriesForCurrentEvent',
        getCategoriesByEventId: 'api/category/getCategoriesByEventId'
    },
     participant: {
         participant: 'api/participant/',
         processParticipantRegistration: 'api/participant/processParticipantRegistration/',
         isParticipantExist: 'api/participant/isParticipantExist/',
         participantsTable: 'api/participant/participantsTable/',
         participantsDropdownData: 'api/participant/participantsDropdownData/',
         uploadParticipantsFromFile: 'api/participant/uploadParticipantsFromFile/',
         update: 'api/participant/update/',
         delete: 'api/participant/delete/',
    },
     payment: {
         payment: 'api/payment',
         getPaymentData: 'api/payment/GetPaymentDataForParticipant'

     },
    bracket: {
        createBracket: 'api/bracket/createBracket',
        runBracket: 'api/bracket/runBracket',
        downloadFile: 'api/bracket/downloadFile',
        updateBracket: 'api/bracket/updateBracket',
        finishRound: 'api/bracket/finishRound',
        getBracketsByCategory: 'api/bracket/getBracketsByCategory',
        getWinners: 'api/bracket/getWinners',
        isAllWinnersSelected: 'api/bracket/isAllWinnersSelected',
        manageAbsoluteWeightDivision: 'api/bracket/manageAbsoluteWeightDivision',
    },
    results: {
        getTeamResutls: 'api/results/getTeamResults'
    }
});
