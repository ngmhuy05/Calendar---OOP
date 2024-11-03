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
        private Cons screen;

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

        private string filePath = "data.xml";

        private NotifyManager notifyManager;
        
        public Form1()
        {
            InitializeComponent();

            ModeScreen();

            Notify = new NotifyIcon();

            appTime = 0;

            // Khởi tạo NotifyManager trước khi sử dụng
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
            Button preBtn = new Button() { Width = 40, Height = 40, Location = new Point(-screen.Margin, 0) };
            for (int i = 0; i < screen.Row; i++)
            {

                Matrix.Add(new List<Button>());

                for (int j = 0; j < screen.Column; j++)
                {

                    Button button = new Button() { Width = screen.SizeBtnWidth, Height = screen.SizeBtnHeight };
                    button.Location = new Point(preBtn.Location.X + preBtn.Width + screen.Margin, preBtn.Location.Y);

                    pnlMatrix.Controls.Add(button);
                    button.Click += btn_Click;

                    Matrix[i].Add(button);

                    preBtn = button;
                }
                preBtn = new Button()
                {
                    Width = 40,
                    Height = 40,
                    Location = new Point(-screen.Margin, preBtn.Location.Y + screen.SizeBtnHeight)
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

        List<PlanItem> JobByDay(DateTime date)
        {
            List<PlanItem> result = new List<PlanItem>();

            foreach (PlanItem item in Job.Job)
            {
                if (item.Date.Year == date.Year && item.Date.Month == date.Month && item.Date.Day == date.Day)
                {
                    result.Add(item);
                }
            }

            return result;
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

        private void InitializeNotification()
        {
            notifyManager = new NotifyManager();
            notifyManager.IsEnabled = ckbNotify.Checked;
        }
        
        private void tmNotify_Tick(object sender, EventArgs e)
        {
            // Thêm kiểm tra null
            if (notifyManager == null)
            {
                InitializeNotification();
                return;
            }
            
            if (!notifyManager.IsEnabled)
                return;
            
            AppTime++;
            
            if (AppTime < Cons.notifyTime)
                return;
            
            DateTime currentDate = DateTime.Now;
            List<PlanItem> todayJobs = GetTodayJobs(currentDate);
            
            if (notifyManager.ShouldNotify(currentDate, Job.Job))
            {
                string message = $"Bạn có {todayJobs.Count} việc cần làm trong ngày hôm nay";
                notifyManager.ShowNotification(message);
            }
            
            AppTime = 0;
        }

        private List<PlanItem> GetTodayJobs(DateTime currentDate)
        {
            List<PlanItem> todayJobs = new List<PlanItem>();
            foreach (PlanItem job in Job.Job)
            {
                if (job.Date.Year == currentDate.Year &&
                    job.Date.Month == currentDate.Month &&
                    job.Date.Day == currentDate.Day)
                {
                    if (job.Status == PlanItem.ListStatus[(int)EPlanItem.COMING] ||
                        job.Status == PlanItem.ListStatus[(int)EPlanItem.DOING])
                    {
                        todayJobs.Add(job);
                    }
                }
            }
            return todayJobs;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            // Thêm kiểm tra null
            if (notifyManager == null)
            {
                InitializeNotification();
            }
            notifyManager.IsEnabled = ckbNotify.Checked;
            nmNotify.Enabled = ckbNotify.Checked;
        }

        private void nmNotify_ValueChanged(object sender, EventArgs e)
        {
            // Thêm kiểm tra null
            if (notifyManager == null)
            {
                InitializeNotification();
            }
            Cons.notifyTime = (int)nmNotify.Value;
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
                screen = new LightMode();
                this.BackColor = Color.White;
            }
            else
            {
                screen = new DarkMode();
                this.BackColor= Color.BlanchedAlmond;
            }
        }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            AddNunmberMatrixByDate((sender as DateTimePicker).Value);
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

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Jobstatic.DailyJob(date));
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {

            MessageBox.Show(Jobstatic.DailyJob(date));
        }

        private void btnTuesday_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

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

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
