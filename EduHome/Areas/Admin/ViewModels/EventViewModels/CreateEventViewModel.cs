namespace EduHome.Areas.Admin.ViewModels.EventViewModels
{
    public class CreateEventViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public string Time { get; set; }
        public string Venue { get; set; }
        public string Date { get; set; }
        public List<int>? SpeakerId { get; set; }
    }
}
