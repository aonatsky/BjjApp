import { CategoryModel } from './category.models';

export class EventModelBase {
  eventId: AAGUID;
  title: string;
  eventDate: Date;
  registrationStartTS: Date;
  earlyRegistrationEndTS: Date;
  registrationEndTS: Date;
  correctionsEnabled: boolean;
  participantListsPublished: boolean;
  bracketsPublished: boolean;
}

export class EventModel extends EventModelBase {
  description: string;
  address: string;
  imgPath: string;
  urlPrefix: string;
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

  constructor() {
    super();
    this.categoryModels = [];
  }
}

export class EventPreviewModel extends EventModelBase {
  constructor() {
    super();
  }
}

export class EventDashboardModel extends EventModelBase {
  bracketsCreated: boolean;
  eventStatus: boolean;
  participantGroups: CategoryWeightDivisionParticipants[];
  constructor() {
    super();
  }
}

export class CategoryWeightDivisionParticipants {
  categoryName: string;
  weightDivisionName: string;
  participantsCount: number;
}
