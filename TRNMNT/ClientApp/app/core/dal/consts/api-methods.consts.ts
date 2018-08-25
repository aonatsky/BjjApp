export const ApiMethods = Object.freeze({
  auth: {
    getToken: 'api/auth/gettoken',
    refreshToken: 'api/auth/updateToken',
    facebookLogin: 'api/auth/facebooklogin'
  },
  user: {
    register: 'api/user/registerParticipantUser',
    updateProfile: 'api/user/updateProfile',
    changePassword: 'api/user/changePassword',
    setPassword: 'api/user/setPassword'
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
    isPrefixExists: 'api/event/isPrefixExists',
    getPrice: 'api/event/getPrice',
    getTeamPrice: 'api/event/getTeamPrice',
    disableCorrections: 'api/event/disableCorrections',
    publishParticipantLists: 'api/event/publishParticipantLists',
    publishBrackets: 'api/event/publishBrackets',
    getEventDashboardData: 'api/event/getEventDashboardData'
  },
  team: {
    getTeamsForEvent: 'api/team/getTeamsForEvent',
    getTeamsForAdmin: 'api/team/getTeamsForAdmin',
    processTeamRegistration: 'api/team/processTeamRegistration',
    getTeamRegistrationPrice: 'api/team/getTeamRegistrationPrice',
    getAthletes: 'api/team/GetAthletes',
    getAthletesForParticipation:'api/team/getAthletesForParticipation',
    getCurrentAthlete: 'api/team/GetCurrentAthlete',
    approveTeamMembership: 'api/team/approveTeamMembership',
    declineTeamMembership: 'api/team/declineTeamMembership',
    declineTeam:'api/team/declineTeam',
    approveTeam:'api/team/approveTeam',
  },
  federation: {
    getFederation: 'api/federation/getFederation',
    updateFederation: 'api/federation/updateFederation'
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
    processParticipantTeamRegistration: 'api/participant/processParticipantTeamRegistration',
    isParticipantExist: 'api/participant/isParticipantExist',
    participantsTable: 'api/participant/participantsTable',
    participantsDropdownData: 'api/participant/participantsDropdownData',
    uploadParticipantsFromFile: 'api/participant/uploadParticipantsFromFile',
    update: 'api/participant/update',
    delete: 'api/participant/delete',
    setWeightInStatus: 'api/participant/setWeightInStatus',
    isFederationMember: 'api/participant/isFederationMember',
    getUserParticipations: 'api/participant/GetUserParticipations',

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
    getParticipantsForAbsoluteDivision: 'api/bracket/getParticipantsForAbsoluteDivision',
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
