CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" varchar(150) NOT NULL,
    "ProductVersion" varchar(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180622090804_initial') THEN
    CREATE TABLE "AspNetRoles" (
        "Id" text NOT NULL,
        "Name" varchar(256) NULL,
        "NormalizedName" varchar(256) NULL,
        "ConcurrencyStamp" text NULL,
        CONSTRAINT "PK_AspNetRoles" PRIMARY KEY ("Id")
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180622090804_initial') THEN
    CREATE TABLE "AspNetUsers" (
        "AccessFailedCount" integer NOT NULL,
        "EmailConfirmed" boolean NOT NULL,
        "LockoutEnabled" boolean NOT NULL,
        "LockoutEnd" timestamp with time zone NULL,
        "PhoneNumberConfirmed" boolean NOT NULL,
        "TwoFactorEnabled" boolean NOT NULL,
        "Id" text NOT NULL,
        "UserName" varchar(256) NULL,
        "NormalizedUserName" varchar(256) NULL,
        "Email" varchar(256) NULL,
        "NormalizedEmail" varchar(256) NULL,
        "PasswordHash" text NULL,
        "SecurityStamp" text NULL,
        "ConcurrencyStamp" text NULL,
        "PhoneNumber" text NULL,
        "FirstName" text NULL,
        "LastName" text NULL,
        "DateOfBirth" timestamp without time zone NOT NULL,
        CONSTRAINT "PK_AspNetUsers" PRIMARY KEY ("Id")
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180622090804_initial') THEN
    CREATE TABLE "AspNetRoleClaims" (
        "Id" serial NOT NULL,
        "RoleId" text NOT NULL,
        "ClaimType" text NULL,
        "ClaimValue" text NULL,
        CONSTRAINT "PK_AspNetRoleClaims" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_AspNetRoleClaims_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180622090804_initial') THEN
    CREATE TABLE "AspNetUserClaims" (
        "Id" serial NOT NULL,
        "UserId" text NOT NULL,
        "ClaimType" text NULL,
        "ClaimValue" text NULL,
        CONSTRAINT "PK_AspNetUserClaims" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_AspNetUserClaims_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180622090804_initial') THEN
    CREATE TABLE "AspNetUserLogins" (
        "LoginProvider" text NOT NULL,
        "ProviderKey" text NOT NULL,
        "ProviderDisplayName" text NULL,
        "UserId" text NOT NULL,
        CONSTRAINT "PK_AspNetUserLogins" PRIMARY KEY ("LoginProvider", "ProviderKey"),
        CONSTRAINT "FK_AspNetUserLogins_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180622090804_initial') THEN
    CREATE TABLE "AspNetUserRoles" (
        "UserId" text NOT NULL,
        "RoleId" text NOT NULL,
        CONSTRAINT "PK_AspNetUserRoles" PRIMARY KEY ("UserId", "RoleId"),
        CONSTRAINT "FK_AspNetUserRoles_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE RESTRICT,
        CONSTRAINT "FK_AspNetUserRoles_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180622090804_initial') THEN
    CREATE TABLE "AspNetUserTokens" (
        "UserId" text NOT NULL,
        "LoginProvider" text NOT NULL,
        "Name" text NOT NULL,
        "Value" text NULL,
        CONSTRAINT "PK_AspNetUserTokens" PRIMARY KEY ("UserId", "LoginProvider", "Name"),
        CONSTRAINT "FK_AspNetUserTokens_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180622090804_initial') THEN
    CREATE TABLE "Federation" (
        "FederationId" uuid NOT NULL,
        "Name" text NULL,
        "Description" text NULL,
        "UpdateTs" timestamp without time zone NOT NULL,
        "OwnerId" text NULL,
        "MembershipPrice" integer NOT NULL,
        "TeamRegistrationPrice" integer NOT NULL,
        "Currency" text NULL,
        "ContactInformation" text NULL,
        "ImgPath" text NULL,
        CONSTRAINT "PK_Federation" PRIMARY KEY ("FederationId"),
        CONSTRAINT "FK_Federation_AspNetUsers_OwnerId" FOREIGN KEY ("OwnerId") REFERENCES "AspNetUsers" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180622090804_initial') THEN
    CREATE TABLE "Order" (
        "OrderId" uuid NOT NULL,
        "OrderTypeId" integer NOT NULL,
        "PaymentApproved" boolean NOT NULL,
        "Amount" integer NOT NULL,
        "PaymentProviderReference" text NULL,
        "Currency" text NULL,
        "Reference" text NULL,
        "UserId" text NULL,
        "CreateTS" timestamp without time zone NOT NULL,
        "UpdateTS" timestamp without time zone NOT NULL,
        CONSTRAINT "PK_Order" PRIMARY KEY ("OrderId"),
        CONSTRAINT "FK_Order_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180622090804_initial') THEN
    CREATE TABLE "Event" (
        "EventId" uuid NOT NULL,
        "OwnerId" text NULL,
        "EventDate" timestamp without time zone NOT NULL,
        "RegistrationStartTS" timestamp without time zone NOT NULL,
        "EarlyRegistrationEndTS" timestamp without time zone NOT NULL,
        "RegistrationEndTS" timestamp without time zone NOT NULL,
        "ImgPath" text NULL,
        "Title" text NULL,
        "UpdateTS" timestamp without time zone NOT NULL,
        "IsActive" boolean NOT NULL,
        "StatusId" integer NOT NULL,
        "UrlPrefix" text NULL,
        "Description" text NULL,
        "Address" text NULL,
        "TNCFilePath" text NULL,
        "CardNumber" text NULL,
        "ContactEmail" text NULL,
        "ContactPhone" text NULL,
        "FBLink" text NULL,
        "VKLink" text NULL,
        "AdditionalData" text NULL,
        "FederationId" uuid NOT NULL,
        "PromoCodeEnabled" boolean NOT NULL,
        "PromoCodeListPath" text NULL,
        "EarlyRegistrationPrice" integer NOT NULL,
        "LateRegistrationPrice" integer NOT NULL,
        "EarlyRegistrationPriceForMembers" integer NOT NULL,
        "LateRegistrationPriceForMembers" integer NOT NULL,
        CONSTRAINT "PK_Event" PRIMARY KEY ("EventId"),
        CONSTRAINT "FK_Event_Federation_FederationId" FOREIGN KEY ("FederationId") REFERENCES "Federation" ("FederationId") ON DELETE RESTRICT,
        CONSTRAINT "FK_Event_AspNetUsers_OwnerId" FOREIGN KEY ("OwnerId") REFERENCES "AspNetUsers" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180622090804_initial') THEN
    CREATE TABLE "FederationMembership" (
        "FederationMembershipId" uuid NOT NULL,
        "FederationId" uuid NOT NULL,
        "UserId" text NULL,
        "CreateTs" timestamp without time zone NOT NULL,
        CONSTRAINT "PK_FederationMembership" PRIMARY KEY ("FederationMembershipId"),
        CONSTRAINT "FK_FederationMembership_Federation_FederationId" FOREIGN KEY ("FederationId") REFERENCES "Federation" ("FederationId") ON DELETE RESTRICT,
        CONSTRAINT "FK_FederationMembership_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180622090804_initial') THEN
    CREATE TABLE "Team" (
        "TeamId" uuid NOT NULL,
        "Name" text NULL,
        "Description" text NULL,
        "UpdateTs" timestamp without time zone NOT NULL,
        "UpdateBy" text NULL,
        "FederationId" uuid NOT NULL,
        "OrderId" uuid NULL,
        "IsApproved" boolean NOT NULL,
        CONSTRAINT "PK_Team" PRIMARY KEY ("TeamId"),
        CONSTRAINT "FK_Team_Federation_FederationId" FOREIGN KEY ("FederationId") REFERENCES "Federation" ("FederationId") ON DELETE RESTRICT
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180622090804_initial') THEN
    CREATE TABLE "Category" (
        "CategoryId" uuid NOT NULL,
        "Name" text NULL,
        "EventId" uuid NOT NULL,
        "MatchTime" integer NOT NULL,
        "CompleteTs" timestamp without time zone NULL,
        CONSTRAINT "PK_Category" PRIMARY KEY ("CategoryId"),
        CONSTRAINT "FK_Category_Event_EventId" FOREIGN KEY ("EventId") REFERENCES "Event" ("EventId") ON DELETE RESTRICT
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180622090804_initial') THEN
    CREATE TABLE "PromoCode" (
        "PromoCodeId" uuid NOT NULL,
        "EventId" uuid NOT NULL,
        "Code" text NULL,
        "IsActive" boolean NOT NULL,
        "UpdateTs" timestamp without time zone NULL,
        "BurntBy" text NULL,
        CONSTRAINT "PK_PromoCode" PRIMARY KEY ("PromoCodeId"),
        CONSTRAINT "FK_PromoCode_Event_EventId" FOREIGN KEY ("EventId") REFERENCES "Event" ("EventId") ON DELETE RESTRICT
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180622090804_initial') THEN
    CREATE TABLE "WeightDivision" (
        "WeightDivisionId" uuid NOT NULL,
        "Name" text NULL,
        "Weight" integer NOT NULL,
        "Descritpion" text NULL,
        "CategoryId" uuid NOT NULL,
        "IsAbsolute" boolean NOT NULL,
        "StartTs" timestamp without time zone NULL,
        "CompleteTs" timestamp without time zone NULL,
        CONSTRAINT "PK_WeightDivision" PRIMARY KEY ("WeightDivisionId"),
        CONSTRAINT "FK_WeightDivision_Category_CategoryId" FOREIGN KEY ("CategoryId") REFERENCES "Category" ("CategoryId") ON DELETE RESTRICT
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180622090804_initial') THEN
    CREATE TABLE "Participant" (
        "ParticipantId" uuid NOT NULL,
        "FirstName" text NULL,
        "LastName" text NULL,
        "Email" text NULL,
        "PhoneNumber" text NULL,
        "DateOfBirth" timestamp without time zone NOT NULL,
        "UpdateTS" timestamp without time zone NOT NULL,
        "UserId" text NULL,
        "IsDisqualified" boolean NOT NULL,
        "IsActive" boolean NOT NULL,
        "IsApproved" boolean NOT NULL,
        "IsMember" boolean NOT NULL,
        "OrderId" uuid NULL,
        "TeamId" uuid NOT NULL,
        "CategoryId" uuid NOT NULL,
        "EventId" uuid NOT NULL,
        "WeightDivisionId" uuid NOT NULL,
        "AbsoluteWeightDivisionId" uuid NULL,
        "ActivatedPromoCode" text NULL,
        CONSTRAINT "PK_Participant" PRIMARY KEY ("ParticipantId"),
        CONSTRAINT "FK_Participant_WeightDivision_AbsoluteWeightDivisionId" FOREIGN KEY ("AbsoluteWeightDivisionId") REFERENCES "WeightDivision" ("WeightDivisionId") ON DELETE RESTRICT,
        CONSTRAINT "FK_Participant_Category_CategoryId" FOREIGN KEY ("CategoryId") REFERENCES "Category" ("CategoryId") ON DELETE RESTRICT,
        CONSTRAINT "FK_Participant_Event_EventId" FOREIGN KEY ("EventId") REFERENCES "Event" ("EventId") ON DELETE RESTRICT,
        CONSTRAINT "FK_Participant_Team_TeamId" FOREIGN KEY ("TeamId") REFERENCES "Team" ("TeamId") ON DELETE RESTRICT,
        CONSTRAINT "FK_Participant_WeightDivision_WeightDivisionId" FOREIGN KEY ("WeightDivisionId") REFERENCES "WeightDivision" ("WeightDivisionId") ON DELETE RESTRICT
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180622090804_initial') THEN
    CREATE TABLE "Match" (
        "MatchId" uuid NOT NULL,
        "WeightDivisionId" uuid NOT NULL,
        "CategoryId" uuid NOT NULL,
        "AParticipantId" uuid NULL,
        "BParticipantId" uuid NULL,
        "WinnerParticipantId" uuid NULL,
        "NextMatchId" uuid NULL,
        "Round" integer NOT NULL,
        "Order" integer NOT NULL,
        "MatchType" integer NOT NULL,
        "MatchResultType" integer NOT NULL,
        "MatchResultDetails" text NULL,
        CONSTRAINT "PK_Match" PRIMARY KEY ("MatchId"),
        CONSTRAINT "FK_Match_Participant_AParticipantId" FOREIGN KEY ("AParticipantId") REFERENCES "Participant" ("ParticipantId") ON DELETE RESTRICT,
        CONSTRAINT "FK_Match_Participant_BParticipantId" FOREIGN KEY ("BParticipantId") REFERENCES "Participant" ("ParticipantId") ON DELETE RESTRICT,
        CONSTRAINT "FK_Match_Category_CategoryId" FOREIGN KEY ("CategoryId") REFERENCES "Category" ("CategoryId") ON DELETE RESTRICT,
        CONSTRAINT "FK_Match_Match_NextMatchId" FOREIGN KEY ("NextMatchId") REFERENCES "Match" ("MatchId") ON DELETE RESTRICT,
        CONSTRAINT "FK_Match_WeightDivision_WeightDivisionId" FOREIGN KEY ("WeightDivisionId") REFERENCES "WeightDivision" ("WeightDivisionId") ON DELETE RESTRICT,
        CONSTRAINT "FK_Match_Participant_WinnerParticipantId" FOREIGN KEY ("WinnerParticipantId") REFERENCES "Participant" ("ParticipantId") ON DELETE RESTRICT
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180622090804_initial') THEN
    CREATE INDEX "IX_AspNetRoleClaims_RoleId" ON "AspNetRoleClaims" ("RoleId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180622090804_initial') THEN
    CREATE UNIQUE INDEX "RoleNameIndex" ON "AspNetRoles" ("NormalizedName");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180622090804_initial') THEN
    CREATE INDEX "IX_AspNetUserClaims_UserId" ON "AspNetUserClaims" ("UserId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180622090804_initial') THEN
    CREATE INDEX "IX_AspNetUserLogins_UserId" ON "AspNetUserLogins" ("UserId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180622090804_initial') THEN
    CREATE INDEX "IX_AspNetUserRoles_RoleId" ON "AspNetUserRoles" ("RoleId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180622090804_initial') THEN
    CREATE INDEX "EmailIndex" ON "AspNetUsers" ("NormalizedEmail");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180622090804_initial') THEN
    CREATE UNIQUE INDEX "UserNameIndex" ON "AspNetUsers" ("NormalizedUserName");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180622090804_initial') THEN
    CREATE INDEX "IX_Category_EventId" ON "Category" ("EventId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180622090804_initial') THEN
    CREATE INDEX "IX_Event_FederationId" ON "Event" ("FederationId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180622090804_initial') THEN
    CREATE INDEX "IX_Event_OwnerId" ON "Event" ("OwnerId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180622090804_initial') THEN
    CREATE INDEX "IX_Federation_OwnerId" ON "Federation" ("OwnerId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180622090804_initial') THEN
    CREATE INDEX "IX_FederationMembership_FederationId" ON "FederationMembership" ("FederationId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180622090804_initial') THEN
    CREATE INDEX "IX_FederationMembership_UserId" ON "FederationMembership" ("UserId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180622090804_initial') THEN
    CREATE INDEX "IX_Match_AParticipantId" ON "Match" ("AParticipantId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180622090804_initial') THEN
    CREATE INDEX "IX_Match_BParticipantId" ON "Match" ("BParticipantId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180622090804_initial') THEN
    CREATE INDEX "IX_Match_CategoryId" ON "Match" ("CategoryId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180622090804_initial') THEN
    CREATE INDEX "IX_Match_NextMatchId" ON "Match" ("NextMatchId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180622090804_initial') THEN
    CREATE INDEX "IX_Match_WeightDivisionId" ON "Match" ("WeightDivisionId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180622090804_initial') THEN
    CREATE INDEX "IX_Match_WinnerParticipantId" ON "Match" ("WinnerParticipantId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180622090804_initial') THEN
    CREATE INDEX "IX_Order_UserId" ON "Order" ("UserId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180622090804_initial') THEN
    CREATE INDEX "IX_Participant_AbsoluteWeightDivisionId" ON "Participant" ("AbsoluteWeightDivisionId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180622090804_initial') THEN
    CREATE INDEX "IX_Participant_CategoryId" ON "Participant" ("CategoryId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180622090804_initial') THEN
    CREATE INDEX "IX_Participant_EventId" ON "Participant" ("EventId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180622090804_initial') THEN
    CREATE INDEX "IX_Participant_TeamId" ON "Participant" ("TeamId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180622090804_initial') THEN
    CREATE INDEX "IX_Participant_WeightDivisionId" ON "Participant" ("WeightDivisionId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180622090804_initial') THEN
    CREATE INDEX "IX_PromoCode_EventId" ON "PromoCode" ("EventId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180622090804_initial') THEN
    CREATE INDEX "IX_Team_FederationId" ON "Team" ("FederationId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180622090804_initial') THEN
    CREATE INDEX "IX_WeightDivision_CategoryId" ON "WeightDivision" ("CategoryId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180622090804_initial') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20180622090804_initial', '2.1.1-rtm-30846');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180627131317_facebookDataadded') THEN
    ALTER TABLE "AspNetUsers" ADD "FacebookId" bigint NOT NULL DEFAULT 0;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180627131317_facebookDataadded') THEN
    ALTER TABLE "AspNetUsers" ADD "PictureUrl" text NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180627131317_facebookDataadded') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20180627131317_facebookDataadded', '2.1.1-rtm-30846');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180702154239_userupdate') THEN
    ALTER TABLE "AspNetUsers" ADD "CreateTS" timestamp without time zone NOT NULL DEFAULT TIMESTAMP '0001-01-01 00:00:00';
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180702154239_userupdate') THEN
    ALTER TABLE "AspNetUsers" ADD "IsActive" boolean NOT NULL DEFAULT FALSE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180702154239_userupdate') THEN
    ALTER TABLE "AspNetUsers" ADD "RegistrationCompleted" boolean NOT NULL DEFAULT FALSE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180702154239_userupdate') THEN
    ALTER TABLE "AspNetUsers" ADD "SocialLogin" text NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180702154239_userupdate') THEN
    ALTER TABLE "AspNetUsers" ADD "UpdateTS" timestamp without time zone NOT NULL DEFAULT TIMESTAMP '0001-01-01 00:00:00';
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180702154239_userupdate') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20180702154239_userupdate', '2.1.1-rtm-30846');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180724122817_OrderStatusAdded') THEN
    ALTER TABLE "Order" ADD "Status" text NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180724122817_OrderStatusAdded') THEN
    ALTER TABLE "FederationMembership" ADD "IsApproved" boolean NOT NULL DEFAULT FALSE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180724122817_OrderStatusAdded') THEN
    ALTER TABLE "FederationMembership" ADD "UpdateTs" timestamp without time zone NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180724122817_OrderStatusAdded') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20180724122817_OrderStatusAdded', '2.1.1-rtm-30846');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180724162149_ParticipantStatusAdded') THEN
    ALTER TABLE "Participant" DROP COLUMN "IsApproved";
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180724162149_ParticipantStatusAdded') THEN
    ALTER TABLE "Order" DROP COLUMN "PaymentApproved";
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180724162149_ParticipantStatusAdded') THEN
    ALTER TABLE "Participant" ADD "ApprovalStatus" text NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180724162149_ParticipantStatusAdded') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20180724162149_ParticipantStatusAdded', '2.1.1-rtm-30846');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180725084332_LimitsAdded') THEN
    ALTER TABLE "WeightDivision" DROP COLUMN "Descritpion";
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180725084332_LimitsAdded') THEN
    ALTER TABLE "Team" DROP COLUMN "IsApproved";
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180725084332_LimitsAdded') THEN
    ALTER TABLE "Team" RENAME COLUMN "UpdateBy" TO "CreateBy";
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180725084332_LimitsAdded') THEN
    ALTER TABLE "FederationMembership" RENAME COLUMN "IsApproved" TO "CreateBy";
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180725084332_LimitsAdded') THEN
    ALTER TABLE "WeightDivision" ALTER COLUMN "Name" TYPE varchar(100);
    ALTER TABLE "WeightDivision" ALTER COLUMN "Name" DROP NOT NULL;
    ALTER TABLE "WeightDivision" ALTER COLUMN "Name" DROP DEFAULT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180725084332_LimitsAdded') THEN
    ALTER TABLE "WeightDivision" ADD "Description" varchar(1000) NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180725084332_LimitsAdded') THEN
    ALTER TABLE "Team" ALTER COLUMN "Name" TYPE varchar(100);
    ALTER TABLE "Team" ALTER COLUMN "Name" DROP NOT NULL;
    ALTER TABLE "Team" ALTER COLUMN "Name" DROP DEFAULT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180725084332_LimitsAdded') THEN
    ALTER TABLE "Team" ALTER COLUMN "Description" TYPE varchar(1000);
    ALTER TABLE "Team" ALTER COLUMN "Description" DROP NOT NULL;
    ALTER TABLE "Team" ALTER COLUMN "Description" DROP DEFAULT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180725084332_LimitsAdded') THEN
    ALTER TABLE "Team" ADD "ApprovalStatus" text NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180725084332_LimitsAdded') THEN
    ALTER TABLE "Team" ADD "ContactEmail" varchar(1000) NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180725084332_LimitsAdded') THEN
    ALTER TABLE "Team" ADD "ContactName" varchar(1000) NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180725084332_LimitsAdded') THEN
    ALTER TABLE "Team" ADD "ContactPhone" varchar(1000) NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180725084332_LimitsAdded') THEN
    ALTER TABLE "Team" ADD "CreateTs" timestamp without time zone NOT NULL DEFAULT TIMESTAMP '0001-01-01 00:00:00';
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180725084332_LimitsAdded') THEN
    ALTER TABLE "Participant" ALTER COLUMN "PhoneNumber" TYPE varchar(100);
    ALTER TABLE "Participant" ALTER COLUMN "PhoneNumber" DROP NOT NULL;
    ALTER TABLE "Participant" ALTER COLUMN "PhoneNumber" DROP DEFAULT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180725084332_LimitsAdded') THEN
    ALTER TABLE "Participant" ALTER COLUMN "LastName" TYPE varchar(100);
    ALTER TABLE "Participant" ALTER COLUMN "LastName" DROP NOT NULL;
    ALTER TABLE "Participant" ALTER COLUMN "LastName" DROP DEFAULT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180725084332_LimitsAdded') THEN
    ALTER TABLE "Participant" ALTER COLUMN "FirstName" TYPE varchar(100);
    ALTER TABLE "Participant" ALTER COLUMN "FirstName" DROP NOT NULL;
    ALTER TABLE "Participant" ALTER COLUMN "FirstName" DROP DEFAULT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180725084332_LimitsAdded') THEN
    ALTER TABLE "Participant" ALTER COLUMN "Email" TYPE varchar(100);
    ALTER TABLE "Participant" ALTER COLUMN "Email" DROP NOT NULL;
    ALTER TABLE "Participant" ALTER COLUMN "Email" DROP DEFAULT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180725084332_LimitsAdded') THEN
    ALTER TABLE "FederationMembership" ADD "ApprovalStatus" text NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180725084332_LimitsAdded') THEN
    ALTER TABLE "Event" ALTER COLUMN "VKLink" TYPE varchar(500);
    ALTER TABLE "Event" ALTER COLUMN "VKLink" DROP NOT NULL;
    ALTER TABLE "Event" ALTER COLUMN "VKLink" DROP DEFAULT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180725084332_LimitsAdded') THEN
    ALTER TABLE "Event" ALTER COLUMN "UrlPrefix" TYPE varchar(100);
    ALTER TABLE "Event" ALTER COLUMN "UrlPrefix" DROP NOT NULL;
    ALTER TABLE "Event" ALTER COLUMN "UrlPrefix" DROP DEFAULT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180725084332_LimitsAdded') THEN
    ALTER TABLE "Event" ALTER COLUMN "Title" TYPE varchar(100);
    ALTER TABLE "Event" ALTER COLUMN "Title" DROP NOT NULL;
    ALTER TABLE "Event" ALTER COLUMN "Title" DROP DEFAULT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180725084332_LimitsAdded') THEN
    ALTER TABLE "Event" ALTER COLUMN "FBLink" TYPE varchar(500);
    ALTER TABLE "Event" ALTER COLUMN "FBLink" DROP NOT NULL;
    ALTER TABLE "Event" ALTER COLUMN "FBLink" DROP DEFAULT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180725084332_LimitsAdded') THEN
    ALTER TABLE "Event" ALTER COLUMN "ContactPhone" TYPE varchar(100);
    ALTER TABLE "Event" ALTER COLUMN "ContactPhone" DROP NOT NULL;
    ALTER TABLE "Event" ALTER COLUMN "ContactPhone" DROP DEFAULT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180725084332_LimitsAdded') THEN
    ALTER TABLE "Event" ALTER COLUMN "ContactEmail" TYPE varchar(100);
    ALTER TABLE "Event" ALTER COLUMN "ContactEmail" DROP NOT NULL;
    ALTER TABLE "Event" ALTER COLUMN "ContactEmail" DROP DEFAULT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180725084332_LimitsAdded') THEN
    ALTER TABLE "Event" ALTER COLUMN "CardNumber" TYPE varchar(100);
    ALTER TABLE "Event" ALTER COLUMN "CardNumber" DROP NOT NULL;
    ALTER TABLE "Event" ALTER COLUMN "CardNumber" DROP DEFAULT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180725084332_LimitsAdded') THEN
    ALTER TABLE "Event" ALTER COLUMN "Address" TYPE varchar(500);
    ALTER TABLE "Event" ALTER COLUMN "Address" DROP NOT NULL;
    ALTER TABLE "Event" ALTER COLUMN "Address" DROP DEFAULT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180725084332_LimitsAdded') THEN
    ALTER TABLE "Category" ALTER COLUMN "Name" TYPE varchar(100);
    ALTER TABLE "Category" ALTER COLUMN "Name" DROP NOT NULL;
    ALTER TABLE "Category" ALTER COLUMN "Name" DROP DEFAULT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180725084332_LimitsAdded') THEN
    ALTER TABLE "AspNetUsers" ALTER COLUMN "LastName" TYPE varchar(100);
    ALTER TABLE "AspNetUsers" ALTER COLUMN "LastName" DROP NOT NULL;
    ALTER TABLE "AspNetUsers" ALTER COLUMN "LastName" DROP DEFAULT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180725084332_LimitsAdded') THEN
    ALTER TABLE "AspNetUsers" ALTER COLUMN "FirstName" TYPE varchar(100);
    ALTER TABLE "AspNetUsers" ALTER COLUMN "FirstName" DROP NOT NULL;
    ALTER TABLE "AspNetUsers" ALTER COLUMN "FirstName" DROP DEFAULT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180725084332_LimitsAdded') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20180725084332_LimitsAdded', '2.1.1-rtm-30846');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180725151416_NullableUpdateTsAdded') THEN
    ALTER TABLE "Team" ALTER COLUMN "UpdateTs" TYPE timestamp without time zone;
    ALTER TABLE "Team" ALTER COLUMN "UpdateTs" DROP NOT NULL;
    ALTER TABLE "Team" ALTER COLUMN "UpdateTs" DROP DEFAULT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180725151416_NullableUpdateTsAdded') THEN
    ALTER TABLE "Participant" ALTER COLUMN "UpdateTS" TYPE timestamp without time zone;
    ALTER TABLE "Participant" ALTER COLUMN "UpdateTS" DROP NOT NULL;
    ALTER TABLE "Participant" ALTER COLUMN "UpdateTS" DROP DEFAULT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180725151416_NullableUpdateTsAdded') THEN
    ALTER TABLE "Order" ALTER COLUMN "UpdateTS" TYPE timestamp without time zone;
    ALTER TABLE "Order" ALTER COLUMN "UpdateTS" DROP NOT NULL;
    ALTER TABLE "Order" ALTER COLUMN "UpdateTS" DROP DEFAULT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180725151416_NullableUpdateTsAdded') THEN
    ALTER TABLE "Federation" ALTER COLUMN "UpdateTs" TYPE timestamp without time zone;
    ALTER TABLE "Federation" ALTER COLUMN "UpdateTs" DROP NOT NULL;
    ALTER TABLE "Federation" ALTER COLUMN "UpdateTs" DROP DEFAULT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180725151416_NullableUpdateTsAdded') THEN
    ALTER TABLE "Event" ALTER COLUMN "UpdateTS" TYPE timestamp without time zone;
    ALTER TABLE "Event" ALTER COLUMN "UpdateTS" DROP NOT NULL;
    ALTER TABLE "Event" ALTER COLUMN "UpdateTS" DROP DEFAULT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180725151416_NullableUpdateTsAdded') THEN
    ALTER TABLE "AspNetUsers" ALTER COLUMN "UpdateTS" TYPE timestamp without time zone;
    ALTER TABLE "AspNetUsers" ALTER COLUMN "UpdateTS" DROP NOT NULL;
    ALTER TABLE "AspNetUsers" ALTER COLUMN "UpdateTS" DROP DEFAULT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180725151416_NullableUpdateTsAdded') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20180725151416_NullableUpdateTsAdded', '2.1.1-rtm-30846');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180731090310_OrderIdAddedToMembership') THEN
    ALTER TABLE "FederationMembership" DROP COLUMN "CreateBy";
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180731090310_OrderIdAddedToMembership') THEN
    ALTER TABLE "FederationMembership" ADD "OrderId" uuid NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180731090310_OrderIdAddedToMembership') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20180731090310_OrderIdAddedToMembership', '2.1.1-rtm-30846');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180807082135_WeightInStatusAdded') THEN
    ALTER TABLE "Participant" ADD "WeightInStatus" text NULL DEFAULT 'pending';
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180807082135_WeightInStatusAdded') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20180807082135_WeightInStatusAdded', '2.1.1-rtm-30846');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180810142839_TeamNullable') THEN
    ALTER TABLE "Participant" ALTER COLUMN "TeamId" TYPE uuid;
    ALTER TABLE "Participant" ALTER COLUMN "TeamId" DROP NOT NULL;
    ALTER TABLE "Participant" ALTER COLUMN "TeamId" DROP DEFAULT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180810142839_TeamNullable') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20180810142839_TeamNullable', '2.1.1-rtm-30846');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180813160640_TeamWithApprovalAdded') THEN
    ALTER TABLE "Team" RENAME COLUMN "CreateBy" TO "OwnerId";
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180813160640_TeamWithApprovalAdded') THEN
    ALTER TABLE "Federation" ALTER COLUMN "Name" TYPE varchar(200);
    ALTER TABLE "Federation" ALTER COLUMN "Name" DROP NOT NULL;
    ALTER TABLE "Federation" ALTER COLUMN "Name" DROP DEFAULT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180813160640_TeamWithApprovalAdded') THEN
    ALTER TABLE "Federation" ADD "CommissionPercentage" integer NOT NULL DEFAULT 0;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180813160640_TeamWithApprovalAdded') THEN
    ALTER TABLE "Federation" ADD "MinCommission" integer NOT NULL DEFAULT 0;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180813160640_TeamWithApprovalAdded') THEN
    ALTER TABLE "Event" ADD "PaymentType" text NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180813160640_TeamWithApprovalAdded') THEN
    ALTER TABLE "AspNetUsers" ADD "TeamId" uuid NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180813160640_TeamWithApprovalAdded') THEN
    ALTER TABLE "AspNetUsers" ADD "TeamMembershipApprovalStatus" text NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180813160640_TeamWithApprovalAdded') THEN
    CREATE INDEX "IX_AspNetUsers_TeamId" ON "AspNetUsers" ("TeamId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180813160640_TeamWithApprovalAdded') THEN
    ALTER TABLE "AspNetUsers" ADD CONSTRAINT "FK_AspNetUsers_Team_TeamId" FOREIGN KEY ("TeamId") REFERENCES "Team" ("TeamId") ON DELETE RESTRICT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180813160640_TeamWithApprovalAdded') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20180813160640_TeamWithApprovalAdded', '2.1.1-rtm-30846');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180816095719_FederationApprovalAdded') THEN
    ALTER TABLE "Team" ADD "FederationApprovalStatus" text NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180816095719_FederationApprovalAdded') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20180816095719_FederationApprovalAdded', '2.1.1-rtm-30846');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180820132307_IsActiveAdded') THEN
    ALTER TABLE "Team" ADD "IsActive" boolean NOT NULL DEFAULT TRUE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180820132307_IsActiveAdded') THEN
    ALTER TABLE "FederationMembership" ADD "IsActive" boolean NOT NULL DEFAULT TRUE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20180820132307_IsActiveAdded') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20180820132307_IsActiveAdded', '2.1.1-rtm-30846');
    END IF;
END $$;
