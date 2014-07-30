using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TestService
{
    public static class DAC
    {
        //EDIT @harshal

        //First, let’s add a method to get the proxy and set up the endpoint. This enables us to 
        //easily programmatically change the address of the service we’re connecting to. We’ll 
        //call this from the proxy methods that call the WCF service

        //put these here rather than relying on the app.config being right 
        //so you can set up a service reference running the service locally,
        //and then just change this to point to the instance in the cloud
        private static string m_endpointAddress = @"http://127.0.0.1:16765/CustomerServices.svc";
        //private static string m_endpointAddress =  http://yourservicename.cloudapp.net/CustomerServices.svc";

        private static CustomerSvc.CustomerServicesClient getClient()
        {
          CustomerSvc.CustomerServicesClient prx = new CustomerSvc.CustomerServicesClient();
          prx.Endpoint.Address = new System.ServiceModel.EndpointAddress(m_endpointAddress);
          //this sets the timeout of the service call, which should give you enough time
          // to finish debugging your service call
          prx.InnerChannel.OperationTimeout = new TimeSpan(0, 5, 0);
          return prx;
        }

        //
        // rest of the methods
        //
        internal static string GetFavoritesForCustomer(out string favoriteMovie,
          out string favoriteLanguage, string firstName, string lastName)
        {
            favoriteMovie = string.Empty;
            favoriteLanguage = string.Empty;
            CustomerSvc.CustomerServicesClient prx = getClient();
            return prx.GetFavorites(out favoriteMovie, out favoriteLanguage, firstName, lastName);
        }

        internal static string UpdateFavoritesByName(string firstName, string lastName,
          string favoriteMovie, string favoriteLanguage)
        {
            CustomerSvc.CustomerServicesClient prx = getClient();
            return prx.UpdateFavoritesByName(firstName, lastName, favoriteMovie, favoriteLanguage);
        }

        internal static string GetCustomerList(out DataSet customers)
        {
            customers = new DataSet();
            CustomerSvc.CustomerServicesClient prx = getClient();
            return prx.GetCustomerList(out customers);
        }

        internal static string AddACustomer(string firstName, string lastName,
          string favoriteMovie, string favoriteLanguage)
        {
            CustomerSvc.CustomerServicesClient prx = getClient();
            return prx.AddACustomer(firstName, lastName, favoriteMovie, favoriteLanguage);
        }

        internal static string AddToQueue(string firstName, string lastName)
        {
            string errorMessage = string.Empty;
            CustomerSvc.CustomerServicesClient prx = getClient();
            errorMessage = prx.SubmitToQueue(firstName, lastName);
            return errorMessage;
        }

        //EDIT completed @harshal
    }
}
