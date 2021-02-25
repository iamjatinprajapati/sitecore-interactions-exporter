SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Interactions](
	[InteractionID] [uniqueidentifier] NOT NULL,
	[EngagementValue] [int] NULL,
	[Bounces] [int] NULL,
	[Conversions] [int] NULL,
	[Converted] [int] NULL,
	[TimeOnSite] [int] NULL,
	[PageViews] [int] NULL,
	[OutcomeOccurrences] [int] NULL,
	[MonetaryValue] [money] NULL,
	[ContactID] [uniqueidentifier] NULL,
	[CampaignID] [uniqueidentifier] NULL,
	[CamapignName] [nvarchar](500) NULL,
	[ChannelID] [uniqueidentifier] NULL,
	[ChannelName] [nvarchar](500) NULL,
	[RawValue] [nvarchar](max) NULL,
	[LastModified] [datetime] NULL,
	[StartDateTime] [datetime] NULL,
	[EndDateTime] [datetime] NULL,
	[UserAgent] [nvarchar](500) NULL,
	[Duration] [bigint] NULL,
	[Duration.Days] [int] NULL,
	[Duration.Hours] [int] NULL,
	[Duration.Minutes] [int] NULL,
	[Duration.Seconds] [int] NULL,
	[Duration.Milliseconds] [int] NULL,
	[UserAgentInfo.DeviceType] [nvarchar](max) NULL,
	[UserAgentInfo.DeviceVendor] [nvarchar](max) NULL,
	[UserAgentInfo.DeviceVendorHardwareModel] [nvarchar](max) NULL,
	[IpInfo.AreaCode] [nvarchar](500) NULL,
	[IpInfo.BusinessName] [nvarchar](500) NULL,
	[IpInfo.City] [nvarchar](500) NULL,
	[IpInfo.Country] [nvarchar](500) NULL,
	[IpInfo.Dns] [nvarchar](500) NULL,
	[IpInfo.IpAddress] [nvarchar](500) NULL,
	[IpInfo.Isp] [nvarchar](500) NULL,
	[IpInfo.Latitude] [decimal](18, 5) NULL,
	[IpInfo.Longitude] [decimal](18, 5) NULL,
	[IpInfo.MetroCode] [nvarchar](500) NULL,
	[IpInfo.PostalCode] [nvarchar](500) NULL,
	[IpInfo.Region] [nvarchar](500) NULL,
	[IpInfo.Url] [nvarchar](max) NULL,
	[WebVisit.Browser.BrowserMajorName] [nvarchar](500) NULL,
	[WebVisit.Browser.BrowserMinorName] [nvarchar](500) NULL,
	[WebVisit.Browser.BrowserVersion] [nvarchar](500) NULL,
	[WebVisit.Language] [nvarchar](50) NULL,
	[WebVisit.OperatingSystem.MajorVersion] [nvarchar](500) NULL,
	[WebVisit.OperatingSystem.MinorVersion] [nvarchar](500) NULL,
	[WebVisit.OperatingSystem.Name] [nvarchar](500) NULL,
	[WebVisit.Referrer] [nvarchar](max) NULL,
	[WebVisit.Screen.ScreenWidth] [int] NULL,
	[WebVisit.Screen.ScreenHeight] [int] NULL,
	[WebVisit.SearchKeywords] [nvarchar](max) NULL,
	[WebVisit.SiteName] [nvarchar](500) NULL,
	[UserAgentInfo.CanSupportTouchScreen] [bit] NULL,
	[WebsiteInfo.Domain] [nvarchar](500) NULL,
 CONSTRAINT [PK_Interactions] PRIMARY KEY CLUSTERED 
(
	[InteractionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
