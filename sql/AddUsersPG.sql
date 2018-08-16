INSERT into public."AspNetUsers"
    ("Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName", "DateOfBirth")
VALUES
    (N'a2070395-168a-49aa-972b-326edd47bf70', 0, N'809b75de-ce7b-4a66-b4cc-ff8ae303c3ad', N'admin', false, N'Ivan', N'Drago', true, NULL, N'ADMIN', N'ADMIN', N'AQAAAAEAACcQAAAAEOGcrW0OF0EzOc38l11oNC1y8SfFoMQOeqxgXnA44Jh5+L3fds3KgTzGADvUBc05uw==', NULL, false, N'ca0d2931-6c19-4c68-b855-13dad950e6f4', false, N'admin', CAST(N'0001-01-01T00:00:00.0000000' AS Date));
INSERT into public."AspNetUserClaims"
    ("Id", "ClaimType", "ClaimValue", "UserId")
VALUES
    (Select nextval(pg_get_serial_sequence('AspNetUserClaims', 'Id')) as new_id;, 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role', 'Owner', 'a2070395-168a-49aa-972b-326edd47bf70'),
    
INSERT into public."AspNetUsers"
    ("Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName", "DateOfBirth")
VALUES
    (N'd0aa12c1-af0c-4ce5-aa6c-ea43ba70aa65', 0, N'93121fb0-d8dd-4c79-b011-3fbcbabc8084', N'owner@t.com', false, N'owner', N'owner', true, NULL, N'OWNER', N'OWNER', N'AQAAAAEAACcQAAAAEOGcrW0OF0EzOc38l11oNC1y8SfFoMQOeqxgXnA44Jh5+L3fds3KgTzGADvUBc05uw==', NULL, false, N'ca0d2931-6c19-4c68-b855-13dad950e6f4', false, N'owner', CAST(N'0001-01-01T00:00:00.0000000' AS Date));
INSERT into public."AspNetUserClaims"
    ("Id", "ClaimType", "ClaimValue", "UserId")
VALUES
    (Select nextval(pg_get_serial_sequence('AspNetUserClaims', 'Id')) as new_id;, 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role', 'Owner', 'd0aa12c1-af0c-4ce5-aa6c-ea43ba70aa65'),

INSERT into public."AspNetUsers"
    ("Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName", "DateOfBirth")
VALUES
    (N'd0aa12c1-af0c-4ce5-aa6c-ea43ba70aa65', 0, N'93121fb0-d8dd-4c79-b011-3fbcbabc8084', N'federationowner@t.com', false, N'federationowner', N'federationowner', true, NULL, N'OWNER', N'OWNER', N'AQAAAAEAACcQAAAAEOGcrW0OF0EzOc38l11oNC1y8SfFoMQOeqxgXnA44Jh5+L3fds3KgTzGADvUBc05uw==', NULL, false, N'ca0d2931-6c19-4c68-b855-13dad950e6f4', false, N'federationowner', CAST(N'0001-01-01T00:00:00.0000000' AS Date));
INSERT into public."AspNetUserClaims"
    ("Id", "ClaimType", "ClaimValue", "UserId")
VALUES
    (Select nextval(pg_get_serial_sequence('AspNetUserClaims', 'Id')) as new_id;, 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role', 'Owner', 'd0aa12c1-af0c-4ce5-aa6c-ea43ba70aa65'),
