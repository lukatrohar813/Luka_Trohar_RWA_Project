using BL.Models;

namespace BL.IServices;

public interface ITypeService
{
    ICollection<TypeDto> GetAll();
    TypeDto? GetById(int? id);
    TypeDto Create(TypeDto type);
    TypeDto? Update(int id, TypeDto type);
    TypeDto? Delete(int id);
    
}