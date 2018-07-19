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
