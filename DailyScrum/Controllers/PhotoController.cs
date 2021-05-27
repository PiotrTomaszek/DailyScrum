using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Threading.Tasks;

namespace DailyScrum.Controllers
{
    public class PhotoController : Controller
    {
        private static string connString = "DefaultEndpointsProtocol=https;AccountName=dailyscrumproejctstorage;AccountKey=8SQzNgRdOSfoXzlTpgxN+IOuURxcQraInTrNMkfl71PHnDV1izWgjkYJoVwEQ7aY2pU0RaLL+6x55ZO7b1TSwA==;EndpointSuffix=core.windows.net";
        private CloudStorageAccount _cloudStorage = CloudStorageAccount.Parse(connString);

        public async Task UploadFile(IFormFile file)
        {
            if (file == null)
            {
                return;
            }

            var cloudBlobClient = _cloudStorage.CreateCloudBlobClient();
            var cloudBlobContainer = cloudBlobClient.GetContainerReference("images");

            if (await cloudBlobContainer.CreateIfNotExistsAsync())
            {
                await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Off
                });
            }

            var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(file.FileName);

            cloudBlockBlob.Properties.ContentType = file.ContentType;

            await cloudBlockBlob.UploadFromStreamAsync(file.OpenReadStream());
        }

        //public async Task UploadPhotoToSession(IFormFile filer)
        //{
        //    if (filer == null)
        //    {
        //        return;
        //    }



        //    HttpContext.Session.Set()

        //}

        //public byte[] ImageToByteArray(Image imageIn)
        //{
        //    using (var ms = new MemoryStream())
        //    {
        //        imageIn.Save(ms, imageIn.RawFormat);
        //        return ms.ToArray();
        //    }
        //}
    }
}
