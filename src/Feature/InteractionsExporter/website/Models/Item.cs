using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SC.Interactions.Exporter.Models
{
  public class Item
  {
    public String ItemID { get; set; }

    public String ItemName { get; set; }

    public String ItemPath { get; set; }

    public String ParentID { get; set; }

    public String TemplateID { get; set; }

    public String TemplateName { get; set; }

    public String CloneSource { get; set; }

    public String ItemLanguage { get; set; }
  }
}