namespace ProductTransferService.AgDataBase.Model
{
    public class XMLFile : XmlContent, IComparable //sort會用到
    {
        public string Name { get; set; }

        public string RemotePath { get; set; }

        public string LocalPath { get; set; }

        public DateTime LastModified { get; set; }

        public bool IsSkip { get; set; }

        /// <summary>是否刪除遠端檔案</summary>
        public bool IsDeleteFromRemote { get; set; }

        public int CompareTo(object obj)
        {
            int res = 0;
            XMLFile sObj = (XMLFile)obj;
            if (this.LastModified > sObj.LastModified)
            {
                res = 1;
            }
            else if (this.LastModified < sObj.LastModified)
            {
                res = -1;
            }

            return res;
        }
    }
}