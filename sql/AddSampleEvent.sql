
SET IDENTITY_INSERT [dbo].[AspNetUserClaims] ON
INSERT [dbo].[AspNetUsers]
    ([Id], [AccessFailedCount], [ConcurrencyStamp], [Email], [EmailConfirmed], [FirstName], [LastName], [LockoutEnabled], [LockoutEnd], [NormalizedEmail], [NormalizedUserName], [PasswordHash], [PhoneNumber], [PhoneNumberConfirmed], [SecurityStamp], [TwoFactorEnabled], [UserName], [DateOfBirth])
VALUES
    (N'a2070395-168a-49aa-972b-326edd47bf70', 0, N'809b75de-ce7b-4a66-b4cc-ff8ae303c3ad', N'Ivan.drago@trnmnt.com', 0, N'Ivan', N'Drago', 1, NULL, N'IVAN.DRAGO@TRNMNT.COM', N'ADMIN', N'AQAAAAEAACcQAAAAEOGcrW0OF0EzOc38l11oNC1y8SfFoMQOeqxgXnA44Jh5+L3fds3KgTzGADvUBc05uw==', NULL, 0, N'ca0d2931-6c19-4c68-b855-13dad950e6f4', 0, N'admin', CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2));
INSERT [dbo].[AspNetUserClaims]
    ([Id], [ClaimType], [ClaimValue], [UserId])
VALUES
    (3, N'http://schemas.microsoft.com/ws/2008/06/identity/claims/role', N'Owner', N'a2070395-168a-49aa-972b-326edd47bf70');
SET IDENTITY_INSERT [dbo].[AspNetUserClaims] OFF ;
INSERT [dbo].[Federation]
    ([FederationId], [ContactInformation], [Currency], [Description], [ImgPath], [MembershipPrice], [Name], [OwnerId], [UpdateTs], [TeamRegistrationPrice])
VALUES
    (N'673ea3ce-2530-48c0-b84c-a3de492cab25', NULL, N'UAH', N'Ukrainian Federation Of Brazilian Jiu Jitsu', NULL, 5, N'UABJJF', N'a2070395-168a-49aa-972b-326edd47bf70', CAST(N'2017-08-15T17:25:38.2630000' AS DateTime2), 0);
INSERT [dbo].[Event]
    ([EventId], [Address], [Description], [EventDate], [ImgPath], [IsActive], [OwnerId], [RegistrationEndTS], [RegistrationStartTS], [StatusId], [Title], [UpdateTS], [UrlPrefix], [AdditionalData], [CardNumber], [ContactEmail], [ContactPhone], [FBLink], [TNCFilePath], [VKLink], [EarlyRegistrationEndTS], [EarlyRegistrationPrice], [EarlyRegistrationPriceForMembers], [LateRegistrationPrice], [LateRegistrationPriceForMembers], [FederationId], [PromoCodeEnabled], [PromoCodeListPath])
VALUES
    (N'60e9da46-c660-4a9b-5c43-08d4c85dc994', N'Conroe High School Pit Gym - 3200 W Davis St, Conroe Tx 77304', N'$85 for 1 division, $95 for 2 ($10 off during early online registration period). Early weigh ins are AT THE VENUE the night before at 5:30-7:30 pm and the morning of the event between 8-9:30 am. Please be assured that we make all possible effort to match all competitors (including those who fall in the women''s 150+ division) as closely as possible according to age, weight and rank. Should the event reach capacity, we reserve the right to close online registration early and to restrict registration at the door as we see fit.', CAST(N'2017-10-04T09:00:00.0000000' AS DateTime2), '', 1, N'a2070395-168a-49aa-972b-326edd47bf70', CAST(N'2017-05-29T06:00:00.0000000' AS DateTime2), CAST(N'2017-05-01T06:00:00.0000000' AS DateTime2), 1, N'Kiev open 2022', CAST(N'2017-09-28T15:42:46.2163470' AS DateTime2), N'kievopen2017', N'In-person registration is available for this event. If you are unable to register online, you may register in-person at the venue during early weigh-ins or the morning of the event. In-person registrations are not eligible for any promotional items or discounts that are available online.', NULL, N'bjjukraine@gmail.com', N'+380501234567', N'https://www.facebook.com/events/1218851341576411', '', N'https://vk.com/bjjfreaks', CAST(N'2017-06-01T06:00:00.0000000' AS DateTime2), 95, 8, 8, 13, N'673ea3ce-2530-48c0-b84c-a3de492cab25', 1, NULL);
INSERT [dbo].[Category]
    ([CategoryId], [Name], [EventId], [RoundTime])
VALUES
    (N'8edc9c5c-5bdd-4376-8415-26a886b738b0', N'KIDS 1', N'60e9da46-c660-4a9b-5c43-08d4c85dc994', 300);
INSERT [dbo].[Category]
    ([CategoryId], [Name], [EventId], [RoundTime])
VALUES
    (N'7198b661-e612-40f5-af17-839526393d07', N'Category TEST', N'60e9da46-c660-4a9b-5c43-08d4c85dc994', 300);
INSERT [dbo].[Category]
    ([CategoryId], [Name], [EventId], [RoundTime])
VALUES
    (N'02b44457-be9e-4196-a666-e5602e04ee61', N'Male Balck Belt', N'60e9da46-c660-4a9b-5c43-08d4c85dc994', 300);
INSERT [dbo].[Category]
    ([CategoryId], [Name], [EventId], [RoundTime])
VALUES
    (N'02b44457-be9e-4196-a666-e5602e04ee63', N'Female Black', N'60e9da46-c660-4a9b-5c43-08d4c85dc994', 300);
INSERT [dbo].[WeightDivision]
    ([WeightDivisionId], [Descritpion], [Name], [Weight], [CategoryId])
VALUES
    (N'83ca0f48-1bf7-4441-e54a-08d4c85dc99e', NULL, N'-90 Heavy', 0, N'02b44457-be9e-4196-a666-e5602e04ee61');
INSERT [dbo].[WeightDivision]
    ([WeightDivisionId], [Descritpion], [Name], [Weight], [CategoryId])
VALUES
    (N'0e397f40-c6f9-40f0-e54b-08d4c85dc99e', NULL, N'-70 Light', 0, N'02b44457-be9e-4196-a666-e5602e04ee61');
INSERT [dbo].[WeightDivision]
    ([WeightDivisionId], [Descritpion], [Name], [Weight], [CategoryId])
VALUES
    (N'5b454a2f-8bd1-4a83-e54d-08d4c85dc99e', NULL, N'- 65 Light', 0, N'02b44457-be9e-4196-a666-e5602e04ee63');
INSERT [dbo].[WeightDivision]
    ([WeightDivisionId], [Descritpion], [Name], [Weight], [CategoryId])
VALUES
    (N'84dbfbc7-a754-426c-e54e-08d4c85dc99e', NULL, N'+65 Heavy', 0, N'02b44457-be9e-4196-a666-e5602e04ee63');
INSERT [dbo].[WeightDivision]
    ([WeightDivisionId], [Descritpion], [Name], [Weight], [CategoryId])
VALUES
    (N'0f2641d3-64e4-4aab-ba65-8ee4306a8e5e', NULL, N'Weight Division', 0, N'8edc9c5c-5bdd-4376-8415-26a886b738b0');
INSERT [dbo].[WeightDivision]
    ([WeightDivisionId], [Descritpion], [Name], [Weight], [CategoryId])
VALUES
    (N'a40b083e-cf0e-4445-b26b-36f462afe104', NULL, N'Weight Divison 44', 0, N'7198b661-e612-40f5-af17-839526393d07');