SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Goals](
	[ID] [uniqueidentifier] NOT NULL,
	[InteractionID] [uniqueidentifier] NOT NULL,
	[ItemID] [uniqueidentifier] NULL,
	[GoalName] [nvarchar](500) NULL,
	[Data] [nvarchar](max) NULL,
	[DataKey] [nvarchar](max) NULL,
	[Text] [nvarchar](max) NULL,
	[Timestamp] [datetime] NULL,
	[Duration] [bigint] NULL,
	[Duration.Days] [int] NULL,
	[Duration.Hours] [int] NULL,
	[Duration.Minutes] [int] NULL,
	[Duration.Seconds] [int] NULL,
	[Duration.Milliseconds] [int] NULL,
	[EngagementValue] [int] NULL,
	[ParentEventID] [uniqueidentifier] NULL,
	[DefinitionID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Goals] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Goals]  WITH NOCHECK ADD  CONSTRAINT [FK_Goals_Interactions] FOREIGN KEY([InteractionID])
REFERENCES [dbo].[Interactions] ([InteractionID])
GO

ALTER TABLE [dbo].[Goals] CHECK CONSTRAINT [FK_Goals_Interactions]
GO
