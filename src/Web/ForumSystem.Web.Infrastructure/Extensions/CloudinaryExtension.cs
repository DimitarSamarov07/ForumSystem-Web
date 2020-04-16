namespace ForumSystem.Web.Infrastructure.Extensions
{
    using System.IO;
    using System.Threading.Tasks;
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class CloudinaryExtension
    {
        public static void AddCloudinary(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            Account account = new Account(
                configuration["Cloudinary:AppName"],
                configuration["Cloudinary:AppKey"],
                configuration["Cloudinary:AppSecret"]);

            Cloudinary cloudinary = new Cloudinary(account);

            services.AddSingleton(cloudinary);
        }

        public static async Task<string> UploadAsync(
            this Cloudinary cloudinary,
            IFormFile file)
        {
            byte[] destinationImage;

            await using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                destinationImage = memoryStream.ToArray();
            }

            await using (var destinationStream = new MemoryStream(destinationImage))
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, destinationStream),
                };

                var result = await cloudinary.UploadAsync(uploadParams);
                return result.SecureUri.AbsoluteUri;
            }
        }
    }
}
