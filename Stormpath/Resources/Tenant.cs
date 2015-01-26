
namespace Stormpath
{
    public class Tenant : Resource
    {
        public string Name { get; set; }
        public string Key { get; set; }
        public Hyperlink CustomData { get; set; }
        public ResourceList<Application> Applications { get; set; }
        public Hyperlink Directories { get; set; }
        public Hyperlink Accounts { get; set; }
        public Hyperlink Groups { get; set; }
    }
}