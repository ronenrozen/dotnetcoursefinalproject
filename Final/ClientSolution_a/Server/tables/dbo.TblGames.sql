CREATE TABLE [dbo].[TblGames] (
    [Id]   INT      IDENTITY (1, 1) NOT NULL,
    [Date] DATETIME NOT NULL,
    [PID]  INT      NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_TblGames_ToTblPlayers] FOREIGN KEY ([PID]) REFERENCES [dbo].[TblPlayers] ([Id]) ON DELETE CASCADE
);