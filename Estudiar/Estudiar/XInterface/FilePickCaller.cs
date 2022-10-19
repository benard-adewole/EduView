using Estudiar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Estudiar.Models;

namespace Estudiar.XInterface
{
    public interface FilePickCaller
    {
        Task<byte[]> CompressImageAsync(byte[] Mybyte, float Width, float Height);
        //Task<List<FilePickedModel>> GetFilesStream();
        Task<List<string>> GetFiles();
        Task<string> GetFilePath();
        Task<string> PickCopyAsync();
        Task<List<IncomingFile>> GetFileStreams();
        Task<IncomingFile> GetFileStream();
    }
}
