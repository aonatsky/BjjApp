INSERT into public."AspNetUsers"
    ("Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName", "DateOfBirth")
VALUES
    (N'a2070395-168a-49aa-972b-326edd47bf70', 0, N'809b75de-ce7b-4a66-b4cc-ff8ae303c3ad', N'Ivan.drago@trnmnt.com', false, N'Ivan', N'Drago', true, NULL, N'IVAN.DRAGO@TRNMNT.COM', N'ADMIN', N'AQAAAAEAACcQAAAAEOGcrW0OF0EzOc38l11oNC1y8SfFoMQOeqxgXnA44Jh5+L3fds3KgTzGADvUBc05uw==', NULL, false, N'ca0d2931-6c19-4c68-b855-13dad950e6f4', false, N'admin', CAST(N'0001-01-01T00:00:00.0000000' AS Date));
INSERT into public."AspNetUserClaims"
    ("Id", "ClaimType", "ClaimValue", "UserId")
VALUES
    (3, 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role', 'Owner', 'a2070395-168a-49aa-972b-326edd47bf70');
INSERT INTO public."Federation"
    ("FederationId", "ContactInformation", "Currency", "Description", "ImgPath", "MembershipPrice", "Name", "OwnerId", "UpdateTs", "TeamRegistrationPrice")
VALUES
    ('673ea3ce-2530-48c0-b84c-a3de492cab25', NULL, N'UAH', N'Ukrainian Federation Of Brazilian Jiu Jitsu', NULL, 5, N'UABJJF','a2070395-168a-49aa-972b-326edd47bf70', NOW(), 0);

