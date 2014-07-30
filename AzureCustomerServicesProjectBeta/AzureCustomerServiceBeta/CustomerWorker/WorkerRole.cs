using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Diagnostics.Management;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Blob;

namespace CustomerWorker
{
    public class WorkerRole : RoleEntryPoint
    {
        CloudQueue queue;
        CloudBlobContainer container;
        public override void Run()
        {
            //start up the queue
          StartUpQueue();

          //loop infinitely until the service shuts down
          while (true)
          {
              try
              {
                  // retrieve a new message from the queue, set the visibility
                  // this is hours, minutes, seconds, and the global static property is in seconds
                  TimeSpan visTimeout =
                    new TimeSpan(0, 0, GlobalStaticProperties.QueueMessageVisibilityTime);
                  CloudQueueMessage msg = queue.GetMessage(visTimeout);
                  if (msg != null)
                  {
                      Trace.TraceInformation("[CustomerWorker.Run] message = {0}, time = {1}, "
                        + "next visible time = {2}",
                        msg.AsString, DateTime.UtcNow, msg.NextVisibleTime.Value.ToString());
                      string errorMessage = string.Empty;
                      //process the message 
                      //assume comma-delimited, first is command. check it and handle the message accordingly
                      string[] msgFields = msg.AsString.Split(new char[] { ',' });
                      string command = msgFields[0];
                      switch (command)
                      {
                          case "process":
                              string firstName = msgFields[1];
                              string lastName = msgFields[2];
                              string favoriteMovie = msgFields[3];
                              string favoriteLanguage = msgFields[4];
                              ProcessQueue pq = new ProcessQueue();
                              pq.ProcessQueueEntry(firstName, lastName, favoriteMovie, favoriteLanguage,
                                container);
                              break;
                      }

                      // remove message from queue
                      //http://blog.smarx.com/posts/deleting-windows-azure-queue-messages-handling-exceptions            
                      try
                      {
                          queue.DeleteMessage(msg);
                      }
                      catch (StorageException ex)
                      {
                          if (ex.RequestInformation.ExtendedErrorInformation != null && ex.RequestInformation.ExtendedErrorInformation.ErrorCode == "MessageNotFound")
                          {
                              // pop receipt must be invalid
                              // ignore or log (so we can tune the visibility timeout)
                          }
                          else
                          {
                              // not the error we were expecting
                              throw;
                          }
                      }
                  }
                  else
                  {
                      //no message found, sleep for 5 seconds
                      Thread.Sleep(5000);
                  }
              }
              catch (Exception ex)
              {
                  Trace.TraceError("[CustomerWorker.Run] Exception thrown "
                    + "trying to read from the queue = {0}", ex.ToString());
                  Thread.Sleep(5000);
              }
          }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // Get a reference to the initial default configuration.
            string wadConnectionString = "Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString";

            // First, get a reference to the storage account where the diagnostics will be written. 
            // It is recommended that you use a separate account for diagnostics and data, so the 
            //   performance of your data access is not impacted by the diagnostics.
            CloudStorageAccount storageAccount =
                CloudStorageAccount.Parse(
                RoleEnvironment.GetConfigurationSettingValue(wadConnectionString));

            // Get an reference to the diagnostic manager for the role instance, 
            //   and then get the default initial configuration, which we will then change.
            RoleInstanceDiagnosticManager roleInstanceDiagnosticManager =
                CloudAccountDiagnosticMonitorExtensions.CreateRoleInstanceDiagnosticManager(
                RoleEnvironment.GetConfigurationSettingValue(wadConnectionString),
                RoleEnvironment.DeploymentId,
                RoleEnvironment.CurrentRoleInstance.Role.Name, RoleEnvironment.CurrentRoleInstance.Id);
            DiagnosticMonitorConfiguration config = DiagnosticMonitor.GetDefaultInitialConfiguration();

            // Change the polling interval for checking for configuration changes
            //   and the buffer quota for the logs. 
            config.ConfigurationChangePollInterval = TimeSpan.FromSeconds(30.0);
            config.DiagnosticInfrastructureLogs.BufferQuotaInMB = 256;

            // The diagnostics data is written locally and then transferred to Azure Storage. 
            // These are the transfer intervals for doing that operation.
            config.Logs.ScheduledTransferPeriod = TimeSpan.FromMinutes(1.0); //for trace logs
            config.Directories.ScheduledTransferPeriod = TimeSpan.FromMinutes(1.0); //for iis logs

            // Configure the monitoring of one Windows performance counter
            // and add it to the configuration.
            int sampleRate = 0;
            int scheduledTransferPeriod = 0;
            bool success = false;
            //this is sample rate, in seconds, for the performance monitoring in %CPU.
            //By making this configurable, you can change the azure config rather than republish the role.
            success = int.TryParse(RoleEnvironment.GetConfigurationSettingValue("PerfMonSampleRate"),
              out sampleRate);
            if (!success || sampleRate <= 0)
                sampleRate = 60;  //default is 60 seconds
            success =
              int.TryParse(RoleEnvironment.GetConfigurationSettingValue("PerfMonScheduledTransferPeriod"),
              out scheduledTransferPeriod);
            if (!success || scheduledTransferPeriod <= 0)
                scheduledTransferPeriod = 120;  //default is 120 seconds

            PerformanceCounterConfiguration perfConfig
                = new PerformanceCounterConfiguration();
            perfConfig.CounterSpecifier = @"\Processor(*)\% Processor Time";
            perfConfig.SampleRate = TimeSpan.FromSeconds((double)sampleRate);
            config.PerformanceCounters.DataSources.Add(perfConfig);
            config.PerformanceCounters.ScheduledTransferPeriod =
              TimeSpan.FromSeconds((double)scheduledTransferPeriod);

            // Configure monitoring of Windows Application and System Event logs,
            // including the quota and scheduled transfer interval, and add them 
            // to the configuration.
            WindowsEventLogsBufferConfiguration eventsConfig
                = new WindowsEventLogsBufferConfiguration();
            eventsConfig.BufferQuotaInMB = 256;
            eventsConfig.ScheduledTransferLogLevelFilter = Microsoft.WindowsAzure.Diagnostics.LogLevel.Undefined; //was warning
            eventsConfig.ScheduledTransferPeriod = TimeSpan.FromMinutes(2.0); //was 10
            eventsConfig.DataSources.Add("Application!*");
            eventsConfig.DataSources.Add("System!*");
            config.WindowsEventLog = eventsConfig;

            //set the configuration to be used by the current role instance
            roleInstanceDiagnosticManager.SetCurrentConfiguration(config);

            //add an event handler for the configuration being changed while the role is running
            RoleEnvironment.Changing +=
              new EventHandler<RoleEnvironmentChangingEventArgs>(RoleEnvironment_Changing);

            //****************Set up our Blob Storage (Container)****************
            //get a reference to the blob client
            CloudBlobClient cbc = storageAccount.CreateCloudBlobClient();
            //get a reference to the container, and create it if it doesn't exist
            container = cbc.GetContainerReference("codecamp");
            container.CreateIfNotExists();
            //now set the permissions so the container is private, 
            //but the blobs are public, so they can be accessed with a specific URL
            BlobContainerPermissions permissions = new BlobContainerPermissions();
            permissions.PublicAccess = BlobContainerPublicAccessType.Blob;
            container.SetPermissions(permissions);

            //call base OnStart();
            return base.OnStart();
        }

