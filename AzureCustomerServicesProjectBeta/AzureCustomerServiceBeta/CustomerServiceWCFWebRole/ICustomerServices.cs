using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace CustomerServiceWCFWebRole
{
    [ServiceContract]
    public interface ICustomerServices
    {
        [OperationContract]
        string GetFavorites(out string favoriteMovie, out string favoriteLanguage,
            string firstName, string lastName);

        [OperationContract]
        string UpdateFavoritesByName(string firstName, string lastName, string favoriteMovie,
          string favoriteLanguage);

        [OperationContract]
        string GetCustomerList(out DataSet customers);

        [OperationContract]
        string AddACustomer(string firstName, string lastName, string favoriteMovie,
           string favoriteLanguage);

        [OperationContract]
        string SubmitToQueue(string firstName, string lastName);
    }


    //// Use a data contract as illustrated in the sample below to add composite types to service operations.
    //[DataContract]
    //public class CompositeType
    //{
    //    bool boolValue = true;
    //    string stringValue = "Hello ";

    //    [DataMember]
    //    public bool BoolValue
    //    {
    //        get { return boolValue; }
    //        set { boolValue = value; }
    //    }

    //    [DataMember]
    //    public string StringValue
    //    {
    //        get { return stringValue; }
    //        set { stringValue = value; }
    //    }
    //}
}
