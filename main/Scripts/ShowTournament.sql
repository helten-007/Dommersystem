declare @id int
set @id = 171;

select * from Tournaments where Id=@id;
select * from Rounds where Tournament_Id=@id;
select * from Contestants where Tournament_Id=@id;
select * from RoundContestants where Round_Id in (select Id from Rounds where Tournament_Id=@id);
