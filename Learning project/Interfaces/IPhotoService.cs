using CloudinaryDotNet.Actions;

namespace Learning_project.Interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file); 
        Task<DeletionResult> DeletePhotoAsync(string publicId);

    }
}
