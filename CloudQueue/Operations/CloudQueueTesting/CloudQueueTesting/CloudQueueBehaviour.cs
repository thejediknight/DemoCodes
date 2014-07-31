using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CloudQueueTesting
{
    static class CloudQueueBehaviour
    {
        /// <summary>
        /// Verifies whether or not the specified message size can be accommodated in an Azure queue.
        /// </summary>
        /// <param name="size">The message size value to be inspected.</param>
        /// <returns>True if the specified size can be accommodated in an Azure queue, otherwise false.</returns>

        //The maximum message size defined in CloudQueueMessage.MaxMessageSize property is not reflective
        //of the maximum allowed payload size. Messages are subject to Base64 encoding when they are 
        //transmitted to a queue. The encoded payloads are always larger than their raw data. The Base64 
        //encoding adds 25% overhead on average. As a result, the 64KB size limit effectively prohibits 
        //from storing any messages with payload larger than 48KB (75% of 64KB).
        public static bool IsAllowedQueueMessageSize(long size)
        {
            return size >= 0 && size <= (CloudQueueMessage.MaxMessageSize - 1) / 4 * 3;
        }
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Enter a size of message in Queue");
                long sz = Convert.ToInt64(Console.ReadLine());

                Console.WriteLine("{0}", IsAllowedQueueMessageSize(sz));
            }
        }
    }
}
