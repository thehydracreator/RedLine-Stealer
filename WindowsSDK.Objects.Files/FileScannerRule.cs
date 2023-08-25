using System.Collections.Generic;
using System.IO;

namespace WindowsSDK.Objects.Files
{
    public abstract class FileScannerRule
    {
        public string Name { get; set; }

        public abstract string GetFolder(FileScannerArg scannerArg, FileInfo filePath);

        public abstract IEnumerable<FileScannerArg> GetScanArgs();
    }
}
