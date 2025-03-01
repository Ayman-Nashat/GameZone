using GameZone.Data;
using GameZone.Models;
using GameZone.Services;
using GameZone.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GameZone.Controllers
{
    public class GamesController : Controller
    {
        private readonly ICategoriesServices _categoriesServices;
        private readonly IDevicesServices _devicesServices;
        private readonly IGamesServices _gamesServices;


        public GamesController(ICategoriesServices categoriesServices,
            IDevicesServices devicesServices,
            IGamesServices gamesServices)
        {
            _categoriesServices = categoriesServices;
            _devicesServices = devicesServices;
            _gamesServices = gamesServices;
        }
        public IActionResult Index()
        {
            var games = _gamesServices.GetAll();
            return View(games);
        }

        public IActionResult Details(int id)
        {
            var game = _gamesServices.GetById(id);
            if (game is null) return NotFound();
            return View(game);
        }
        [HttpGet]
        public IActionResult Create()
        {
            CreateGameFormViewModel model = new CreateGameFormViewModel()
            {
                Categories = _categoriesServices.GetSelectList(),
                Devices = _devicesServices.GetSelectList(),
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGameFormViewModel model)
        {
            if (!ModelState.IsValid) { 
                model.Categories = _categoriesServices.GetSelectList();
                model.Devices = _devicesServices.GetSelectList();
                return View(model);
            }
            await _gamesServices.Create(model);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int id)
        {
            var game = _gamesServices.GetById(id);
            if (game is null) return NotFound();

            EditGameFormViewModel model = new EditGameFormViewModel()
            {
                Categories = _categoriesServices.GetSelectList(),
                Devices = _devicesServices.GetSelectList(),
                Id = id,
                Name = game.Name,
                Description = game.Description,
                SelectedDevices = game.Devices.Select(d => d.DeviceId).ToList(),
                CategoryId = game.CategoryId,
                CurrentCover = game.Cover
            }; 
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditGameFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = _categoriesServices.GetSelectList();
                model.Devices = _devicesServices.GetSelectList();
                return View(model);
            }
            var game = await _gamesServices.Update(model);
            if(game is null) return BadRequest();
            return RedirectToAction(nameof(Index));
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var test = _gamesServices.Delete(id);
            if (test) Ok();
            return BadRequest();
        }
    }
}
