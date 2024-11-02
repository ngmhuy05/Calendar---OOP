using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThiCuoiKy
{
    [Serializable]
    public class JobStatus
    {
        private PlanData _job;

        public JobStatus(PlanData job)
        {
            _job = job;
        }

        public bool IsMatchingDate(PlanItem item, DateTime date)
        {
            return item.Date.Year == date.Year &&
                   item.Date.Month == date.Month &&
                   item.Date.Day == date.Day;
        }

        public int CountJobsByStatus(DateTime date, StatusEnum status)
        {
            int count = 0;
            foreach (PlanItem item in _job.Job)
            {
                if (IsMatchingDate(item, date) &&
                    PlanItem.liststatus.IndexOf(item.Status) == (int)status)
                {
                    count++;
                }
            }
            return count;
        }

        public int GetDoneJobs(DateTime date)
        {
            return CountJobsByStatus(date, StatusEnum.Done);
        }

        public int GetComingJobs(DateTime date)
        {
            return CountJobsByStatus(date, StatusEnum.Coming);
        }

        public int GetLateJobs(DateTime date)
        {
            return CountJobsByStatus(date, StatusEnum.Late);
        }

        public int GetDoingJobs(DateTime date)
        {
            return CountJobsByStatus(date, StatusEnum.Doing);
        }
    }

    [Serializable]
    public class Jobstatic
    {
        private DateTime date;
        private PlanData job;
        private JobStatus statusCalculator;  

        public Jobstatic(PlanData job)
        {
            this.job = job;
            this.statusCalculator = new JobStatus(job);  
        }

        public PlanData Job
        {
            get => job;
            set => job = value;
        }

        public DateTime Datetime
        {
            get => date;
            set => date = value;
        }

        public int JobByDay(DateTime date)
        {
            int count = 0;
            foreach (PlanItem item in job.Job)
            {
                if (item.Date.Year == date.Year &&
                    item.Date.Month == date.Month &&
                    item.Date.Day == date.Day)
                {
                    count++;
                }
            }
            return count;
        }

        public int JobDone(DateTime date)
        {
            return statusCalculator.GetDoneJobs(date);
        }

        public int JobComing(DateTime date)
        {
            return statusCalculator.GetComingJobs(date);
        }

        public int JobLate(DateTime date)
        {
            return statusCalculator.GetLateJobs(date);
        }

        public int JobDoing(DateTime date)
        {
            return statusCalculator.GetDoingJobs(date);
        }

        public string DailyJob(DateTime date)
        {
            return $"Tong: {JobByDay(date)} viec || " +
                   $"Done: {JobDone(date)} || " +
                   $"Doing: {JobDoing(date)} || " +
                   $"Missed: {JobLate(date)} || " +
                   $"Comming: {JobComing(date)} || ";
        }
    }
}