SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PageViewEvents](
	[ID] [uniqueidentifier] NOT NULL,
	[InteractionID] [uniqueidentifier] NOT NULL,
	[ItemID] [uniqueidentifier] NULL,
	[ItemName] [nvarchar](max) NULL,
	[EngagementValue] [int] NULL,
	[Timestamp] [datetime] NULL,
	[Duration] [bigint] NULL,
	[Duration.Days] [int] NULL,
	[Duration.Hours] [int] NULL,
	[Duration.Minutes] [int] NULL,
	[Duration.Seconds] [int] NULL,
	[Duration.Milliseconds] [int] NULL,
	[ItemLanguage] [nvarchar](50) NULL,
	[Url] [nvarchar](max) NULL,
	[SitecoreRenderingDeviceID] [uniqueidentifier] NULL,
	[SitecoreRenderingDevice.Name] [nvarchar](50) NULL,
	[ItemPath] [nvarchar](max) NULL,
	[UTMCampaign] [nvarchar](500) NULL,
	[UTMSource] [nvarchar](500) NULL,
	[UTMMedium] [nvarchar](500) NULL,
	[invsrc] [nvarchar](500) NULL,
	[UTMContent] [nvarchar](500) NULL,
	[ParentEventID] [uniqueidentifier] NULL,
	[DefinitionID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_PageViewEvents] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[PageViewEvents]  WITH NOCHECK ADD  CONSTRAINT [FK_PageViewEvents_Interactions] FOREIGN KEY([InteractionID])
REFERENCES [dbo].[Interactions] ([InteractionID])
GO

ALTER TABLE [dbo].[PageViewEvents] CHECK CONSTRAINT [FK_PageViewEvents_Interactions]
GO
