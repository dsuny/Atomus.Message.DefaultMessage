using Atomus.Database;
using Atomus.Service;
using System;

namespace Atomus.Message.Controllers
{
    internal static class DefaultMessageController
    {
        internal static IResponse Search(this ICore core)
        {
            IServiceDataSet serviceDataSet;

            serviceDataSet = new ServiceDataSet
            {
                ServiceName = core.GetAttribute("ServiceName"),
                TransactionScope = false
            };
            serviceDataSet["Message"].ConnectionName = core.GetAttribute("ConnectionName");
            serviceDataSet["Message"].CommandText = core.GetAttribute("Procedure");
            serviceDataSet["Message"].AddParameter("@USER_ID", DbType.Decimal, 18);

            serviceDataSet["Message"].NewRow();
            serviceDataSet["Message"].SetValue("@USER_ID", Config.Client.GetAttribute("Account.USER_ID") ?? DBNull.Value);

            return core.ServiceRequest(serviceDataSet);
        }
    }
}