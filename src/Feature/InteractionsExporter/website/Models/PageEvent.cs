using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SC.Interactions.Exporter.Models
{
  public class PageEvent
  {
    public string ContactId { get; set; }

    public string InteractionId { get; set; }

    public string ChannelId { get; set; }

    public DateTime Date { get; set; }

    public string PageId { get; set; }

    public string Language { get; set; }

    public string PageName
    {
      get;set;
    }

    public string ItemPath
    {
      get;set;
    }

    public Sitecore.Data.Items.Item Item { get; set; }

    public string PagePath
    {
      get; set;
    }

    public string Url { get; set; }

    private Dictionary<String, String> QueryStrings
    {
      get {
        var data = new Dictionary<string, string>();
        if (!String.IsNullOrEmpty(this.Url))
        {
          var queryPart = this.Url.ToLowerInvariant().Split("?".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Length > 1 ? this.Url.Split("?".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1] : String.Empty;
          foreach (var query in queryPart.Split("&".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
          {
            try
            {
              var split = query.Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
              data.Add(split[0], split[1]);
            }
            catch (Exception ex)
            {

            }
          }
        }
        return data;
      }
    }

    public string UTMCampaign
    {
      get {
        if (QueryStrings.ContainsKey("utm_campaign"))
        {
          return QueryStrings["utm_campaign"];
        }
        return string.Empty;
      }
    }

    public string Invsrc
    {
      get {
        if (QueryStrings.ContainsKey("invsrc"))
        {
          return QueryStrings["invsrc"];
        }
        return string.Empty;
      }
    }

    public string UTMSource
    {
      get {
        if (QueryStrings.ContainsKey("utm_source"))
        {
          return QueryStrings["utm_source"];
        }
        return string.Empty;
      }
    }

    public string UTMMedium
    {
      get {
        if (QueryStrings.ContainsKey("utm_medium"))
        {
          return QueryStrings["utm_medium"];
        }
        return string.Empty;
      }
    }

    public string UTMContent
    {
      get {
        if (QueryStrings.ContainsKey("utm_content"))
        {
          return QueryStrings["utm_content"];
        }
        return string.Empty;
      }
    }
  }
}