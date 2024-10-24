namespace HexDumpApp
{
    public interface IView
    {
        string GetFilePath();
        void ShowHexDump(string hexDump);
        void ShowError(string message);
    }
}
