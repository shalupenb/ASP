namespace ASP.Models.Home.URLStruct
{
    public class URLStructPageModel
    {
        public String TabHeader { get; set; } = null!;
        public String PageTitle { get; set; } = null!;
        public List<String> PageText { get; set; } = null!;
        public String PageImageSrc { get; set; } = null!;
    }
}
