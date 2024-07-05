using AutoMapper;
using BL.IServices;
using BL.Models;
using DAL.IRepositories;

namespace BL.Services;

public class TypeService : ITypeService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;

    public TypeService(IRepositoryManager repositoryManager, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }
    public ICollection<TypeDto> GetAll()
    {
        var allTypes = _repositoryManager.Type.GetAll();
        return _mapper.Map<ICollection<TypeDto>>(allTypes).ToList();
    }

    public TypeDto? GetById(int? id)
    {
        var typeToGet = _repositoryManager.Type.GetFirstOrDefault(t => t.Id == id);
        if (typeToGet == null) throw new InvalidOperationException("Type does not exist");
        return _mapper.Map<TypeDto>(typeToGet);
    }

    public TypeDto Create(TypeDto type)
    {
        var existingTypes = _repositoryManager.Type.GetFirstOrDefault(t => t.Name == type.Name);
        if (existingTypes != null) throw new InvalidOperationException($"Type with Name: {type.Name} already exists");

        var newType = _mapper.Map<DAL.Models.Type>(type);
        _repositoryManager.Type.Add(newType);
        _repositoryManager.Save();
        return _mapper.Map<TypeDto>(newType);
    }

    public TypeDto? Update(int id, TypeDto type)
    {
        var typeToUpdate = _repositoryManager.Type.GetFirstOrDefault(t => t.Id == id);
        if (typeToUpdate == null)
            throw new InvalidOperationException("Type does not exist");

        
        var existingTypeWithSameName = _repositoryManager.Type.GetFirstOrDefault(t => t.Name == type.Name && t.Id != id);
        if (existingTypeWithSameName != null)
            throw new InvalidOperationException($"Type with Name: {type.Name} already exists");

        _mapper.Map(type, typeToUpdate);
        _repositoryManager.Save();
        return _mapper.Map<TypeDto>(typeToUpdate);
    }

    public TypeDto? Delete(int id)
    {
        var typeToDelete = _repositoryManager.Type.GetFirstOrDefault(t => t.Id == id);
        if (typeToDelete == null) throw new InvalidOperationException("Type does not exist");


        _repositoryManager.Type.Delete(typeToDelete);
        _repositoryManager.Save();
        return _mapper.Map<TypeDto>(typeToDelete);
    }
}