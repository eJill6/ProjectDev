namespace EncryptImagesTool
{
    public partial class EncryptImagesTool : Form
    {
        private static readonly string s_aesExtension = ".aes";
        private static readonly string s_pngExtension = ".png";
        private static readonly string s_jpgExtension = ".jpg";
        private static readonly string s_svgExtension = ".svg";
        
        private static readonly HashSet<string> s_fileExtension = new HashSet<string> 
        {
            s_pngExtension, 
            s_jpgExtension, 
            s_svgExtension 
        };


        public EncryptImagesTool()
        {
            InitializeComponent();
        }

        private void BtnSelectPath_Click(object sender, EventArgs e)
        {
            if (IsPickDirectory)
            {
                var folderBrowserDialog = new FolderBrowserDialog();
                DialogResult dialogResult = folderBrowserDialog.ShowDialog();

                if (dialogResult != DialogResult.OK)
                {
                    return;
                }

                txtSelectPath.Text = folderBrowserDialog.SelectedPath;
            }
            else
            {
                var openFileDialog = new OpenFileDialog();

                if (IsEncryptAction)
                {
                    string imageFilter = string.Join(";", s_fileExtension.Select(s => $"*{s}"));
                    openFileDialog.Filter = $"圖片檔案({imageFilter})|{imageFilter}";
                }
                else
                {
                    openFileDialog.Filter = $"AES 檔案 ({s_aesExtension})|*{s_aesExtension}";
                }

                DialogResult dialogResult = openFileDialog.ShowDialog();

                if (dialogResult != DialogResult.OK)
                {
                    return;
                }

                txtSelectPath.Text = openFileDialog.FileName;
            }
        }

        private void BtnExecute_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSelectPath.Text))
            {
                lblResult.Text = "請選擇路徑";
                return;
            }

            progress.Value = 0;
            lblSProcessedCount.Text = "0";
            lblResult.Text = "-";

            if (IsEncryptAction)
            {
                DoEncrypt();
            }
            else
            {
                DoDescrypt();
            }
        }

        private void DoEncrypt()
        {
            DoProcessImageJob((imageFileInfo) => CreateAesImage(imageFileInfo));
        }

        private void DoDescrypt()
        {
            DoProcessImageJob((imageFileInfo) => CreateImageFromAes(imageFileInfo));
        }

        private void DoProcessImageJob(Action<FileInfo> doJob)
        {
            List<FileInfo> imageFileInfos = GetProcessFileInfos();
            int totalCount = imageFileInfos.Count;
            progress.Maximum = totalCount;
            lblTotalCount.Text = totalCount.ToString();

            try
            {
                foreach (FileInfo imageFileInfo in imageFileInfos)
                {
                    doJob.Invoke(imageFileInfo);
                    progress.Value++;
                    lblProcessedCount.Text = progress.Value.ToString();
                    Task.Delay(200).Wait();
                }

                lblResult.Text = "處理完成";
            }
            catch (Exception ex)
            {
                lblResult.Text = ex.Message;
            }
        }

        private List<FileInfo> GetProcessFileInfos()
        {
            List<FileInfo> fileInfos = new List<FileInfo>();

            string path = txtSelectPath.Text;
            if (File.Exists(path))
            {
                fileInfos.Add(new FileInfo(path));
            }
            else if (Directory.Exists(path))
            {

                List<string> allDirectories = GetAllDirectories();
                fileInfos = GetAllImageFileInfos(allDirectories);
            }

            return fileInfos;
        }

        private List<FileInfo> GetAllImageFileInfos(List<string> allDirectories)
        {
            var allFileInfos = new List<FileInfo>();

            for (int i = 0; i < allDirectories.Count; i++)
            {
                string folderPath = allDirectories[i];
                List<FileInfo> folderFileInfos = Directory.GetFiles(folderPath).Select(s => new FileInfo(s)).ToList();
                List<FileInfo> imageFileInfos = null;

                if (IsEncryptAction)
                {
                    imageFileInfos = folderFileInfos.Where(w => s_fileExtension.Contains(w.Extension.ToLower())).ToList();
                }
                else
                {
                    imageFileInfos = folderFileInfos.Where(w => w.Extension.ToLower() == s_aesExtension).ToList();
                }

                allFileInfos.AddRange(imageFileInfos);
            }

            return allFileInfos;
        }

        private void CreateAesImage(FileInfo fileInfo)
        {
            byte[] fileContent = File.ReadAllBytes(fileInfo.FullName);
            byte[] encryptContent = AESUtil.Encrypt(fileContent);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileInfo.FullName);
            string newFullFilePath = Path.Combine(fileInfo.DirectoryName, $"{fileNameWithoutExtension}{s_aesExtension}");

            File.WriteAllBytes(newFullFilePath, encryptContent);
        }

        private void CreateImageFromAes(FileInfo fileInfo)
        {
            byte[] fileContent = File.ReadAllBytes(fileInfo.FullName);
            byte[] encryptContent = AESUtil.Decrypt(fileContent);
            string newFullFilePath = Path.Combine(fileInfo.DirectoryName, fileInfo.Name + s_pngExtension);

            File.WriteAllBytes(newFullFilePath, encryptContent);
        }

        private List<string> GetAllDirectories()
        {
            string basePath = txtSelectPath.Text;
            List<string> allDirectories = Directory.GetDirectories(basePath, "*", SearchOption.AllDirectories).ToList();
            allDirectories.Insert(0, basePath);

            return allDirectories;
        }

        private bool IsPickDirectory => rdoPickDirectory.Checked;

        private bool IsEncryptAction => rdoEncrypt.Checked;
    }
}