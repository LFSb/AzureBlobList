using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AzureBlobList
{
  public class BlobDownloader
  {
    private readonly CloudBlobContainer _container;

    public BlobDownloader(CloudBlobContainer container)
    {
      _container = container;
    }

    public void SavePageBlob(CloudBlobContainer container, CloudBlob blob)
    {
      var blobRef = _container.GetPageBlobReference(blob.Name);

      var path = Path.Combine(ConfigurationManager.AppSettings["BlobSavePath"], blob.Name);

      Console.WriteLine("Will save to: {0}", path);

      using (var fileStream = File.OpenWrite(path))
      {
        var sw = new Stopwatch();
        sw.Start();

        var bla = blobRef.DownloadToStreamAsync(fileStream);

        while (bla.IsCompleted == false)
        {
          Console.WriteLine("Status:{0}-{1}", bla.Status, DateTime.Now.ToShortTimeString());

          Thread.Sleep(1000);
        }
      }
    }

    public void SaveBlockBlob(CloudBlobContainer container, CloudBlockBlob pageBlob)
    {
      var blobRef = _container.GetBlockBlobReference(pageBlob.Name);

      var path = Path.Combine(ConfigurationManager.AppSettings["BlobSavePath"], pageBlob.Name);

      Console.WriteLine("Will save to: {0}", path);

      using (var fileStream = File.OpenWrite(path))
      {
        var sw = new Stopwatch();
        sw.Start();

        var bla = blobRef.DownloadToStreamAsync(fileStream);

        while (bla.IsCompleted == false)
        {
          Console.WriteLine("Status:{0}-{1}", bla.Status, DateTime.Now.ToShortTimeString());

          Thread.Sleep(1000);
        }
      }
    }
  }
}