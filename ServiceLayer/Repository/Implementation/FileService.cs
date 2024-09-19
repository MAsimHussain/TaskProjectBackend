using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ServiceLayer.Services.Interface;
using System.IO;
namespace ApplicationLayer.Repository.Implementation
{
    public class FileService:IFileService
    {
        private readonly IHostingEnvironment _webHostEnvironment;
        public FileService(IHostingEnvironment hostEnvironment)
        {
            _webHostEnvironment = hostEnvironment;

        }

        public async Task<bool> DeleteFileAsync(string fileName)
        {

            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string filePath = Path.Combine(wwwRootPath, "Images", fileName);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            return true;
        }

        public async Task<string?> UploadFileAsync(IFormFile profileImageFile)
        {
            if (profileImageFile != null && profileImageFile.Length > 0)
            {
                string uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");

                if (!Directory.Exists(uploadFolderPath))
                {
                    Directory.CreateDirectory(uploadFolderPath);
                }

                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(profileImageFile.FileName);

                string logoFilePath = Path.Combine(uploadFolderPath, uniqueFileName);

                using (var fileStream = new FileStream(logoFilePath, FileMode.Create))
                {
                    await profileImageFile.CopyToAsync(fileStream);
                }

                return uniqueFileName;
            }

            return null;
        }
    }
}
