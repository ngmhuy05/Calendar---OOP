using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.Notify
{
    public class NotifyManager : NotificationBase
    {
        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; }
        }

        public override bool ShouldNotify(DateTime currentDate, List<PlanItem> jobs)
        {
            if (!isEnabled) return false;

            List<PlanItem> todayJobs = new List<PlanItem>();
            foreach (PlanItem job in jobs)
            {
                if (job.Date.Date == currentDate.Date &&
                    (job.Status == "DOING" || job.Status == "COMING"))
                {
                    todayJobs.Add(job);
                }
            }
            return todayJobs.Count > 0;
        }

        public override void ShowNotification(string message)
        {
            if (!isEnabled) return;

            using (NotifyForm notifyForm = new NotifyForm(message))
            {
                notifyForm.ShowDialog();
            }
        }
    }
}
