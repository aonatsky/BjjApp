INSERT INTO public."Team"
("TeamId", "Name", "Description", "UpdateTs", "UpdateBy", "FederationId", "OrderId", "IsApproved")
VALUES('a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11','Alliance', '', now(), '7d761013-6465-47b0-9778-057604322390', '673ea3ce-2530-48c0-b84c-a3de492cab25', null, true);

select "FederationId" from "Federation"