using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerWorker
{
    class ProcessQueue
    {
        public string ProcessQueueEntry(string firstName, string lastName, string favoriteMovie,
          string favoriteLanguage, CloudBlobContainer container)
        {
            string errorMessage = string.Empty;

            try
            {
                Trace.TraceInformation("[ProcessQueueEntry] for command [process], " +
                    "firstName = {0}, lastName = {1}, favoriteMovie = {2}, favoriteLanguage = {3}",
                    firstName, lastName, favoriteMovie, favoriteLanguage);

                //let's write the information to blob storage. First, create the message.
                string messageToWrite = string.Format("FirstName = {0}{1}LastName={2}{1}" +
                  "FavoriteMovie = {3}{1}FavoriteLanguage={4}",
                  firstName, Environment.NewLine, lastName,
                  favoriteMovie, favoriteLanguage);

                //now create the file name -- I'm putting the date/time stamp in the name.
                // It's nothing but a unique blob name that we are trying to give to this blob. 
                string fileName = "test_" + DateTime.Now.ToUniversalTime().ToString("yyyyMMdd-hh-mm-ss",
                  new System.Globalization.CultureInfo("en-US")) + ".txt";

                //get a reference to the blob 
                var blob = container.GetBlockBlobReference(fileName); //in client lib 1.7 : GetBlobReference(fileName);

                //upload the text to the blob
                var bytesToUpload = Encoding.UTF8.GetBytes(messageToWrite);
                using(var memoryStream = new MemoryStream(bytesToUpload))
                {
                    blob.UploadFromStream(memoryStream);
                }

                //Old code for azure storage client library 1.7
                //blob.UploadText(messageToWrite);

            }
            catch (Exception ex)
            {
                errorMessage = "Error processing entry.";
                Trace.TraceError("[ProcessQueueEntry] Exception thrown = {0}", ex);
            }
            return errorMessage;
        }
    }
}
