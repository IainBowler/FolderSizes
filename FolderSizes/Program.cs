using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FolderSizes
{
    class Program
    {
        private static List<FolderInfo> _folders = new List<FolderInfo>();
        private static List<string> _inaccessibleFolders = new List<string>();

        static void Main(string[] args)
        {
            PopulateFoldersList();

            _folders = _folders.OrderByDescending(fi => fi.Size).ToList();

            ExportFoldersListToFile();
        }

        private static void PopulateFoldersList()
        {
            foreach (var driveInfo in DriveInfo.GetDrives())
            {
                GetFolderSize(driveInfo.RootDirectory);
            }
        }

        private static long GetFolderSize(DirectoryInfo directoryInfo)
        {
            long folderSize = 0;

            FileInfo[] fileInfoArray = new FileInfo[0];

            try
            {
                fileInfoArray = directoryInfo.GetFiles();
            }
            catch (UnauthorizedAccessException e)
            {
                _inaccessibleFolders.Add(directoryInfo.FullName);
                return 0;
            }

            foreach (FileInfo fileInfo in fileInfoArray)
            {
                folderSize += fileInfo.Length;
            }

            DirectoryInfo[] subDirectoryInfoArray = directoryInfo.GetDirectories();

            foreach (DirectoryInfo subDirectoryInfo in subDirectoryInfoArray)
            {
                folderSize += GetFolderSize(subDirectoryInfo);
            }

            _folders.Add(new FolderInfo(directoryInfo.FullName, folderSize));

            return folderSize;
        }

        private static void ExportFoldersListToFile()
        {
            using (StreamWriter writetext = new StreamWriter("FolderSizes.txt"))
            {
                foreach (var item in _folders)
                {
                    var sizeString = item.Size.ToString().PadLeft(15) + " ";
                    writetext.WriteLine(sizeString + item.Path);
                }
            }

            using (StreamWriter writetext = new StreamWriter("InaccessibleFolders.txt"))
            {
                foreach (var item in _inaccessibleFolders)
                {
                    writetext.WriteLine(item);
                }
            }
        }
    }
}
