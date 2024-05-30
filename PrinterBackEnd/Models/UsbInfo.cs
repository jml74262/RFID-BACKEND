namespace PrinterBackEnd.Models
{
    public class UsbInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public UsbInfo(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }

}
