using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThiCuoiKy
{

    public interface IPlanItem
    {
        DateTime Date { get; set; }
        string Job { get; set; }
        Point FromTime { get; set; }
        Point ToTime { get; set; }
        string Status { get; set; }
        string Note1 { get; set; }
    }

    [Serializable]
    public class PlanItem : IPlanItem
    {
        private DateTime date;
        private string job;
        private Point fromTime;
        private Point toTime;
        private string status;
        private string Note;

        public static List<string> liststatus = new List<string>() { "Done", "Late", "Coming", "Doing" };

        public DateTime Date { get => date; set => date = value; }
        public string Job { get => job; set => job = value; }
        public Point FromTime { get => fromTime; set => fromTime = value; }
        public Point ToTime { get => toTime; set => toTime = value; }
        public string Status { get => status; set => status = value; }
        public string Note1 { get => Note; set => Note = value; }

    }
    [Serializable]
    public class RecurringPlanItem : PlanItem
    {
        private int repeatInterval;
        private static List<RecurringPlanItem> allRecurringItems = new List<RecurringPlanItem>();

        public class DuplicateJobException : Exception
        {
            public DuplicateJobException(string message) : base(message) { }
        }

        public int RepeatInterval
        {
            get => repeatInterval;
            set => repeatInterval = value;
        }

        // Kiểm tra xem có công việc trùng lặp không
        private bool HasDuplicateJob(DateTime date, string job)
        {
            for (int i = 0; i < allRecurringItems.Count; i++)
            {
                RecurringPlanItem item = allRecurringItems[i];
                if (item.Date.Date == date.Date && item.Job.ToLower() == job.ToLower())
                {
                    throw new DuplicateJobException($"Công việc '{job}' bị trùng lặp trong cùng 1 ngày ");
                }
            }
            return false;
        }

        // Thêm công việc mới với kiểm tra trùng lặp
        public void AddRecurringItem()
        {
            try
            {
                HasDuplicateJob(this.Date, this.Job);
                allRecurringItems.Add(this);
            }
            catch (DuplicateJobException ex)
            {
                throw; // Ném lại ngoại lệ để lớp gọi có thể xử lý
            }
        }

        // Lấy danh sách tất cả công việc
        public static List<RecurringPlanItem> GetRecurringItems()
        {
            List<RecurringPlanItem> a = new List<RecurringPlanItem>();
            for (int i = 0; i < allRecurringItems.Count; i++)
            {
                a.Add(allRecurringItems[i]);
            }
            return a;
        }

        // Constructor
        public RecurringPlanItem()
        {
            repeatInterval = 1;
        }

        public RecurringPlanItem(DateTime date, string job, Point fromTime, Point toTime, int repeatInterval)
        {
            this.Date = date;
            this.Job = job;
            this.FromTime = fromTime;
            this.ToTime = toTime;
            this.repeatInterval = repeatInterval;
            this.Status = "Coming";
        }
    }
    public enum StatusEnum
    {
        Done,
        Late,
        Coming,
        Doing,
    }
}
