using ConsoleTest.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RedisImpl.Properties;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace ConsoleTest
{
    public class WebApi
    {
        /// <summary>
        ///  查询一条记录
        /// </summary>
        /// <param name="guid">数据唯一id</param>
        /// <param name="entityName">实体名</param>
        /// <param name="xName">
        ///第三个参数可以是空
        ///第三个参数可以是?$select=name等取具体属性
        ///第三个参数可以是？$expand=contact_customer_accounts($select=name)来取主实体属性字段及所关联的N实体记录，schemaName为1：N关系的架构名称
        ///第三个参数可以是?$expand=Account_Tasks($filter=endswith(subject,'1');$select=subject)，筛选符合条件的N实体记录</param>
        /// <returns></returns>
        public static string RetrieveRecord(string guid, string entityName, string xName)
        {
            string result = "";
            string domain;
            string username;
            string pwd;
            string orgurl;
            string weburi;

            switch (Constant.EnvironmentType)
            {
                case EnvironmentType.Dev:
                    {
                        domain = Settings.Default.Domain;
                        username = Settings.Default.UserName;
                        pwd = Settings.Default.PassWord;
                        orgurl = Settings.Default.OrganizationUriDev;
                        weburi = Settings.Default.WebUriDev + entityName + "(" + guid.Replace("{", "").Replace("}", "") + ")";
                        break;
                    }
                case EnvironmentType.Test:
                    {
                        domain = Settings.Default.DomainTest;
                        username = Settings.Default.UserNameTest;
                        pwd = Settings.Default.PassWordTest;
                        orgurl = Settings.Default.OrganizationUriTest;
                        weburi = Settings.Default.WebUriTest + entityName + "(" + guid.Replace("{", "").Replace("}", "") + ")";
                        break;
                    }
                case EnvironmentType.Official:
                    {
                        domain = Settings.Default.Domain;
                        username = Settings.Default.UserName;
                        pwd = Settings.Default.PassWord;
                        orgurl = Settings.Default.OrganizationUriOfficial;
                        weburi = Settings.Default.WebUriOfficial + entityName + "(" + guid.Replace("{", "").Replace("}", "") + ")";
                        break;
                    }
                default:
                    {
                        // 默认连接开发环境
                        domain = Settings.Default.Domain;
                        username = Settings.Default.UserName;
                        pwd = Settings.Default.PassWord;
                        orgurl = Settings.Default.OrganizationUriDev;
                        weburi = Settings.Default.WebUriDev + entityName + "(" + guid.Replace("{", "").Replace("}", "") + ")";
                        break;
                    }
            }

            weburi = Settings.Default.WebUriDev + entityName + "(" + guid.Replace("{", "").Replace("}", "") + ")";
            if (!string.IsNullOrEmpty(xName))
            {
                weburi = weburi + xName;
            }
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(weburi);
            req.Credentials = new NetworkCredential(username, pwd, domain);
            req.Method = "Get";
            req.Accept = "application/json";
            req.ContentType = "application/json; charset=utf-8";
            req.Headers.Set("OData-MaxVersion", "4.0");
            req.Headers.Set("OData-Version", "4.0");
            using (HttpWebResponse res = (HttpWebResponse)req.GetResponse())
            {
                StreamReader read = new StreamReader(res.GetResponseStream());
                result = read.ReadToEnd();
            }

            return result;
        }

        /// <summary>
        /// 查询单个属性字段值
        /// </summary>
        /// <param name="guid">实体guid</param>
        /// <param name="entityName">实体名</param>
        /// <param name="propertyName">属性名</param>
        /// <returns>返回属性的字符串</returns>
        public static string RetrieveOneProperty(string guid, string entityName, string propertyName)
        {
            string result = "";
            string domain = Settings.Default.DomainTest;
            string username = Settings.Default.UserNameTest;
            string pwd = Settings.Default.PassWordTest;
            string weburi = Settings.Default.WebUriDev + entityName + "(" + guid.Replace("{", "").Replace("}", "") + ")/" + propertyName;

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(weburi);
            req.Credentials = new NetworkCredential(username, pwd, domain);
            req.Method = "Get";
            req.Accept = "application/json";
            req.ContentType = "application/json; charset=utf-8";
            req.Headers.Set("OData-MaxVersion", "4.0");
            req.Headers.Set("OData-Version", "4.0");
            using (HttpWebResponse res = (HttpWebResponse)req.GetResponse())
            {
                StreamReader read = new StreamReader(res.GetResponseStream());
                result = read.ReadToEnd();
            }

            if (result != "")
            {
                result = ((JValue)JsonConvert.DeserializeObject<JObject>(result)["value"]).Value.ToString();
            }

            return result;
        }

        /// <summary>
        /// 使用fileter查询多条数据
        /// </summary>
        /// <param name="entityName">实体名</param>
        /// <param name="filter">查询条件，写法见odata4.0的语法</param>
        /// <returns></returns>
        public static string RetrieveMultipleByFilter(string entityName, string filter)
        {
            string result = "";
            string domain = Settings.Default.Domain;
            string username = Settings.Default.UserName;
            string pwd = Settings.Default.PassWord;
            string weburi = Settings.Default.WebUriDev + entityName + filter;

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(weburi);
            req.Credentials = new NetworkCredential(username, pwd, domain);
            req.Method = "Get";
            req.Accept = "application/json";
            req.ContentType = "application/json; charset=utf-8";
            req.Headers.Set("OData-MaxVersion", "4.0");
            req.Headers.Set("OData-Version", "4.0");
            using (HttpWebResponse res = (HttpWebResponse)req.GetResponse())
            {
                StreamReader read = new StreamReader(res.GetResponseStream());
                result = read.ReadToEnd();
            }
            return result;
        }

        /// <summary>
        /// 使用fetchxml查询数据
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="fetchxml"></param>
        /// <returns></returns>
        public static string RetrieveRecordByFechXml(string entityName,string fetchxml)
        {
            string result = "";
            string domain = Settings.Default.Domain;
            string username = Settings.Default.UserName;
            string pwd = Settings.Default.PassWord;
            string weburi = Settings.Default.WebUriDev + entityName + "?fetchXml=" + Uri.EscapeUriString(fetchxml);

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(weburi);
            req.Credentials = new NetworkCredential(username, pwd, domain);
            req.Method = "Get";
            req.Accept = "application/json";
            req.ContentType = "application/json; charset=utf-8";
            req.Headers.Set("OData-MaxVersion", "4.0");
            req.Headers.Set("OData-Version", "4.0");
            using (HttpWebResponse res = (HttpWebResponse)req.GetResponse())
            {
                StreamReader read = new StreamReader(res.GetResponseStream());
                result = read.ReadToEnd();
            }
            return result;
        }

        /// <summary>
        /// 创建记录
        /// </summary>
        /// <param name="entityobject">实体属性集json字符串</param>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public static Guid CreateRecord(string entityobject, string entityName)
        {
            Guid guid = Guid.Empty;
            string domain = Settings.Default.Domain;
            string username = Settings.Default.UserName;
            string pwd = Settings.Default.PassWord;
            string weburi = Settings.Default.WebUriDev + entityName;

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(weburi);
            req.Credentials = new NetworkCredential(username, pwd, domain);
            req.Method = "post";
            req.Accept = "application/json";
            req.ContentType = "application/json; charset=utf-8";
            req.Headers.Add("OData-MaxVersion", "4.0");
            req.Headers.Add("OData-Version", "4.0");
            //req.Headers.Add("If-Match", "*");
            //ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] data = Encoding.UTF8.GetBytes(entityobject);
            Stream newStream = req.GetRequestStream();
            newStream.Write(data, 0, data.Length);
            newStream.Close();
            using (HttpWebResponse res = (HttpWebResponse)req.GetResponse())
            {
                //StreamReader read = new StreamReader(res.GetResponseStream());
                string head = res.Headers.ToString();
                string entityId = head.Substring(head.IndexOf("OData-EntityId"), head.IndexOf("Persistent-Auth") - head.IndexOf("OData-EntityId"));
                guid = new Guid(entityId.Substring(entityId.IndexOf("(") + 1, entityId.IndexOf(")") - entityId.IndexOf("(") - 1));

            }
            return guid;
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="guid">实体guid</param>
        /// <param name="entityObject">需要更新的实体属性集json字符串</param>
        /// <param name="entityName">实体名</param>
        public static void UpdateRecord(string guid, string entityObject, string entityName)
        {
            string domain = Settings.Default.Domain;
            string username = Settings.Default.UserName;
            string pwd = Settings.Default.PassWord;
            string weburi = Settings.Default.WebUriDev + entityName + "(" + guid.Replace("{", "").Replace("}", "") + ")";

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(weburi);
            req.Credentials = new NetworkCredential(username, pwd, domain);
            req.Method = "PATCH";
            req.Accept = "application/json";
            req.ContentType = "application/json; charset=utf-8";
            req.Headers.Add("OData-MaxVersion", "4.0");
            req.Headers.Add("OData-Version", "4.0");
            req.Headers.Add("If-Match", "*");
            //ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] data = Encoding.UTF8.GetBytes(entityObject);
            Stream newStream = req.GetRequestStream();
            newStream.Write(data, 0, data.Length);
            newStream.Close();
            using (HttpWebResponse res = (HttpWebResponse)req.GetResponse())
            {
                //StreamReader read = new StreamReader(res.GetResponseStream());
                string head = res.Headers.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="entityName"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        public static void UpdateRecordOneProperty(string guid, string entityName, string propertyName, string propertyValue)
        {
            string domain = Settings.Default.Domain;
            string username = Settings.Default.UserName;
            string pwd = Settings.Default.PassWord;
            string weburi = Settings.Default.WebUriDev + entityName + "(" + guid.Replace("{", "").Replace("}", "") + ")";

            propertyValue ="{\"value\":\"" + propertyValue + "\"}";
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(weburi);
            req.Credentials = new NetworkCredential(username, pwd, domain);
            req.Method = "PUT";
            req.Accept = "application/json";
            req.ContentType = "application/json; charset=utf-8";
            req.Headers.Add("OData-MaxVersion", "4.0");
            req.Headers.Add("OData-Version", "4.0");
            //req.Headers.Add("If-Match", "*");
            //ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] data = Encoding.UTF8.GetBytes(propertyValue);
            Stream newStream = req.GetRequestStream();
            newStream.Write(data, 0, data.Length);
            newStream.Close();
            using (HttpWebResponse res = (HttpWebResponse)req.GetResponse())
            {
                //StreamReader read = new StreamReader(res.GetResponseStream());
                string head = res.Headers.ToString();
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="entityName"></param>
        public static void DeleteRecord(string guid, string entityName)
        {
            string domain = Settings.Default.Domain;
            string username = Settings.Default.UserName;
            string pwd = Settings.Default.PassWord;
            string weburi = Settings.Default.WebUriDev + entityName + "(" + guid.Replace("{", "").Replace("}", "") + ")";

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(weburi);
            req.Credentials = new NetworkCredential(username, pwd, domain);
            req.Method = "delete";
            req.Accept = "application/json";
            req.ContentType = "application/json; charset=utf-8";
            req.Headers.Add("OData-MaxVersion", "4.0");
            req.Headers.Add("OData-Version", "4.0");

            using (HttpWebResponse res = (HttpWebResponse)req.GetResponse())
            {
                //StreamReader read = new StreamReader(res.GetResponseStream());
                string head = res.Headers.ToString();
            }
        }

        /// <summary>
        /// 删除单个属性值
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="entityName"></param>
        /// <param name="propertyName"></param>
        public static void DeleteOneProperty(string guid,string  entityName, string propertyName)
        {
            string domain = Settings.Default.Domain;
            string username = Settings.Default.UserName;
            string pwd = Settings.Default.PassWord;
            string weburi = Settings.Default.WebUriDev + entityName + "(" + guid.Replace("{", "").Replace("}", "") + ")";

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(weburi);
            req.Credentials = new NetworkCredential(username, pwd, domain);
            req.Method = "DELETE";
            //req.Accept = "application/json";
            req.ContentType = "application/json; charset=utf-8";
            req.Headers.Add("OData-MaxVersion", "4.0");
            req.Headers.Add("OData-Version", "4.0");

            using (HttpWebResponse res = (HttpWebResponse)req.GetResponse())
            {
                //StreamReader read = new StreamReader(res.GetResponseStream());
                string head = res.Headers.ToString();
            }
        }
    }
}