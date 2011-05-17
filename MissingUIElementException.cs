using System.IO;

namespace WinUIDriver
{
    public class MissingUIElementException : DirectoryNotFoundException
    {
        public MissingUIElementException(string message)
            : base(message)
        {

        }
    }
}