using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Services.Common;
using System.Data.Services.Client;

namespace CustomerServiceWCFWebRole
{
    [DataServiceKey("PartitionKey", "RowKey")]
    public class CustomerEntity : TableEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FavoriteMovie { get; set; }
        public string FavoriteLanguage { get; set; }


        private readonly string partitionKey = "customer";
        public CustomerEntity() { }

        public CustomerEntity(string firstName, string lastName, string favoriteMovie, string favoriteLanguage)
        {
            PartitionKey = partitionKey;
            RowKey = firstName + " " + lastName;

            FirstName = firstName;
            LastName = lastName;
            FavoriteMovie = favoriteMovie;
            FavoriteLanguage = favoriteLanguage;
        }
    }
}