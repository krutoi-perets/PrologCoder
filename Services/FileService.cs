using System.IO;

namespace PrologCoder.Services
{
    public class FileService
    {
        public string OpenFile(string path)
        {
            return File.ReadAllText(path);
        }

        public void SaveFile(string path, string text)
        {
            if (path == null)
                throw new InvalidOperationException("Файл еще не имеет пути");
            File.WriteAllText(path, text);
        }
    }
}
