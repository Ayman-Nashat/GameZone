using GameZone.Attributes;
using GameZone.Settings;

namespace GameZone.ViewModel
{
    public class EditGameFormViewModel : GameFormViewModel
    {
        public int Id { get; set; }

        public string? CurrentCover { get; set; }

        [AllowedExtensions(FileSettings.AllowedExtensions),
            MaxFileSize(FileSettings.MAxFileSizeInBytes)]
        public IFormFile? Cover { get; set; } = default!;
    }
}
