namespace ProductTransferService.SportDataBase.Model
{
    public class LeagueName
    {
        public string version_key = string.Empty;

        public List<LangName> names = new List<LangName>();
    }

    public class TeamName
    {
        public string version_key = string.Empty;

        public List<LangName> names = new List<LangName>();
    }

    public class LangName
    {
        public string Lang { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
    }
}