using CostaRicaMusicBLL.DTOs;
namespace CostaRicaMusicBLL.Interfaces;
public interface ICancionService
{
    PagedResultDto<CancionDto> GetPaged(string? search, int page, int pageSize);
    CancionDto? GetById(int id);
    List<CancionDto> GetAll();
}
