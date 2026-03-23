
namespace ASC.Web.Configuration
{
    public class ApplicationSettings
    {
        public string ApplicationName { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public int PageSize { get; set; }
        public bool EnableFeatureX { get; set; }
    }
}
