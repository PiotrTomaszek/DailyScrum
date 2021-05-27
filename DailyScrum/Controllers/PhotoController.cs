using DailyScrum.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DailyScrum.Controllers
{
    public class PhotoController : Controller
    {
        private static string connString = "DefaultEndpointsProtocol=https;AccountName=dailyscrumproejctstorage;AccountKey=8SQzNgRdOSfoXzlTpgxN+IOuURxcQraInTrNMkfl71PHnDV1izWgjkYJoVwEQ7aY2pU0RaLL+6x55ZO7b1TSwA==;EndpointSuffix=core.windows.net";
        private CloudStorageAccount _cloudStorage = CloudStorageAccount.Parse(connString);

        private readonly IUserRepository _userRepository;

        public PhotoController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task UploadFile(IFormFile file, string userId)
        {
            if (file == null || string.IsNullOrEmpty(userId))
            {
                return;
            }

            var cloudBlobClient = _cloudStorage.CreateCloudBlobClient();
            var cloudBlobContainer = cloudBlobClient.GetContainerReference("images");

            try
            {
                if (await cloudBlobContainer.CreateIfNotExistsAsync())
                {
                    await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions
                    {
                        PublicAccess = BlobContainerPublicAccessType.Off
                    });
                }
            }
            catch (System.Exception)
            {
            }

            // to remove

            var url = _userRepository.GetPhotoPathById(userId);
            url = url.Substring(url.LastIndexOf('/') + 1);

            var cloudBlockBlobToDelete = cloudBlobContainer.GetBlockBlobReference(url);
            await cloudBlockBlobToDelete.DeleteIfExistsAsync();

            // to save
            var img = Image.Load(file.OpenReadStream());

            var finalSize = img.Width > img.Height ? img.Height : img.Width;

            var x = ((img.Width - 256) / 4);
            var y = (img.Height - 256) / 4;

            img.Mutate(mut => mut
            .Crop(new Rectangle(x, 0, finalSize, finalSize))
            .Resize(256, 256));

            var encoder = new JpegEncoder()
            {
                Quality = 30
            };

            var memoryStream = new MemoryStream();

            img.Save(memoryStream, encoder);

            var result = memoryStream.ToArray();

            var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(Guid.NewGuid().ToString());

            cloudBlockBlob.Properties.ContentType = file.ContentType;

            await cloudBlockBlob.UploadFromByteArrayAsync(result, 0, result.Length);

            _userRepository.SetPhotoPathById(userId, cloudBlockBlob.Uri.AbsoluteUri);
        }
    }
}
