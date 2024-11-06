using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThiCuoiKy
{
    // NotifyManager là lớp cụ thể kế thừa từ NotificationBase.
    // Nó implement các phương thức trừu tượng ShouldNotify và ShowNotification
    public class NotifyManager : NotificationBase
    {
        // Thuộc tính: trạng thái bật/tắt thông báo
        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; }
        }

        // Kiểm tra xem có công việc nào cần thông báo vào ngày hiện tại không
        // Override phương thức trừu tượng
        public override bool ShouldNotify(DateTime currentDate, List<PlanItem> jobs)
        {
            // Nếu thông báo bị tắt, không cần kiểm tra gì cả
            if (!isEnabled) return false;

            // Lọc ra các công việc của hôm nay
            List<PlanItem> todayJobs = new List<PlanItem>();
            foreach (PlanItem job in jobs)
            {
                if (job.Date.Date == currentDate.Date &&
                    (job.Status == "DOING" || job.Status == "COMING"))
                {
                    todayJobs.Add(job);
                }
            }

            // Nếu có công việc cần thông báo, trả về true
            return todayJobs.Count > 0;
        }

        //Hiển thị một giao diện thông báo dưới dạng NotifyForm
        public override void ShowNotification(string message)
        {
            // Nếu thông báo bị tắt, không cần hiển thị    
            if (!isEnabled) return;

            // Tạo và hiển thị Form thông báo
            using (NotifyForm notifyForm = new NotifyForm(message))
            {
                notifyForm.ShowDialog();
            }
        }
    }
}
