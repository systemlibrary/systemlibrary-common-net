namespace SystemLibrary.Common.Net.Tests.Configs
{
    public class CarProperty
    {
        public string lastName { get; set; }
        public string firstName { get; set; }
    }

    public class CarSettings : Config<CarSettings>
    {
        public string FirstName { get; set; }
        public string lastname { get; set; }
        public int age { get; set; }
        public bool isEnabled { get; set; }
        public CarProperty Car { get; set; }
    }
}
