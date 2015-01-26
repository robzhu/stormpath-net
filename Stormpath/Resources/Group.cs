
namespace Stormpath
{
    public class Group : Resource
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ResourceStatus Status { get; set; }
        public Hyperlink CustomData { get; set; }
        public Hyperlink Directory { get; set; }
        public Hyperlink Tenant { get; set; }
        public Hyperlink Accounts { get; set; }
        public Hyperlink AccountMemberships { get; set; }
        public Hyperlink Applications { get; set; }
    }
}