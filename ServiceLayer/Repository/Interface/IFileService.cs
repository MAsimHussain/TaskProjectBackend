
using Microsoft.AspNetCore.Http;

namespace ServiceLayer.Services.Interface
{
    public interface IFileService
    {


        Task<bool> DeleteFileAsync(string fileName);
        Task<string?> UploadFileAsync(IFormFile profileImageFile);


    }
}
