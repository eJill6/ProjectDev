namespace MS.Core.MM.Attributes
{
    public class DescriptionAttributeBase : Attribute
    {
        public DescriptionAttributeBase(string description)
        {
            Description = description;
        }

        public string Description { get; set; }
    }
}
