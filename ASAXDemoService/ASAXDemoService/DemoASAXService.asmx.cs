using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace ASAXDemoService
{
    //
    //ASMX is a legacy technology and is not supported .NEt 4.0 onwards. It is recommened to use WCF for .NET 4.0 and higher.
    //But, if you stillw ant to use ASMX, use the ASP.NET Empty project and add a ASMX file explicitly.
    //Note that the result returned to the client is encoded in XML and is not plain text/HTML.
    //

    /// <summary>
    /// Summary description for DemoASAXService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class DemoASAXService : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        //
        //When browser calls http://localhost:6833/WebService1.asmx/DummyWebMethod, XML encoded string is returned to the 
        //browser. 
        //
        [WebMethod]
        public string DummyWebMethod()
        {
            return "Dummy Web Method returns this.";
        }
    }
}