        void RoleEnvironment_Changing(object sender, RoleEnvironmentChangingEventArgs e)
        {
            // If a configuration setting is changing
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
            {
                // Set e.Cancel to true to restart this role instance
                e.Cancel = true;
            }
        }

        private void StartUpQueue()
        {
            //get a reference to the storage account
            CloudStorageAccount storageAccount =
                CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue(
                "Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString"));

            // initialize the queue client that will be used to access the queue 
            CloudQueueClient queueStorage = storageAccount.CreateCloudQueueClient();
            string queueName = GlobalStaticProperties.ProcessQueueName;
            //get a reference to the queue
            queue = queueStorage.GetQueueReference(queueName);

            //only initialize this once after the role starts up
            //so check this boolean and loop until it manages to make sure the queue is present 
            //because this role can't run without the queue
            bool storageInitialized = false;
            while (!storageInitialized)
            {
                try
                {
                    // create the message queue if it doesn't already exist
                    queue.CreateIfNotExists();
                    // set this to true, because at this point, we know it's there
                    storageInitialized = true;
                }
                catch (StorageException ex)
                {
                    // for this error, give a reminder about the dev storage service being started
                    if (ex.RequestInformation.HttpStatusCode != 200)
                    {
                        Trace.TraceError("[CustomerWorker.StartUpQueue] Storage services initialization failure."
                          + " Check your storage account configuration settings. If running locally,"
                          + " ensure that the Development Storage service is running. Message: '{0}'",
                          ex.Message);
                        //sleep 5 seconds and then loop back around and try again
                        System.Threading.Thread.Sleep(5000);
                    }
                    else
                    {
                        Trace.TraceError("[CustomerWorker.StartUpQueue] StorageClientException thrown. "
                          + "Ex = {0}", ex.ToString());
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    Trace.TraceError("[CustomerWorker.StartupQueue] Exception thrown "
                      + "trying to initialize the queue. Ex = {0}", ex.ToString());
                    throw;
                }
            }
        }
    }
}
