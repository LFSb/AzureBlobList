using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AzureBlobList
{
  class Program
  {
    private static BlobDownloader blobDownloader;

    static void Main(string[] args)
    {
      // Retrieve storage account from connection string.
      var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);

      // Create the blob client.
      var blobClient = storageAccount.CreateCloudBlobClient();

      // Retrieve reference to a previously created container.
      var container = blobClient.GetContainerReference(ConfigurationManager.AppSettings["ContainerReference"]);

      blobDownloader = new BlobDownloader(container);

      // Loop over items within the container and output the length and URI.
      foreach (var item in container.ListBlobs(null, false))
      {
        if (item is CloudBlockBlob)
        {
          var blob = (CloudBlockBlob)item;

          Console.WriteLine("Block blob of length {0}: {1}", blob.Properties.Length, blob.Uri);

          HandleUserInput(container, blob);
        }
        else if (item is CloudPageBlob)
        {
          var pageBlob = (CloudPageBlob)item;

          Console.WriteLine("Page blob of length {0}: {1}", pageBlob.Properties.Length, pageBlob.Uri);

          HandleUserInput(container, pageBlob);
        }
        else if (item is CloudBlobDirectory)
        {
          var directory = (CloudBlobDirectory)item;

          Console.WriteLine("Directory: {0}", directory.Uri);
        }
      }

      Console.WriteLine("All done!");
      Console.ReadLine();
    }

    private static void HandleUserInput(CloudBlobContainer container, CloudBlockBlob pageBlob)
    {
      Console.WriteLine("Save file? Y/N");

      var result = Console.ReadLine();

      if (String.IsNullOrEmpty(result))
      {
        return;
      }

      switch (result.ToLower())
      {
        case "y":
          {
            blobDownloader.SaveBlockBlob(container, pageBlob);
          } break;
        default:
          {
            Console.WriteLine("welp");
            break;
          }
      }
    }

    private static void HandleUserInput(CloudBlobContainer container, CloudPageBlob pageBlob)
    {
      Console.WriteLine("Save file? Y/N");

      var result = Console.ReadLine();

      if (String.IsNullOrEmpty(result))
      {
        return;
      }

      switch (result.ToLower())
      {
        case "y":
          {
            blobDownloader.SavePageBlob(container, pageBlob);
          } break;
        default:
          {
            Console.WriteLine("welp");
            break;
          }
      }
    }
  }
}
