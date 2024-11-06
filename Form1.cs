using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using System.Xml.Serialization;
using Microsoft.Win32;

namespace ThiCuoiKy
{
    [Serializable]
    public partial class Form1 : Form
    {
        private Cons cons;

        private NotifyIcon notify;
        private List<List<Button>> matrix;

        private List<string> dateOfWeek = new List<string>() { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

        private Jobstatic jobstatic;
        private DateTime date;
        private PlanData job;
        private int time;
        private int appTime;

        public int AppTime
        {
            get { return appTime; }
            set { appTime = value; }
        }
        public int Time
        {
            get { return time; }
            set { time = value; }
        }
        public NotifyIcon Notify
        {
            get { return notify; }
            set { notify = value; }
        }

        public List<List<Button>> Matrix
        {
            get { return matrix; }
            set { matrix = value; }
        }
        public DateTime Date { get => date; set => date = value; }
        public PlanData Job { get => job; set => job = value; }
        public Jobstatic Jobstatic { get => jobstatic; set => jobstatic = value; }
        public Cons Cons { get => cons; set => cons = value; }

        private string filePath = "data.xml";

        private NotifyManager notifyManager;
        public Form1()
        {
            InitializeComponent();

            ModeScreen();

            // Khởi tạo NotifyIcon
            Notify = new NotifyIcon();

            // Reset đếm thời gian
            appTime = 0;

            // Khởi tạo NotifyManager
            InitializeNotification();

            LoadMtrix();

            try
            {
                Job = DeserializeFromXML(filePath) as PlanData;
            }
            catch
            {
                SetDefualData();

            }

        }
        private void SerializeToXML(object data, string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            XmlSerializer sr = new XmlSerializer(typeof(PlanData));

            sr.Serialize(fs, data);

            fs.Close();
        }

        private object DeserializeFromXML(string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            try
            {
                XmlSerializer sr = new XmlSerializer(typeof(PlanData));

                object result = sr.Deserialize(fs);
                fs.Close();
                return result;
            }
            catch (Exception)
            {
                fs.Close();
                throw new NotImplementedException();
            }
        }



        private void Form1_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            SerializeToXML(Job, filePath);
        }


        void SetDefualData()
        {
            Job = new PlanData();
            Job.Job = new List<PlanItem>();
            Job.Job.Add(new PlanItem()
            {
                Date = DateTime.Now,
                FromTime = new Point(4, 0),
                ToTime = new Point(5, 0),
                Job = "test",
                Status = PlanItem.liststatus[(int)StatusEnum.Coming]
            });

            Job.Job.Add(new PlanItem()
            {
                Date = DateTime.Now,
                FromTime = new Point(4, 0),
                ToTime = new Point(5, 0),
                Job = "test",
                Status = PlanItem.liststatus[(int)StatusEnum.Coming]
            });

            Job.Job.Add(new PlanItem()
            {
                Date = DateTime.Now,
                FromTime = new Point(4, 0),
                ToTime = new Point(5, 0),
                Job = "test",
                Status = PlanItem.liststatus[(int)StatusEnum.Coming]
            });

            Job.Job.Add(new PlanItem()
            {
                Date = DateTime.Now,
                FromTime = new Point(4, 0),
                ToTime = new Point(5, 0),
                Job = "test",
                Status = PlanItem.liststatus[(int)StatusEnum.Coming]
            });


        }

        void LoadMtrix()
        {

            Matrix = new List<List<Button>>();
            Button preBtn = new Button() { Width = 40, Height = 40, Location = new Point(-Cons.Margin, 0) };
            for (int i = 0; i < Cons.Row; i++)
            {

                Matrix.Add(new List<Button>());

                for (int j = 0; j < Cons.Column; j++)
                {

                    Button button = new Button() { Width = Cons.SizeBtnWidth, Height = Cons.SizeBtnHeight };
                    button.Location = new Point(preBtn.Location.X + preBtn.Width + Cons.Margin, preBtn.Location.Y);

                    pnlMatrix.Controls.Add(button);
                    button.Click += btn_Click;

                    Matrix[i].Add(button);

                    preBtn = button;
                }
                preBtn = new Button()
                {
                    Width = 40,
                    Height = 40,
                    Location = new Point(-Cons.Margin, preBtn.Location.Y + Cons.SizeBtnHeight)
                };
            }
            DefualtDate();
            // AddNunmberMatrixByDate(dtpkDate.Value);
        }

