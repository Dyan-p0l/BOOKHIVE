USE [bookhiveDB]
GO

SELECT [UserID]
      ,[FullName]
      ,[Username]
      ,[Password]
      ,[UserType]
  FROM [dbo].[Users]

GO


SELECT * INTO UsersBackup FROM Users;