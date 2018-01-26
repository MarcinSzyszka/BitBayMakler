namespace BitBayCommon.Settings
{
    public interface ISettings
    {
        string PublicApiUrl { get; set; }
        string DataDirectoryPath { get; set; }
    }
}
