using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.Diagnostics;
using System.Data.Services.Client;
using System.Data;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.DataServices;

namespace CustomerServiceWCFWebRole
{
    public class TableStorageMethods
    {
        private CloudTableClient cloudTableClient;
        string tableName = "customer";

        public TableStorageMethods()
        {
            //get a reference to the cloud storage account, and then make sure the table exists 
            CloudStorageAccount cloudStorageAccount =
              CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("DataConnectionString"));
            cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
            CloudTable table = cloudTableClient.GetTableReference(tableName);
            table.CreateIfNotExists();
        }

        internal string ST_AddCustomer(string firstName, string lastName,
          string favoriteMovie, string favoriteLanguage)
        {
            Trace.TraceInformation("[AddCustomer] called. FirstName = {0}, LastName = {1}, Movie = {2}, "
              + "Language = {3}", firstName, lastName, favoriteMovie, favoriteLanguage);

            var customerEntity = new CustomerEntity(firstName, lastName, favoriteMovie, favoriteLanguage);

            string errorMessage = string.Empty;

            try
            {
                //add the record to the table
                CloudTable table = cloudTableClient.GetTableReference(tableName);
                TableOperation insertOperation = TableOperation.Insert(customerEntity);
                table.Execute(insertOperation);

                //Library 1.7 Code
                //TableServiceContext tableServiceContext = cloudTableClient.GetDataServiceContext();
                //tableServiceContext.AddObject(tableName, cust);
                //tableServiceContext.SaveChanges();
            }
            //you might want to handle these two exceptions differently
            catch (DataServiceRequestException ex)
            {
                errorMessage = "Error adding entry.";
                Trace.TraceError("[ST_AddCustomer] firstName = {0}, lastName = {1}, exception = {2}",
                  firstName, lastName, ex);
            }
            //this exception could be caused by a problem with the storage account
            catch (StorageException ex)
            {
                errorMessage = "Error adding entry.";
                Trace.TraceError("[ST_AddCustomer] firstName = {0}, lastName = {1}, exception = {2}",
                  firstName, lastName, ex);
            }
            //general catch
            catch (Exception ex)
            {
                errorMessage = "Error adding entry.";
                Trace.TraceError("[ST_AddCustomer] firstName = {0}, lastName = {1}, exception = {2}",
                  firstName, lastName, ex);
            }
            return errorMessage;
        }

        internal string ST_GetCustomerFavorites(out string favoriteMovie, out string favoriteLanguage,
          string firstName, string lastName)
        {
            Trace.TraceInformation("[GetCustomerFavorites] called. FirstName = {0}, LastName = {1}",
              firstName, lastName);
            string errorMessage = string.Empty;
            favoriteMovie = string.Empty;
            favoriteLanguage = string.Empty;

            CustomerEntity customerEntity = new CustomerEntity(firstName, lastName, string.Empty, string.Empty);

            try
            {
                //Old 1.7 TableServiceContext tableServiceContext = cloudTableClient.GetDataServiceContext();
                //TableOperation.Retrieve
                //IQueryable<CustomerEntity> entities = (from e in table.CreateQuery<CustomerEntity>()
                //                                 where e.PartitionKey == cust.PartitionKey && e.RowKey == cust.RowKey
                //                                 select e);

                CloudTable table = cloudTableClient.GetTableReference(tableName);

                //One way of doing it in 2.0 library:

                //TableOperation retrieveOperation = TableOperation.Retrieve<CustomerEntity>(customerEntity.PartitionKey, customerEntity.RowKey);
                //TableResult retrievedResult = table.Execute(retrieveOperation);
                //CustomerEntity fetchedCustomerEntity = retrievedResult.Result as CustomerEntity;

                //Another way:

                //create filter for PartitonKey condition
                string pkFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, customerEntity.PartitionKey);
                //create filter for RowKey condition
                string rkFilter = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, customerEntity.RowKey);
                
                //create combined filter 
                string combinedFilter = TableQuery.CombineFilters(pkFilter, TableOperators.And, rkFilter);

                //create query to execute based on bove filters
                var query = new TableQuery<CustomerEntity>().Where(combinedFilter);
                //execute the query. Result will be the CustomerEntry satisfying the filters
                var fetchedCutomerEntity = table.ExecuteQuery<CustomerEntity>(query).FirstOrDefault();

