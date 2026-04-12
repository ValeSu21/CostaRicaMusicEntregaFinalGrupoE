using CostaRicaMusicBLL.DTOs;
namespace CostaRicaMusicBLL.Interfaces;
public interface IPlaylistService
{
    List<PlaylistDto> GetByUser(string email);
    PlaylistDto? GetById(int id);
    int Create(PlaylistDto dto);
    void Update(PlaylistDto dto);
    void Delete(int id);
    void AddSong(int playlistId, int songId);
    void RemoveSong(int playlistId, int songId);
}
