using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Description;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using RedisImpl.Properties;
using ConsoleTest.Util;

namespace ConsoleTest
{
    public enum EnvironmentType
    {
        Dev,
        Test,
        YW,
        Official
    }

    public class OrganizationService
    {
        private volatile static IOrganizationService _service = null;

        private static readonly Object Locker = new Object();

        private OrganizationService() { }

        public static IOrganizationService CreateOrganizationService()
        {
            if(_service == null)
            {
                lock(Locker)
                {
                    if(_service == null)
                    {
                        _service = GetOrganizationService();
                    }
                }
            }

            return _service;
        }

        private static IOrganizationService GetOrganizationService()
        {
            string domain;
            string username;
            string pwd;
            string orgurl;

            switch (Constant.EnvironmentType)
            {
                case EnvironmentType.Dev: 
                    {
                        domain = Settings.Default.DomainTest;
                        username = Settings.Default.UserNameTest;
                        pwd = Settings.Default.PassWordTest;
                        orgurl = Settings.Default.OrganizationUriDev;
                        break;
                    }
                case EnvironmentType.Test:
                    {
                        domain = Settings.Default.DomainTest;
                        username = Settings.Default.UserNameTest;
                        pwd = Settings.Default.PassWordTest;
                        orgurl = Settings.Default.OrganizationUriTest;
                        break;
                    }
                case EnvironmentType.Official:
                    {
                        domain = Settings.Default.Domain;
                        username = Settings.Default.UserName;
                        pwd = Settings.Default.PassWord;
                        orgurl = Settings.Default.OrganizationUriOfficial;
                        break;
                    }
                case EnvironmentType.YW:
                    {
                        domain = Settings.Default.DomainYW;
                        username = Settings.Default.UserNameYW;
                        pwd = Settings.Default.PassWordYW;
                        orgurl = Settings.Default.OrganizationUriYW;
                        break;
                    }
                default:
                    {
                        // 默认连接开发环境
                        domain = Settings.Default.Domain;
                        username = Settings.Default.UserName;
                        pwd = Settings.Default.PassWord;
                        orgurl = Settings.Default.OrganizationUriDev;
                        break;
                    }
            }

            // Validate server certification.
            ServicePointManager.ServerCertificateValidationCallback = RemoteCertificateValidationCallback;

            // Get the _service management.
            IServiceManagement<IOrganizationService> orgServiceManagement =
                        ServiceConfigurationFactory.CreateManagement<IOrganizationService>(
                        new Uri(orgurl));

            // Set the credentials.
            AuthenticationCredentials authCredentials = new AuthenticationCredentials();
            authCredentials.ClientCredentials.Windows.ClientCredential =
                        new System.Net.NetworkCredential(username,
                            pwd,
                            domain);

            // Get the organization _service proxy.
            OrganizationServiceProxy organizationProxy =
                        GetProxy<IOrganizationService, OrganizationServiceProxy>(orgServiceManagement, authCredentials);

            // This statement is required to enable early-bound type support.
            organizationProxy.EnableProxyTypes();

            return organizationProxy;
        }

        private static TProxy GetProxy<TService, TProxy>(
            IServiceManagement<TService> serviceManagement,
            AuthenticationCredentials authCredentials)
            where TService : class
            where TProxy : ServiceProxy<TService>
        {
            Type classType = typeof(TProxy);

            // Obtain discovery/organization _service proxy for ActiveDirectory environment.
            // Instantiate a new class of type using the 2 parameter constructor of type IServiceManagement and ClientCredentials.
            return (TProxy)classType
                .GetConstructor(new Type[] { typeof(IServiceManagement<TService>), typeof(ClientCredentials) })
                .Invoke(new object[] { serviceManagement, authCredentials.ClientCredentials });
        }

        private static bool RemoteCertificateValidationCallback(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
            {
                return true;
                //return false;
            }

            return true;
        }
    }
}