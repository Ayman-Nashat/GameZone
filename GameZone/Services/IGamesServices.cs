using GameZone.Models;
using GameZone.ViewModel;

namespace GameZone.Services
{
    public interface IGamesServices
    {
        IEnumerable<Game> GetAll();
        Task Create(CreateGameFormViewModel model);
        Game? GetById(int id);
        Task<Game?> Update(EditGameFormViewModel model);
        bool Delete(int id);
    }
}
