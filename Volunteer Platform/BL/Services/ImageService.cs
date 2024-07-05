using AutoMapper;
using BL.IServices;
using BL.Models;
using DAL.IRepositories;
using DAL.Models;



namespace BL.Services;


    public class ImageService : IImageService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public ImageService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }
        public ICollection<ImageDto> GetAll()
        {
            var allImages = _repositoryManager.Image.GetAll();
            return _mapper.Map<ICollection<ImageDto>>(allImages).ToList();
        }

        public ImageDto? GetById(int id)
        {
            var imageToGet = _repositoryManager.Image.GetFirstOrDefault(t => t.Id == id);
            if (imageToGet == null) return null;
            return _mapper.Map<ImageDto>(imageToGet);
        }

        public ImageDto? GetByFileName(string FileName)
        {
            var imageToGet = _repositoryManager.Image.GetFirstOrDefault(t => t.FileName == FileName);
            if (imageToGet == null) return null;
            return _mapper.Map<ImageDto>(imageToGet);
        }

    

        public ImageDto? GetDefaultImage()
        {
	       var defaultImg= _repositoryManager.Image.GetFirstOrDefault(i => i.FileName == "default.jpeg");
           return _mapper.Map<ImageDto>(defaultImg);
	}


        public ImageDto Create(ImageDto imageDto)
    {
        var existingImage = _repositoryManager.Image.GetFirstOrDefault(i => i.FileName == imageDto.FileName);
        if (existingImage != null)
        {
            return new ImageDto
            {
                Id = existingImage.Id,
                FileName = existingImage.FileName,
                FilePath = existingImage.FilePath,
                ContentType = existingImage.ContentType
            };
        }

        var newImage = _mapper.Map<Image>(imageDto);
        _repositoryManager.Image.Add(newImage);
        _repositoryManager.Save();

        return _mapper.Map<ImageDto>(newImage);
    }
   
}
