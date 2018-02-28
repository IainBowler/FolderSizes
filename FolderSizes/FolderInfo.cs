namespace FolderSizes
{
    internal class FolderInfo
    {
        public string Path { get; set; }
        public long Size { get; set; }

        public FolderInfo(string path, long size)
        {
            Path = path;
            Size = size;
        }
    }
}
