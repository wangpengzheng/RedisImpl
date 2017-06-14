using System;
using System.Linq;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;

namespace ConsoleTest.Util
{
    class DemoDataProvider
    {
        private static OrganizationServiceProxy _service = null;
        public const String ProjectNameDemo = "G20170411000019";
        public const String ProjectPlanIdDemo = "G20170103000080-1";
        public const String AccountDemo = "C00000024"; //Test: "C00000397" // Official: "C00000003";// Dev: "C00000260";
        public const String BankIdDemo = "B-0002";
        public const String SubbranchDemo = "BD-00000001";
        public const String PlanOperationDemo = "260DF626-B9F1-E611-80CA-0050569572DA";
        public const String AutoEntityShare = "A6944E97-86F6-E611-80BF-005056950284";

        public static OrganizationServiceProxy GetOrganizationService()
        {
            if (_service == null)
            {
                _service = (OrganizationServiceProxy)OrganizationService.CreateOrganizationService();
            }

            return _service;
        }

        public static Entity GetTestAccountEntity(OrganizationServiceProxy service)
        {
            // 根据项目ID获取项目信息
            QueryByAttribute query = new QueryByAttribute("account");
            query.ColumnSet = new ColumnSet("name", "accountnumber");
            query.Attributes.Add("accountnumber");
            query.Values.Add(AccountDemo);
            EntityCollection entitityCollection = service.RetrieveMultiple(query);

            if (entitityCollection.Entities.Count() == 1)
            {
                Entity postProjectImage = entitityCollection.Entities[0];
                return postProjectImage;
            }
            else
            {
                throw new Exception(String.Format("客户ID:{0}未找到", AccountDemo));
            }
        }

        public static Entity GetTestAccountEntityByID(OrganizationServiceProxy service)
        {
            //return service.Retrieve("account", new Guid(""), new ColumnSet(true));
            return service.Retrieve("account", new Guid("c63056a2-f149-e611-80ba-005056950284"), new ColumnSet("name","accountnumber"));
        }

        public static string GetTestAccountEntityByID_WebAPI()
        {
            //return service.Retrieve("account", new Guid(""), new ColumnSet(true));
            return WebApi.RetrieveOneProperty("c63056a2-f149-e611-80ba-005056950284", "accounts", "name");
        }

        public static Entity GetTestProjectEntity(OrganizationServiceProxy service)
        {
            // 根据项目ID获取项目信息
            QueryByAttribute query = new QueryByAttribute("ubg_project");
            query.ColumnSet = new ColumnSet(true);
            query.Attributes.Add("ubg_name");
            query.Values.Add(ProjectNameDemo);
            EntityCollection entitityCollection = service.RetrieveMultiple(query);

            if (entitityCollection.Entities.Count() == 1)
            {
                Entity postProjectImage = entitityCollection.Entities[0];
                return postProjectImage;
            }
            else
            {
                throw new Exception(String.Format("项目ID:{0}未找到", ProjectNameDemo));
            }
        }

        public static Entity GetTestFinancingDemandEntity(OrganizationServiceProxy service)
        {
            // 根据项目ID获取项目信息
            QueryByAttribute query = new QueryByAttribute("ubg_financingdemand");
            query.ColumnSet = new ColumnSet(true);
            query.Attributes.Add("ubg_name");
            query.Values.Add(ProjectNameDemo);
            EntityCollection entitityCollection = service.RetrieveMultiple(query);

            if (entitityCollection.Entities.Count() == 1)
            {
                Entity postProjectImage = entitityCollection.Entities[0];
                return postProjectImage;
            }
            else
            {
                throw new Exception(String.Format("融资需求ID:{0}未找到", ProjectNameDemo));
            }
        }

        public static Entity GetTestSubbranchEntity(OrganizationServiceProxy service)
        {
            // 根据支行编号获取项目信息
            QueryByAttribute query = new QueryByAttribute("ubg_subbranch");
            query.ColumnSet = new ColumnSet(true);
            query.Attributes.Add("ubg_subbranchnum");
            query.Values.Add(SubbranchDemo);
            EntityCollection entitityCollection = service.RetrieveMultiple(query);

            if (entitityCollection.Entities.Count() == 1)
            {
                Entity postsubbranchImage = entitityCollection.Entities[0];
                return postsubbranchImage;
            }
            else
            {
                throw new Exception(String.Format("支行ID:{0}未找到", SubbranchDemo));
            }
        }
        public static Entity GetTestBankEntity(OrganizationServiceProxy service)
        {
            // 根据银行编号获取银行信息
            QueryByAttribute query = new QueryByAttribute("ubg_bank");
            query.ColumnSet = new ColumnSet(true);
            query.Attributes.Add("ubg_id");
            query.Values.Add(BankIdDemo);
            EntityCollection entitityCollection = service.RetrieveMultiple(query);

            if (entitityCollection.Entities.Count() == 1)
            {
                Entity curBank = entitityCollection.Entities[0];
                return curBank;
            }
            else
            {
                throw new Exception(String.Format("银行编号:{0}未找到", BankIdDemo));
            }
        }

        public static Entity GetTestProjectPlanEntity(OrganizationServiceProxy service)
        {
            QueryByAttribute query = new QueryByAttribute("ubg_projectplan");
            query.ColumnSet = new ColumnSet(true);
            query.Attributes.Add("ubg_name");
            query.Values.Add(ProjectPlanIdDemo);
            EntityCollection entitityCollection = service.RetrieveMultiple(query);

            if (entitityCollection.Entities.Count() == 1)
            {
                Entity projectPlan = entitityCollection.Entities[0];
                return projectPlan;
            }
            else
            {
                throw new Exception(String.Format("用款计划:{0}未找到", ProjectPlanIdDemo));
            }
        }

        public static Entity GetTestPlanOperationEntity(OrganizationServiceProxy service)
        {
            QueryByAttribute query = new QueryByAttribute("ubg_planoperation");
            query.ColumnSet = new ColumnSet(true);
            query.Attributes.Add("ubg_planoperationid");
            query.Values.Add(new Guid(PlanOperationDemo));
            EntityCollection entitityCollection = service.RetrieveMultiple(query);

            if (entitityCollection.Entities.Count() == 1)
            {
                Entity projectPlan = entitityCollection.Entities[0];
                return projectPlan;
            }
            else
            {
                throw new Exception(String.Format("用款操作:{0}未找到", PlanOperationDemo));
            }
        }

        public static Entity GetAutoEntityShareEntity(OrganizationServiceProxy service)
        {
            QueryByAttribute query = new QueryByAttribute("ubg_autoentityshare");
            query.ColumnSet = new ColumnSet(true);
            query.Attributes.Add("ubg_autoentityshareid");
            query.Values.Add(new Guid(AutoEntityShare));
            EntityCollection entitityCollection = service.RetrieveMultiple(query);

            if (entitityCollection.Entities.Count() == 1)
            {
                Entity autoEntityShare = entitityCollection.Entities[0];
                return autoEntityShare;
            }
            else
            {
                throw new Exception(String.Format("自动共享配置:{0}未找到", PlanOperationDemo));
            }
        }
    }
}
