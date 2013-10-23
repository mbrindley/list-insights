namespace ListInsight.Common
{
    public interface IDnsMx
    {
        string[] GetMxRecords(string domain);
    }
}