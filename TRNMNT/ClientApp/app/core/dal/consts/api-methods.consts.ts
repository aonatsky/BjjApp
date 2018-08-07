export const ApiMethods = Object.freeze({
  auth: {
    getToken: 'api/auth/gettoken',
    refreshToken: 'api/auth/updateToken',
    register: 'api/auth/registerParticipantUser',
    updateProfile: 'api/auth/updateProfile',
    changePassword: 'api/auth/changePassword',
    setPassword: 'api/auth/setPassword',
    facebookLogin: 'api/auth/facebooklogin'
  },
  log: 'api/log',
  event: {
    event: 'api/event',
    getEvents: 'api/event/getEvents',
    updateEvent: 'api/event/updateEvent',
    deleteEvent: 'api/event/deleteEvent',
    uploadImage: 'api/event/uploadEventImage',
    uploadTnc: 'api/event/uploadEventTnc',
    uploadPromoCodeList: 'api/event/uploadPromoCode',
    getEvent: 'api/event/getEvent',
    getEventBaseInfo: 'api/event/getEventBaseInfo',
    getEventsForOwner: 'api/event/getEventsForOwner',
    getEventInfo: 'api/event/getEventInfo',
    createEvent: 'api/event/createEvent',
    getPrice: 'api/event/getPrice'
  },
  team: {
    getTeamsForEvent: 'api/team/getTeamsForEvent',
    getTeamsForAdmin: 'api/team/getTeamsForAdmin',
    processTeamRegistration: 'api/team/processTeamRegistration',
    getTeamRegistrationPrice: 'api/team/getTeamRegistrationPrice'
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
    processParticipantRegistration: 'api/participant/processParticipantRegistration',
    isParticipantExist: 'api/participant/isParticipantExist',
    participantsTable: 'api/participant/participantsTable',
    participantsDropdownData: 'api/participant/participantsDropdownData',
    uploadParticipantsFromFile: 'api/participant/uploadParticipantsFromFile',
    update: 'api/participant/update',
    delete: 'api/participant/delete',
    setWeightInStatus: 'api/participant/setWeightInStatus',
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
    getBracketsByCategory: 'api/bracket/getBracketsByCategory',
    getParticipnatsForAbsoluteDivision: 'api/bracket/getParticipnatsForAbsoluteDivision',
    isCategoryCompleted: 'api/bracket/isCategoryCompleted',
    manageAbsoluteWeightDivision: 'api/bracket/manageAbsoluteWeightDivision',
    setRoundResult: 'api/bracket/setRoundResult',
    setBracketResult: 'api/bracket/setBracketResult'
  },
  results: {
    getTeamResutls: 'api/results/getTeamResults',
    getPersonalResultsFile: 'api/results/GetPersonalResultsFile'
  }
});
