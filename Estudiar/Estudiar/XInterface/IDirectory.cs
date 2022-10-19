using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;

namespace Estudiar.XInterface
{
    public interface IAppDirectory
    {
        string CreateFile(string fileName);
        string CreateDirectory(string directoryName);
        void RemoveDirectory();
        string RenameDirectory(string oldDirectoryName, string newDirectoryName);
    }
    public interface IExternalDirectory 
    {
        Task<List<string>> GetSubFolders(string CurrentFolder);
        Task<List<string>> GetSubFiles(string CurrentFolder);
        bool CanRead();
        string TempFolderDir();
        string RootDir();
        string CacheDirectory();
        string WasteDir();
        string CyBlogFolderDir();
        string CreateDirectory(string DirectoryName);
        string CreateFile(string fileName);
        
        string[] CacheSubDirectories();
    }
}
