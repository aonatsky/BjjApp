import { CategoryModel } from './category.models';

export class EventModelBase {
  eventId: AAGUID;
  title: string;
  eventDate: Date;
  registrationStartTS: Date;
  earlyRegistrationEndTS: Date;
  registrationEndTS: Date;
}

export class EventModel extends EventModelBase {
  constructor() {
    super();
    this.categoryModels = [];
  }

  description: string;
  address: string;
  imgPath: string;
  urlPrefix: string;
  descritpion: string;
  tncFilePath: string;
  cardNumber: string;
  contactEmail: string;
  contactPhone: string;
  fbLink: string;
  vkLink: string;
  additionalData: string;
  promoCodeEnabled: boolean;
  promoCodeListPath: string;
  earlyRegistrationPrice: number;
  lateRegistrationPrice: number;
  earlyRegistrationPriceForMembers: number;
  lateRegistrationPriceForMembers: number;
  categoryModels: CategoryModel[];
}

export class EventPreviewModel extends EventModelBase {
  constructor() {
    super();
  }
}

export class EventDashboardModel extends EventModelBase {
  constructor() {
    super();
  }
  bracketsCreated: boolean;
  correctionsEnabled: boolean;
  participantListsPublished: boolean;
  bracketsPublished: boolean;
  eventStatus: boolean;
  participantGroups: CategoryWeightDivisionParticipants[]
}

export class CategoryWeightDivisionParticipants {
  categoryName: string;
  weightDivisionName: string;
  participantsCount: number;
}
