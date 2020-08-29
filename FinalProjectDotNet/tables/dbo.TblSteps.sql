CREATE TABLE [dbo].[TblSteps] (
    [Id]     INT          IDENTITY (1, 1) NOT NULL,
    [Color]  VARCHAR (50) NOT NULL,
    [SrcRow] INT          NOT NULL,
    [SrcCol] INT          NOT NULL,
    [DstRow] INT          NOT NULL,
    [DstCol] INT          NOT NULL,
    [GameId] INT NOT NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