                favoriteMovie = fetchedCutomerEntity.FavoriteMovie;
                favoriteLanguage = fetchedCutomerEntity.FavoriteLanguage;
            }
            catch (Exception ex)
            {
                Trace.TraceError("[ST_GetCustomerFavorites] firstName = {0}, lastName = {1}, exception = {2}",
                  firstName, lastName, ex);
                errorMessage = "Error retrieving data.";
            }
            return errorMessage;
        }

        internal string ST_SetCustomerFavorites(string firstName, string lastName,
          string favoriteMovie, string favoriteLanguage)
        {
            Trace.TraceInformation("[SetCustomerFavorites] FirstName = {0}, LastName = {1}, Movie = {2}, "
              + "Language = {3}", firstName, lastName, favoriteMovie, favoriteLanguage);

            string errorMessage = string.Empty;

            CustomerEntity customerEntity = new CustomerEntity(firstName, lastName, favoriteMovie, favoriteLanguage);

            try
            {
                CloudTable table = cloudTableClient.GetTableReference(tableName);

                //create filter for PartitonKey condition
                string pkFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, customerEntity.PartitionKey);
                //create filter for RowKey condition
                string rkFilter = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, customerEntity.RowKey);

                //create combined filter 
                string combinedFilter = TableQuery.CombineFilters(pkFilter, TableOperators.And, rkFilter);

                //create query to execute based on bove filters
                var query = new TableQuery<CustomerEntity>().Where(combinedFilter);
                //execute the query. Result will be the CustomerEntry satisfying the filters
                var queryResultCutomerEntity = table.ExecuteQuery<CustomerEntity>(query).FirstOrDefault();


                queryResultCutomerEntity.FavoriteLanguage = favoriteLanguage;
                queryResultCutomerEntity.FavoriteMovie = favoriteMovie;

                TableOperation replaceOperation = TableOperation.Replace(queryResultCutomerEntity);
                table.Execute(replaceOperation);
            }
            catch (Exception ex)
            {
                Trace.TraceError("[ST_SetCustomerFavorites] FirstName = {0}, LastName = {1}, ex = {2}",
                  firstName, lastName, ex);
                errorMessage = "Error setting customer favorites.";
            }
            return errorMessage;
        }

        internal string ST_GetListOfCustomers(out DataSet customers)
        {
            Trace.TraceInformation("[GetListOfCustomers] called.");
            string errorMessage = string.Empty;

            //since the SQL Azure version returns a dataset, create a dataset and return it.
            //this way you don't have to change the client code
            customers = new DataSet();
            DataTable dt = new DataTable();
            DataColumn wc = new DataColumn("ID", typeof(Int32));
            wc.AutoIncrement = true;
            wc.AutoIncrementSeed = 1;
            wc.AutoIncrementStep = 1;
            dt.Columns.Add(wc);

            dt.Columns.Add("FirstName", typeof(String));
            dt.Columns.Add("LastName", typeof(String));
            dt.Columns.Add("FavoriteMovie", typeof(String));
            dt.Columns.Add("FavoriteLanguage", typeof(String));

            try
            {
                //retrieve the list of customers
                //TableServiceContext tableServiceContext = cloudTableClient.GetDataServiceContext();
                //DataServiceQuery<Customer> dataServiceQuery =
                //  tableServiceContext.CreateQuery<Customer>(tableName);
                //IEnumerable<Customer> entities =
                //  dataServiceQuery.Where(e => e.PartitionKey == "customer").AsTableServiceQuery<Customer>();

                CloudTable table = cloudTableClient.GetTableReference(tableName);

                //create filter for PartitonKey condition
                string pkFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "customer");
 
                //create query to execute based on bove filters
                var query = new TableQuery<CustomerEntity>().Where(pkFilter);
                //execute the query. Result will be the CustomerEntry satisfying the filters
                var entities = table.ExecuteQuery<CustomerEntity>(query);

                if (entities != null)
                {
                    //add the entries to the DataTable
                    foreach (CustomerEntity cust in entities)
                    {
                        DataRow newRow = dt.NewRow();
                        newRow["FirstName"] = cust.FirstName;
                        newRow["LastName"] = cust.LastName;
                        newRow["FavoriteMovie"] = cust.FavoriteMovie;
                        newRow["FavoriteLanguage"] = cust.FavoriteLanguage;
                        dt.Rows.Add(newRow);
                    }
                }
                else
                {
                    Trace.TraceError("[ST_GetListOfCustomers] No rows found in table.");
                    errorMessage = "No rows found in table.";
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("[ST_GetListOfCustomers] ex = {0}", ex);
                errorMessage = "Error getting list of customers.";
            }

            //add the data table to the dataset
            customers.Tables.Add(dt);

            return errorMessage;
        }

    }
}