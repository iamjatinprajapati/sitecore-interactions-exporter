using Newtonsoft.Json;
using Sitecore.Analytics.Aggregation.Pipeline;
using Sitecore.ExperienceAnalytics.Aggregation.Calculator;
using Sitecore.XConnect;
using Sitecore.XConnect.Client.Serialization;
using Sitecore.XConnect.Collection.Model;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace SC.Interactions.Exporter.Pipelines.InteractionsBatch
{
  public class ExportInteraction
  {
    public void Process(InteractionBatchAggregationPipelineArgs args)
    {
      if(!Sitecore.Configuration.Settings.GetBoolSetting("SC.Interactions.Exporter.EnableXDBDataExport", false))
      {
        Sitecore.Diagnostics.Log.Debug($"{this} xDB Data Export is disabled.", this);
        return;
      }
      try
      {
        Sitecore.Diagnostics.Log.Debug($"{typeof(ExportInteraction).FullName} Starting Data Export", this);
        var interactionsTable = new DataTable {
          TableName = "Interactions"
        };
        var pageEventsTable = new DataTable {
          TableName = "PageViewEvents"
        };
        var goalsTable = new DataTable {
          TableName = "Goals"
        };
        using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["xDBInteractionDatabase"].ConnectionString))
        {
          using (var adapter = new SqlDataAdapter())
          {
            var selectCommand = new SqlCommand("Select Top 0 * from Interactions", connection);
            adapter.SelectCommand = selectCommand;
            Sitecore.Diagnostics.Log.Debug($"{typeof(ExportInteraction).FullName} Filing Interactions table schema from database.", this);
            adapter.Fill(interactionsTable);

            Sitecore.Diagnostics.Log.Debug($"{typeof(ExportInteraction).FullName} Filing PageViewEvents table schema from database.", this);
            selectCommand.CommandText = "Select Top 0 * from PageViewEvents";
            adapter.Fill(pageEventsTable);

            Sitecore.Diagnostics.Log.Debug($"{typeof(ExportInteraction).FullName} Filing Goals table schema from database.", this);
            selectCommand.CommandText = "Select Top 0 * from Goals";
            adapter.Fill(goalsTable);
          }
        }
        
        var client = Sitecore.XConnect.Client.Configuration.SitecoreXConnectClientConfiguration.GetClient();
        var serializerSettings = new JsonSerializerSettings {
          ContractResolver = new XdbJsonContractResolver(client.Model, serializeFacets: true, serializeContactInteractions: true),
          Formatting = Formatting.Indented,
          DateTimeZoneHandling = DateTimeZoneHandling.Utc,
          DefaultValueHandling = DefaultValueHandling.Ignore
        };
        foreach (var context in args.Contexts)
        {
          var interaction = context.Interaction;
          ExtractInteractionData(interactionsTable, serializerSettings, interaction);
          ExtractPageEvents(pageEventsTable, interaction);
          ExtractGoals(goalsTable, interaction);
        }
        
        using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["xDBInteractionDatabase"].ConnectionString))
        {
          connection.Open();
          using (var bulk = new SqlBulkCopy(connection))
          {
            if (interactionsTable.Rows.Count > 0)
            {
              bulk.DestinationTableName = "Interactions";
              bulk.WriteToServer(interactionsTable);
            }

            if (pageEventsTable.Rows.Count > 0)
            {
              bulk.DestinationTableName = "PageViewEvents";
              bulk.WriteToServer(pageEventsTable);
            }

            if(goalsTable.Rows.Count > 0)
            {
              bulk.DestinationTableName = "Goals";
              bulk.WriteToServer(goalsTable);
            }
          }
          connection.Close();
        }
        
        Sitecore.Diagnostics.Log.Debug($"{typeof(ExportInteraction).FullName} Finished Data Export", this);
      }
      catch (Exception ex)
      {
        Sitecore.Diagnostics.Log.Error($"{typeof(ExportInteraction).FullName} {ex.Message} {ex.StackTrace}", ex, this);
      }
    }

    private void ExtractInteractionData(DataTable interactionTables, JsonSerializerSettings serializerSettings, Sitecore.XConnect.Interaction interaction)
    {
      Sitecore.Diagnostics.Log.Debug($"{typeof(ExportInteraction).FullName} Extracting interaction data for {interaction.Id}", this);
      var calculatedInteraction = new DefaultInteractionCalculator().Calculate(interaction);
      var ipInfo = interaction.IpInfo();
      var userAgentInfo = interaction.UserAgentInfo();
      var webVisitInfo = interaction.WebVisit();

      var row = interactionTables.NewRow();
      row["InteractionID"] = interaction.Id.Value;
      row["EngagementValue"] = calculatedInteraction.EngagementValue;
      row["Bounces"] = calculatedInteraction.Bounces;
      row["Conversions"] = calculatedInteraction.Conversions;
      row["Converted"] = calculatedInteraction.Converted;
      row["TimeOnSite"] = calculatedInteraction.TimeOnSite;
      row["PageViews"] = calculatedInteraction.PageViews;
      row["OutcomeOccurrences"] = calculatedInteraction.OutcomeOccurrences;
      row["MonetaryValue"] = calculatedInteraction.MonetaryValue;
      row["ContactID"] = interaction.Contact.Id.Value;
      row["UserAgent"] = interaction.UserAgent;
      row["LastModified"] = interaction.LastModified;
      row["StartDateTime"] = interaction.StartDateTime;
      row["EndDateTime"] = interaction.EndDateTime;
      row["Duration"] = interaction.Duration.Ticks;
      row["Duration.Days"] = interaction.Duration.Days;
      row["Duration.Hours"] = interaction.Duration.Hours;
      row["Duration.Minutes"] = interaction.Duration.Minutes;
      row["Duration.Seconds"] = interaction.Duration.Seconds;
      row["Duration.Milliseconds"] = interaction.Duration.Milliseconds;

      if (interaction.VenueId.HasValue)
      {
        row["VenueId"] = interaction.VenueId;
      }
      if (interaction.CampaignId.HasValue)
      {
        row["CampaignID"] = interaction.CampaignId.Value;
        row["CampaignName"] = GetItem(interaction.CampaignId.Value.ToString())?.Name;
      }
      row["ChannelID"] = interaction.ChannelId;
      row["ChannelName"] = GetItem(interaction.ChannelId.ToString())?.Name;

      //User agent info
      row["UserAgentInfo.CanSupportTouchScreen"] = userAgentInfo.CanSupportTouchScreen;
      row["UserAgentInfo.DeviceType"] = userAgentInfo.DeviceType;
      row["UserAgentInfo.DeviceVendor"] = userAgentInfo.DeviceVendor;
      row["UserAgentInfo.DeviceVendorHardwareModel"] = userAgentInfo.DeviceVendorHardwareModel;

      //Ip info
      row["IpInfo.AreaCode"] = ipInfo.AreaCode;
      row["IpInfo.BusinessName"] = ipInfo.BusinessName;
      row["IpInfo.City"] = ipInfo.City;
      row["IpInfo.Country"] = ipInfo.Country;
      row["IpInfo.Dns"] = ipInfo.Dns;
      row["IpInfo.IpAddress"] = ipInfo.IpAddress;
      row["IpInfo.Isp"] = ipInfo.Isp;
      if (ipInfo.Latitude.HasValue)
      {
        row["IpInfo.Latitude"] = ipInfo.Latitude;
      }
      if (ipInfo.Longitude.HasValue)
      {
        row["IpInfo.Longitude"] = ipInfo.Longitude;
      }

      row["IpInfo.MetroCode"] = ipInfo.MetroCode;
      row["IpInfo.PostalCode"] = ipInfo.PostalCode;
      row["IpInfo.Region"] = ipInfo.Region;
      row["IpInfo.Url"] = ipInfo.Url;

      //Webvisit
      row["WebVisit.Browser.BrowserMajorName"] = webVisitInfo.Browser.BrowserMajorName;
      row["WebVisit.Browser.BrowserMinorName"] = webVisitInfo.Browser.BrowserMinorName;
      row["WebVisit.Browser.BrowserVersion"] = webVisitInfo.Browser.BrowserVersion;
      row["WebVisit.Language"] = webVisitInfo.Language;
      row["WebVisit.OperatingSystem.MajorVersion"] = webVisitInfo.OperatingSystem.MajorVersion;
      row["WebVisit.OperatingSystem.MinorVersion"] = webVisitInfo.OperatingSystem.MinorVersion;
      row["WebVisit.OperatingSystem.Name"] = webVisitInfo.OperatingSystem.Name;
      row["WebVisit.Referrer"] = webVisitInfo.Referrer;
      row["WebVisit.Screen.ScreenWidth"] = webVisitInfo.Screen.ScreenWidth;
      row["WebVisit.Screen.ScreenHeight"] = webVisitInfo.Screen.ScreenHeight;
      row["WebVisit.SearchKeywords"] = webVisitInfo.SearchKeywords;
      row["WebVisit.SiteName"] = webVisitInfo.SiteName;

      row["RawValue"] = JsonConvert.SerializeObject(interaction.Events, serializerSettings);
      interactionTables.Rows.Add(row);
      Sitecore.Diagnostics.Log.Debug($"{typeof(ExportInteraction).FullName} Successfully extracted interaction data for {interaction.Id}", this);
    }
  
    private void ExtractPageEvents(DataTable pageViewEventsTable, Sitecore.XConnect.Interaction interaction)
    {
      Sitecore.Diagnostics.Log.Debug($"{typeof(ExportInteraction).FullName} Extracting page view events data for {interaction.Id}", this);
      var pageViewEvents = interaction.Events.OfType<PageViewEvent>().OrderBy(e => e.Timestamp).ToList();
      foreach(var pageViewEvent in pageViewEvents)
      {
        var row = pageViewEventsTable.NewRow();
        var item = GetItem(pageViewEvent.ItemId.ToString());
        row["ID"] = pageViewEvent.Id;
        if (pageViewEvent.ParentEventId.HasValue)
        {
          row["ParentEventID"] = pageViewEvent.ParentEventId.Value;
        }
        row["DefinitionID"] = pageViewEvent.DefinitionId;
        row["InteractionID"] = interaction.Id.Value;
        row["ItemID"] = pageViewEvent.ItemId;
        row["ItemName"] = item?.Name;
        row["ItemPath"] = item?.Paths.FullPath;
        row["EngagementValue"] = pageViewEvent.EngagementValue;
        row["Timestamp"] = pageViewEvent.Timestamp;
        row["Duration"] = pageViewEvent.Duration.Ticks;
        row["Duration.Days"] = pageViewEvent.Duration.Days;
        row["Duration.Hours"] = pageViewEvent.Duration.Hours;
        row["Duration.Minutes"] = pageViewEvent.Duration.Minutes;
        row["Duration.Seconds"] = pageViewEvent.Duration.Seconds;
        row["Duration.Milliseconds"] = pageViewEvent.Duration.Milliseconds;
        row["ItemLanguage"] = pageViewEvent.ItemLanguage;
        row["Url"] = pageViewEvent.Url;
        row["UTMCampaign"] = GetQueryStringValue(pageViewEvent.Url, "utm_campaign");
        row["UTMSource"] = GetQueryStringValue(pageViewEvent.Url, "utm_source");
        row["UTMMedium"] = GetQueryStringValue(pageViewEvent.Url, "utm_medium");
        row["invsrc"] = GetQueryStringValue(pageViewEvent.Url, "invsrc");
        row["UTMContent"] = GetQueryStringValue(pageViewEvent.Url, "utm_content");
        row["SitecoreRenderingDeviceID"] = pageViewEvent.SitecoreRenderingDevice.Id;
        row["SitecoreRenderingDevice.Name"] = pageViewEvent.SitecoreRenderingDevice.Name;
        pageViewEventsTable.Rows.Add(row);
      }
    }

    private void ExtractGoals(DataTable goalsTable, Sitecore.XConnect.Interaction interaction)
    {
      Sitecore.Diagnostics.Log.Debug($"{typeof(ExportInteraction).FullName} Extracting page view events data for {interaction.Id}", this);
      var goals = interaction.Events.OfType<Goal>().OrderBy(e => e.Timestamp).ToList();
      foreach (var goal in goals)
      {
        var row = goalsTable.NewRow();
        row["InteractionID"] = interaction.Id.Value;
        row["ItemID"] = goal.DefinitionId;  //Is the actual goal ID which we see in CMS
        row["GoalName"] = GetItem(goal.DefinitionId.ToString())?.Name;
        row["EngagementValue"] = goal.EngagementValue;
        row["Timestamp"] = goal.Timestamp;
        row["Duration"] = goal.Duration.Ticks;
        row["Duration.Days"] = goal.Duration.Days;
        row["Duration.Hours"] = goal.Duration.Hours;
        row["Duration.Minutes"] = goal.Duration.Minutes;
        row["Duration.Seconds"] = goal.Duration.Seconds;
        row["Duration.Milliseconds"] = goal.Duration.Milliseconds;
        row["Data"] = goal.Data;
        row["DataKey"] = goal.DataKey;
        row["Text"] = goal.Text;
        if (goal.ParentEventId.HasValue)
        {
          row["ParentEventID"] = goal.ParentEventId.Value;
        }
        row["DefinitionID"] = goal.DefinitionId;
        goalsTable.Rows.Add(row);
      }
    }

    private String GetQueryStringValue(String url, String queryParamterName)
    {
      var urlParts = url.ToLowerInvariant().Split(new char[] { '?' });
      if (urlParts.Length > 1)
      {
        var queryParamters = Sitecore.Web.WebUtil.ParseQueryString(urlParts[1]);
        if (queryParamters.ContainsKey(queryParamterName))
        {
          return queryParamters[queryParamterName];
        }
      }
      
      return String.Empty;
    }

    private Sitecore.Data.Database GetMasterDatabase()
    {
      return Sitecore.Configuration.Factory.GetDatabase("master");
    }

    private Sitecore.Data.Items.Item GetItem(String id)
    {
      return GetMasterDatabase().GetItem(Sitecore.Data.ID.Parse(id));
    }
  }
}