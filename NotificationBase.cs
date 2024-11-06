using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.Notify
{
    public abstract class NotificationBase
    {
        protected bool isEnabled;

        protected NotificationBase()
        {
            this.isEnabled = false;
        }

        public abstract void ShowNotification(string message);
        public abstract bool ShouldNotify(DateTime currentDate, List<PlanItem> jobs);
    }
}