        private void btn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty((sender as Button).Text))
                return;
            DailyPlan daily = new DailyPlan(new DateTime(dtpkDate.Value.Year, dtpkDate.Value.Month, Convert.ToInt32((sender as Button).Text)), Job);
            daily.ShowDialog();
        }

        int DayOfMonth(DateTime date)
        {
            switch (date.Month)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    return 31;
                case 2:
                    if ((date.Year % 4 == 0 && date.Year % 100 != 0) || date.Year % 400 == 0)
                        return 29;
                    else
                        return 28;

                default:
                    return 30;

            }
        }

        void DefualtDate()
        {
            dtpkDate.Value = DateTime.Now;
        }

        bool cmpDate(DateTime a, DateTime b)
        {
            return a.Year == b.Year && a.Month == b.Month && a.Day == b.Day;
        }
        void AddNunmberMatrixByDate(DateTime date)
        {
            ClearMatrix();

            DateTime useDate = new DateTime(date.Year, date.Month, 1);

            int line = 0;
            for (int i = 1; i <= DayOfMonth(date); i++)
            {

                int column = dateOfWeek.IndexOf(useDate.DayOfWeek.ToString());
                Button button = Matrix[line][column];
                button.Text = i.ToString();

                if (cmpDate(useDate, DateTime.Now))
                {
                    button.BackColor = Color.DeepPink;
                }
                if (cmpDate(useDate, date))
                {
                    button.BackColor = Color.Aqua;
                }

                if (column >= 6)
                    line++;

                useDate = useDate.AddDays(1);
            }

        }

        void ClearMatrix()
        {
            for (int i = 0; i < Matrix.Count; i++)
            {
                for (int j = 0; j < Matrix[i].Count; j++)
                {
                    Button btn = Matrix[i][j];
                    btn.Text = "";

                    btn.BackColor = Color.Silver;
                }

            }
        }

        //Phương thức để quản lý và hiển thị các thông báo trong ứng dụng dựa trên trạng thái của checkbox ckbNotify.
        private void InitializeNotification()
        {
            // Tạo mới NotifyManager
            notifyManager = new NotifyManager();

            // Trạng thái bật/tắt thông báo dựa trên checkbox
            notifyManager.IsEnabled = ckbNotify.Checked;
        }

        // Event handler được gọi khi Timer tmNotify được kích hoạt hoặc "tick" (tức là đến thời điểm đã được thiết lập).
        private void tmNotify_Tick(object sender, EventArgs e)
        {
            // Nếu checkbox thông báo không được chọn thì không chạy thông báo
            if (notifyManager == null)
            {
                // Khởi tạo NotifyManager nếu chưa có
                InitializeNotification();
                return;
            }
            
            // Tăng biến đếm thời gian
            if (!notifyManager.IsEnabled)
                return;

            // Tăng bộ đếm thời gian
            AppTime++;

            // Nếu chưa đến thời gian thông báo, thoát ra khỏi hàm
            if (AppTime < Cons.NotifyTime)
            {     
                return;
            }

            // Lấy ngày hiện tại
            DateTime currentDate = DateTime.Now;

            // Kiểm tra các công việc trong ngày hiện tại
            List<PlanItem> todayJobs = GetTodayJobs(currentDate);

            // Kiểm tra xem có cần thông báo không
            if (notifyManager.ShouldNotify(currentDate, Job.Job))
            {
                // Tạo nội dung thông báo
                string message = $"Bạn có {todayJobs.Count} việc cần làm trong ngày hôm nay";

                // Hiển thị thông báo
                notifyManager.ShowNotification(message);
            }

            // Reset bộ đếm thời gian
            AppTime = 0;
        }

        // Phương thức (method) được sử dụng để lấy danh sách các công việc (PlanItem) được thực hiện trong ngày hiện tại.
        private List<PlanItem> GetTodayJobs(DateTime currentDate)
        {
            // Tạo danh sách công việc hôm nay
            List<PlanItem> todayJobs = new List<PlanItem>();
            
            // Lặp qua tất cả các công việc có trong hệ thống
            foreach (PlanItem job in Job.Job)
            {
                // Kiểm tra xem công việc đó có ngày trùng với ngày hiện tại không
                if (job.Date.Year == currentDate.Year &&
                   job.Date.Month == currentDate.Month &&
                   job.Date.Day == currentDate.Day)
                {
                    // Kiểm tra xem trạng thái của công việc có phải là "Coming" hoặc "Doing" không
                    if (job.Status == PlanItem.liststatus[(int)StatusEnum.Coming] ||
                       job.Status == PlanItem.liststatus[(int)StatusEnum.Doing])
                    {
                        // Nếu công việc thỏa mãn các điều kiện trên, thì thêm nó vào danh sách todayJobs
                        // Thêm công việc vào danh sách
                        todayJobs.Add(job);
                    }    
                }    
            }

            // Trả về danh sách công việc của hôm nay
            return todayJobs;
        }
        
        private bool InDay()
        {
            int current = DateTime.Now.Hour;
            return current >= 6 && current < 18;
        }

        private void ModeScreen()
        {
            if(InDay())
            {
                Cons = new LightMode();
                this.BackColor = Color.White;
            }
            else
            {
                Cons = new DarkMode();
                this.BackColor= Color.BlanchedAlmond;
            }
        }
        
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            AddNunmberMatrixByDate((sender as DateTimePicker).Value);
        }
        
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            // Kiểm tra biến notifyManager khởi tạo chưa
            if (notifyManager == null)
            {
                // Khởi tạo NotifyManager nếu chưa có
                InitializeNotification();
            }

            // Cập nhật trạng thái bật/tắt thông báo
            notifyManager.IsEnabled = ckbNotify.Checked;

            // Cập nhật trạng thái của NumericUpDown
            nmNotify.Enabled = ckbNotify.Checked;
        }

        private void nmNotify_ValueChanged(object sender, EventArgs e)
        {
            // Kiểm tra biến notifyManager khởi tạo chưa
            if (notifyManager == null)
            {
                // Khởi tạo NotifyManager nếu chưa có
                InitializeNotification();
            }

            // Reset lại biến đếm thời gian sau khi thông báo
            Cons.NotifyTime = (int)nmNotify.Value;
        }
        
        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Jobstatic.DailyJob(date));
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Jobstatic.DailyJob(date));
        }

        private void btnToday_Click(object sender, EventArgs e)
        {
            dtpkDate.Value = DateTime.Now;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dtpkDate.Value = dtpkDate.Value.AddMonths(1);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            dtpkDate.Value = dtpkDate.Value.AddMonths(-1);
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {
        }

        private void btnMonday_Click(object sender, EventArgs e)
        {
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        }

        private void btnTuesday_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
