using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameZone.Services
{
    public interface ICategoriesServices
    {
        IEnumerable<SelectListItem> GetSelectList();
    }
}
