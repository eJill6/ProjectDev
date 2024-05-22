using Timer = System.Windows.Forms.Timer;

namespace MobileApiCreateSign
{
    public partial class Toast : Form
    {
        private readonly Timer _timer = new Timer();

        public Toast(bool isAutoClose, string message)
        {
            InitializeComponent();
            Rectangle workingArea = Screen.GetWorkingArea(this);
            this.Location = new Point(workingArea.Right - Size.Width,
                                      workingArea.Bottom - Size.Height);
            MinimizeBox = false;
            MaximizeBox = false;

            txtMsg.Text = message;

            if (isAutoClose)
            {
                _timer.Start();
                _timer.Interval = 1500;
                _timer.Tick += Timer_Tick;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _timer.Stop();
            Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}