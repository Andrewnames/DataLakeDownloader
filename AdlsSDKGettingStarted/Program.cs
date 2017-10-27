using System;
using System;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.DataLake.Store;
using Microsoft.Azure.Management.DataLake.Store;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest.Azure.Authentication;

namespace AdlsSDKGettingStarted
{
    public class Program
    {
        private static string applicationId = "4798bbfd-246c-4675-98d9-63b187306266";     // Also called client id
        private static string clientSecret = "sdzsRmNL2Tqp7f4aDFST+dChRnlb0d0n1L4T/9uvaOs=";
        private static string tenantId = "5a91e3b5-8913-46fa-af5d-d4eb37e32d86";
        private static string adlsAccountFQDN = "andrewnames.azuredatalakestore.net";   // full account FQDN, not just the account name like example.azure.datalakestore.net

        public static void Main(string[] args)
        {
            // Obtain AAD token
            var creds = new ClientCredential(applicationId, clientSecret);
            var clientCreds = ApplicationTokenProvider.LoginSilentAsync(tenantId, creds).GetAwaiter().GetResult();

            // Create ADLS client object

            try
            {
                string fileName = "/Test/Training.zip";




                var filePathToWrite = Path.Combine(Directory.GetCurrentDirectory(), "Training.zip");
                var extracPathToWrite = Path.Combine(Directory.GetCurrentDirectory());
                var adlsFileSystemClient = new DataLakeStoreFileSystemManagementClient(clientCreds);
                var srcPath = fileName;
                var destPath = filePathToWrite;
                using (var stream = adlsFileSystemClient.FileSystem.OpenAsync("andrewnames", srcPath))
                using (var fileStream = new FileStream(destPath, FileMode.Create))
                {
                    var streamTask = stream.Result.CopyToAsync(fileStream);

                   
                    
                        while (!streamTask.IsCompleted)
                        {
                        Console.Clear();
                            Console.WriteLine("************training content downloading in process. It will take about two minutes..please wait..." + DateTime.Now.ToString("h:mm:ss tt"));
                            Thread.Sleep(1000);

                        }
                   
                    ZipFile.ExtractToDirectory(destPath, extracPathToWrite);

                }






            }
            catch (AdlsException e)
            {
                PrintAdlsException(e);
            }

            Console.WriteLine("Done. Press ENTER to continue ...");
            Console.ReadLine();
        }

        private static void PrintAdlsException(AdlsException exp)
        {
            Console.WriteLine("ADLException");
            Console.WriteLine($"   Http Status: {exp.HttpStatus}");
            Console.WriteLine($"   Http Message: {exp.HttpMessage}");
            Console.WriteLine($"   Remote Exception Name: {exp.RemoteExceptionName}");
            Console.WriteLine($"   Server Trace Id: {exp.TraceId}");
            Console.WriteLine($"   Exception Message: {exp.Message}");
            Console.WriteLine($"   Exception Stack Trace: {exp.StackTrace}");
            Console.WriteLine();
        }
    }
}