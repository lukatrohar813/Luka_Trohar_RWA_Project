using BL.Models;


namespace BL.IServices;

public interface IImageService
{
    ICollection<ImageDto> GetAll();
    ImageDto? GetById(int id);
    ImageDto Create(ImageDto image);
    ImageDto? GetByFileName(string FileName);
 
    ImageDto? GetDefaultImage();

}