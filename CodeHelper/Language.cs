using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Xml;

namespace Maticsoft.CodeHelper
{
    public static class Language
    {
                               
        #region LoadFromCfg
        public static Hashtable LoadFromCfg(string filename)
        {
            try
            {
                string LanguageSet = Maticsoft.CmConfig.AppConfig.GetSettings().Language;
                string cfgPath = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\language\\" + LanguageSet + "\\" + filename;

                Hashtable lanlist = new Hashtable();

                XmlDocument doc = new System.Xml.XmlDocument();
                doc.Load(cfgPath);
                XmlElement root = doc.DocumentElement;
                foreach (XmlNode node in root.ChildNodes)
                {
                    lanlist.Add(node.Attributes["key"].Value, node.Attributes["value"].Value);
                }

                #region
                //XmlNode n1;                
                //bool support_from_to = false;
                //n1 = root.SelectSingleNode("support_from_to");
                //if (!Boolean.TryParse(n1.InnerText, out support_from_to))
                //{
                //    support_from_to = false;
                //}
                //cfg.Support_From_To = support_from_to;
                //n1 = root.SelectSingleNode("from");
                //DateTime from = DateTime.Now.Date.AddDays(-1);
                //if (!DateTime.TryParse(n1.InnerText, out from))
                //{
                //    from = DateTime.Now.Date.AddDays(-1);
                //}
                //cfg.From = from;
                //n1 = root.SelectSingleNode("to");
                //DateTime to = DateTime.Now.Date.AddDays(-1);
                //if (!DateTime.TryParse(n1.InnerText, out to))
                //{
                //    to = DateTime.Now.Date.AddDays(-1);
                //}
                //cfg.To = to;
                //if (cfg.To < cfg.From)
                //{
                //    cfg.To = cfg.From;
                //}
                //cfg.Support_From_To = support_from_to;
                //n1 = root.SelectSingleNode("last");
                //DateTime last = DateTime.Now.Date.AddDays(-1);
                //if (!DateTime.TryParse(n1.InnerText, out last))
                //{
                //    last = DateTime.Now.Date.AddDays(-1);
                //}
                //cfg.Last = last;
                #endregion

                return lanlist;
            }
            catch(System.Exception ex)
            {
                throw new Exception("Load language file fail:" + ex.Message);
            }
        }

        #endregion
    }
}
