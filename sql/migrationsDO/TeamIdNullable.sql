
alter table "Participant" alter column "TeamId" DROP NOT NULL;
alter table "Participant" DROP CONSTRAINT "FK_Participant_Team_TeamId";