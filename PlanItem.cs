using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public abstract class BasePlanItem : IPlanItem
    {
        private DateTime date;
        private string job;
        private Point fromTime;
        private Point toTime;
        private string status;
        private string Note;

        public static List<string> liststatus = new List<string>() { "Done", "Late", "Coming", "Doing" };

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        public string Job
        {
            get { return job; }
            set { job = value; }
        }

        public Point FromTime
        {
            get { return fromTime; }
            set { fromTime = value; }
        }

        public Point ToTime
        {
            get { return toTime; }
            set { toTime = value; }
        }

        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        public string Note1
        {
            get { return Note; }
            set { Note = value; }
        }

        protected BasePlanItem()
        {
            date = DateTime.Now;
        }

        public virtual void UpdateStatus(StatusEnum newStatus)
        {
            status = liststatus[(int)newStatus];
        }

        public virtual int GetDuration()
        {
            return (ToTime.X * 60 + ToTime.Y) - (FromTime.X * 60 + FromTime.Y);
        }

        public abstract bool IsCompleted();
    }

    [Serializable]
    public class PlanItem : BasePlanItem
    {
        public PlanItem() : base()
        {
        }

        public PlanItem(DateTime date, string job, Point fromTime, Point toTime)
        {
            this.Date = date;
            this.Job = job;
            this.FromTime = fromTime;
            this.ToTime = toTime;
            this.Status = liststatus[(int)StatusEnum.Coming];
        }

        public override bool IsCompleted()
        {
            return Status == liststatus[(int)StatusEnum.Done];
        }

        public bool IsDoing()
        {
            return Status == liststatus[(int)StatusEnum.Doing];
        }

        public bool IsLate()
        {
            return Status == liststatus[(int)StatusEnum.Late];
        }

        public bool IsComing()
        {
            return Status == liststatus[(int)StatusEnum.Coming];
        }

        public bool IsToday()
        {
            return Date.Date == DateTime.Now.Date;
        }
    }

    [Serializable]
        // Xử lý công việc lặp lại 
    public class RecurringPlanItem : BasePlanItem
    {
        private DayOfWeek recurringDay;

        public DayOfWeek RecurringDay
        {
            get { return recurringDay; }
            set { recurringDay = value; }
        }

        public RecurringPlanItem() : base()
        {
            recurringDay = DateTime.Now.DayOfWeek;
        }

        public override bool IsCompleted()
        {
            return false; 
        }

        public bool IsRecurringDay()
        {
            return DateTime.Now.DayOfWeek == recurringDay;
        }

        public override void UpdateStatus(StatusEnum newStatus)
        {
            if (IsRecurringDay())
            {
                base.UpdateStatus(newStatus);
            }
        }
    }

    [Serializable]
    // Thêm deadline khi mà công việc quá thời gian sẽ chuyển sang late 
    public class UrgentPlanItem : BasePlanItem
    {
        private DateTime deadline;
        private int priority;

        public DateTime Deadline
        {
            get { return deadline; }
            set { deadline = value; }
        }

        public int Priority
        {
            get { return priority; }
            set { priority = value; }
        }

        public UrgentPlanItem() : base()
        {
            deadline = DateTime.Now.AddDays(1);
            priority = 1;
        }

        public override bool IsCompleted()
        {
            return Status == liststatus[(int)StatusEnum.Done];
        }

        public override void UpdateStatus(StatusEnum newStatus)
        {
            base.UpdateStatus(newStatus);
            if (DateTime.Now > Deadline && !IsCompleted())
            {
                Status = liststatus[(int)StatusEnum.Late];
            }
        }

        public bool IsOverdue()
        {
            return DateTime.Now > Deadline && !IsCompleted();
        }
    }

    public enum StatusEnum
    {
        Done,
        Late,
        Coming,
        Doing
    }
}
