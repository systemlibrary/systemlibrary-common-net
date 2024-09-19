namespace SystemLibrary.Common.Net.Tests.Configurations
{
    public class HumanConfigs : Config<HumanConfigs>
    {
        public string firstname { get; set; }
        public string LastName { get; set; }
        public int Phone { get; set; }
        public bool IsAlive { get; set; }
        public bool IsAliveCapital { get; set; }
    }
}
