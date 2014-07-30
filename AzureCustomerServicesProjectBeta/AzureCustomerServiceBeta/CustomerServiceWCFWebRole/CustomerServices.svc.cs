using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace CustomerServiceWCFWebRole
{
    //This atrribute allows the WCF service to be called from outside the Azure load balancer.
    [ServiceBehavior(AddressFilterMode = AddressFilterMode.Any)]

    //This class provides the implementation of our service contract in ICustomerServices
    public class CustomerServices : ICustomerServices
    {
        // The first time the queue is initialized, it will set a boolean called storageInitialized 
        // to true, and then subsequent calls will know it is already initlaized.
        private static bool storageInitialized = false;
        
        // The object gate is used for locking, to make sure multiple people don’t initialize the queue at the same time.
        private static object gate = new Object();

        // CloudQueueClient is the client for accessing the queue.
        private static CloudQueueClient queueStorage;

        // CloudQueue is the queue itself.
        private static CloudQueue queue;


        // Change our service to call the TableStorageMethods instead of the SQL Database methods.
        // We can switch between the two by using this Enum.
        public enum DataBaseType { sqlazure, tablestorage }
        private DataBaseType currentDataBase = DataBaseType.tablestorage;



        //initialize the queue, but only the first time 
        private void InitializeStorage()
        {
            //if it's already initialized, return
            if (storageInitialized)
            {
                return;
            }
            //lock the object
            lock (gate)
            {
                //if someone else initialize the queue while you had the object locked,
                //  return
                if (storageInitialized)
                {
                    return;
                }
                //try initializing the queue
                try
                {
                    Trace.TraceInformation("[CustomerServices.InitializeStorage] Initializing storage queue");
                    // read account configuration settings and get a reference 
                    //  to the storage account
                    CloudStorageAccount storageAccount =
                        CloudStorageAccount.Parse(
                        RoleEnvironment.GetConfigurationSettingValue(
                        "Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString"));

                    // get a reference to the queue client
                    queueStorage = storageAccount.CreateCloudQueueClient();
                    // this uses the entry in GlobalStaticProperties that is set in the role config
                    queue = queueStorage.GetQueueReference(GlobalStaticProperties.ProcessQueueName);
                    //create the queue if it doesn't already exist
                    queue.CreateIfNotExists();
                }
                catch (WebException ex)
                {
                    //try to give some help
                    Trace.TraceError("[CustomerServices.InitializeStorage] WebException thrown trying "
                      + " to initialize the storage services. "
                      + " Check the storage account config settings. If running locally, "
                      + "be sure Dev Storage svc is running. Exception = {0}",
                        ex.ToString());
                    return;
                }
                catch (Exception ex)
                {
                    Trace.TraceError("[CustomerServices.InitializeStorage] Exception thrown trying "
                      + "to initialize the storage. Exception = {0}", ex.ToString());
                    return;
                }
                //this is only set to true if it doesn't throw an exception
                storageInitialized = true;
            }
        }

        //Method to add a message to the queue. 
        //This actually formats the message and adds it to the queue.
        public string SubmitToQueue(string firstName, string lastName)
        {
            //call to make sure the queue exists
            InitializeStorage();
            string errorMessage = string.Empty;
            string favoriteMovie = string.Empty;
            string favoriteLanguage = string.Empty;
            //get the favorites info for the name passed in
            errorMessage = GetFavorites(out favoriteMovie, out favoriteLanguage,
                firstName, lastName);
            if (errorMessage.Length == 0)
            {
                //I'm passing the message as a comma-delimited string. Format the string.
                string msgString = String.Format("process,{0},{1},{2},{3}",
                  firstName, lastName, favoriteMovie, favoriteLanguage);
                //set the message
                CloudQueueMessage message = new CloudQueueMessage(msgString);
                Trace.TraceInformation("[SubmitToQueue] Message passed to queue = {0}", msgString);
                //add the message to the queue
                queue.AddMessage(message);
            }
            else
            {
                errorMessage = "Entry not submitted to queue. "
                  + "Error when retrieving favorites = '" + errorMessage + "'.";
                Trace.TraceError("[SubmitToQueue] firstName = {0}, lastName = {1}, {2}",
                    firstName, lastName, errorMessage);
            }
            return errorMessage;
        }
        
        public string GetFavorites(out string favoriteMovie, out string favoriteLanguage,
          string firstName, string lastName)
        {
            string errorMessage = string.Empty;
            favoriteMovie = string.Empty;
            favoriteLanguage = string.Empty;

            if (currentDataBase == DataBaseType.sqlazure)
            {
                CustomerFavorites cf = new CustomerFavorites();
                errorMessage = cf.GetCustomerFavorites(out favoriteMovie, out favoriteLanguage,
                  firstName, lastName);
            }
            else
            {
                TableStorageMethods tsm = new TableStorageMethods();
                errorMessage = tsm.ST_GetCustomerFavorites(out favoriteMovie, out favoriteLanguage,
                  firstName, lastName);
            }
            return errorMessage;
        }

        public string UpdateFavoritesByName(string firstName, string lastName, string favoriteMovie,
          string favoriteLanguage)
        {
            string errorMessage = string.Empty;

            if (currentDataBase == DataBaseType.sqlazure)
            {
                CustomerFavoritesUpdate cfu = new CustomerFavoritesUpdate();
                errorMessage = cfu.SetCustomerFavorites(firstName, lastName, favoriteMovie, favoriteLanguage);
            }
            else
            {
                TableStorageMethods tsm = new TableStorageMethods();
                errorMessage = tsm.ST_SetCustomerFavorites(firstName, lastName, favoriteMovie, favoriteLanguage);
            }

            return errorMessage;
        }

        public string AddACustomer(string firstName, string lastName,
          string favoriteMovie, string favoriteLanguage)
        {
            string errorMessage = string.Empty;

            if (currentDataBase == DataBaseType.sqlazure)
            {
                CustomerFavoritesAdd cfa = new CustomerFavoritesAdd();
                errorMessage = cfa.AddCustomer(firstName, lastName, favoriteMovie, favoriteLanguage);
            }
            else
            {
                TableStorageMethods tsm = new TableStorageMethods();
                errorMessage = tsm.ST_AddCustomer(firstName, lastName, favoriteMovie, favoriteLanguage);
            }

            return errorMessage;
        }


        public string GetCustomerList(out DataSet customers)
        {
            string errorMessage = string.Empty;
            customers = new DataSet();

            if (currentDataBase == DataBaseType.sqlazure)
            {
                CustomerList cl = new CustomerList();
                errorMessage = cl.GetListOfCustomers(out customers);
            }
            else
            {
                TableStorageMethods tsm = new TableStorageMethods();
                errorMessage = tsm.ST_GetListOfCustomers(out customers);
            }

            return errorMessage;
        }

        //public CompositeType GetDataUsingDataContract(CompositeType composite)
        //{
        //    if (composite == null)
        //    {
        //        throw new ArgumentNullException("composite");
        //    }
        //    if (composite.BoolValue)
        //    {
        //        composite.StringValue += "Suffix";
        //    }
        //    return composite;
        //}
    }
}
