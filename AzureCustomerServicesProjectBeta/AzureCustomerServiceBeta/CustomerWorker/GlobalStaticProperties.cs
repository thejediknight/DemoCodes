using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.Diagnostics;

namespace CustomerWorker
{
    internal static class GlobalStaticProperties
    {

        private static string _ProcessQueueName;
        /// <summary>
        /// name of the queue
        /// </summary>
        internal static string ProcessQueueName
        {
            get
            {
                if (string.IsNullOrEmpty(_ProcessQueueName))
                {
                    _ProcessQueueName = RoleEnvironment.GetConfigurationSettingValue("ProcessQueueName");
                    Trace.TraceInformation("[CustomerWorker.GlobalStaticProperties] "
                      + "ProcessQueueName to {0}", _ProcessQueueName);
                }
                return _ProcessQueueName;
            }
        }

        private static int _QueueMessageVisibilityTime { get; set; }
        /// <summary>
        /// This is the amount of time the message remains invisible after being
        /// read from the queue, before it becomes visible again (unless it is deleted)
        /// </summary>
        internal static int QueueMessageVisibilityTime
        {
            get
            {
                if (_QueueMessageVisibilityTime <= 0)
                {
                    //hasn't been loaded yet, so load it 
                    string VisTime =
                      RoleEnvironment.GetConfigurationSettingValue("QueueMessageVisibilityTime");
                    int intTest = 0;
                    bool success = int.TryParse(VisTime, out intTest);
                    if (!success || intTest <= 0)
                    {
                        _QueueMessageVisibilityTime = 120;
                    }
                    else
                    {
                        _QueueMessageVisibilityTime = intTest;
                    }
                    Trace.TraceInformation("[CustomerWorker.GlobalStaticProperties] "
                      + "Setting QueueMessageVisibilityTime to {0}", _QueueMessageVisibilityTime);
                }
                return _QueueMessageVisibilityTime;
            }
        }
    }
}
