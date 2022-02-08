using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DBService.Utils
{
    public static class XmlUtility
    {
        public static List<string> ToUserList(string xml)
        {
            try
            {
                var doc = XDocument.Parse(xml);

                var items = (from r in doc.Root.Elements("user")
                             select r.Attribute("FullName").ToString()
                             ).ToList();
                             //select new User()
                             //{
                             //    FullName = (string)r.Attribute("FullName"),
                             //    Userid = (int)r.Attribute("id"),
                             //    UserLogin = (string)r.Attribute("UserName"),
                             //}).ToList();

                return items;
            }
            catch (Exception ex)
            {
                return new List<string>();
            }
        }

    }

}
