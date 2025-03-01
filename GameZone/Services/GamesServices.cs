using GameZone.Data;
using GameZone.Models;
using GameZone.Settings;
using GameZone.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace GameZone.Services
{
    public class GamesServices : IGamesServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public readonly string _imagesPath;

        public GamesServices(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _imagesPath = $"{_webHostEnvironment.WebRootPath}{FileSettings.ImagePath}";
        }
        private async Task<string> SaveCover(IFormFile Cover)
        {
            var coverName = $"{Guid.NewGuid()}{Path.GetExtension(Cover.FileName)}";

            var path = Path.Combine(_imagesPath, coverName);

            using var stream = File.Create(path);
            await Cover.CopyToAsync(stream);
            return coverName;
        }
        public IEnumerable<Game> GetAll()
        {
            return _context.Games
                .Include(x => x.Category)
                .Include(x => x.Devices)
                .ThenInclude(d => d.Device)
                .AsNoTracking()
                .ToList();
        }
        public Game? GetById(int id)
        {
            return _context.Games
                .Include(x => x.Category)
                .Include(x => x.Devices)
                .ThenInclude(d => d.Device)
                .AsNoTracking()
                .SingleOrDefault(g => g.Id == id);
        }
        public async Task Create(CreateGameFormViewModel model)
        {
            var coverName = await SaveCover(model.Cover);
            Game game = new()
            {
                Name = model.Name,
                Description = model.Description,
                CategoryId = model.CategoryId,
                Cover = coverName,
                Devices = model.SelectedDevices.Select(d => new GameDevice { DeviceId = d }).ToList()
            };
            _context.Games.Add(game);
            _context.SaveChanges();
        }
        public async Task<Game?> Update(EditGameFormViewModel model)
        {
            var game = _context.Games
                .Include(g => g.Devices)
                .SingleOrDefault(d => d.Id == model.Id);

            if (game is null) return null;

            var oldcover = game.Cover;

            game.Name = model.Name;
            game.Description = model.Description;
            game.CategoryId = model.CategoryId;
            game.Devices = model.SelectedDevices.Select(d => new GameDevice { DeviceId = d }).ToList();

            if (model.Cover is not null)
                game.Cover = await SaveCover(model.Cover);

            int res = _context.SaveChanges();
            if (res > 0)
            {
                if (model.Cover is not null)
                {
                    var cover2 = Path.Combine(_imagesPath, oldcover);
                    File.Delete(cover2);
                }
                return game;
            }
            var cover = Path.Combine(_imagesPath, game.Cover);
            File.Delete(cover);
            return null;

        }

        public bool Delete(int id)
        {
            var isDeleted = false;

            var game = _context.Games.Find(id);

            if (game is null)
                return isDeleted;

            _context.Remove(game);
            var effectedRows = _context.SaveChanges();

            if (effectedRows > 0)
            {
                isDeleted = true;

                var cover = Path.Combine(_imagesPath, game.Cover);
                File.Delete(cover);
            }

            return isDeleted;
        }
    }
}













