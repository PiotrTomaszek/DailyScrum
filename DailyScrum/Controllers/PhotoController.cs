using DailyScrum.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Linq;
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

            var offsetX = img.Width / 4;
            var offsetY = img.Height / 4;

            if (img.Width < 256)
                offsetX = 0;

            if (img.Height < 256)
                offsetY = 0;

            img.Mutate(x => x
            .Crop(new Rectangle(offsetX, offsetY, finalSize, finalSize))
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


        public static Image ResizeImage(Image imgToResize, Size destinationSize)
        {
            var originalWidth = imgToResize.Width;
            var originalHeight = imgToResize.Height;

            //how many units are there to make the original length
            var hRatio = (float)originalHeight / destinationSize.Height;
            var wRatio = (float)originalWidth / destinationSize.Width;

            //get the shorter side
            var ratio = Math.Min(hRatio, wRatio);

            var hScale = Convert.ToInt32(destinationSize.Height * ratio);
            var wScale = Convert.ToInt32(destinationSize.Width * ratio);

            //start cropping from the center
            var startX = (originalWidth - wScale) / 2;
            var startY = (originalHeight - hScale) / 2;

            //crop the image from the specified location and size
            var sourceRectangle = new Rectangle(startX, startY, wScale, hScale);

            //the future size of the image
            var bitmap = new Bitmap(destinationSize.Width, destinationSize.Height);

            //fill-in the whole bitmap
            var destinationRectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            //generate the new image
            using (var g = Graphics.FromImage(bitmap))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(imgToResize, destinationRectangle, sourceRectangle, GraphicsUnit.Pixel);
            }

            return bitmap;

        }
    }
}
