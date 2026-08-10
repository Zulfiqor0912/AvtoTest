namespace AvtoTest.Data.Entities.TestEntities;

public class AnonymousUser
{
    public Guid Id { get; set; }
    public int TestCount { get; set; } = 0;
    public string IpAddress { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
    public string BrowseType { get; set; } = string.Empty;
    public string DeviceType { get; set; } = string.Empty;
    public string OperatingSystem { get; set; } = string.Empty;
    public DateTime CreateAt { get; set; } = DateTime.Now; //birinchi marta saytga kirganim
    public DateTime LastVisited { get; set; } = DateTime.Now; //oxirgi marta qachon kirganim
    public DateTime? LastTestAt { get; set; } = null; //Oxirgi marta qachon test ishlaganim
}
