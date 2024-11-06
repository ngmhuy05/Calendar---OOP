using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThiCuoiKy
    // Đây là lớp trừu tượng chứa các hành vi cơ bản của hệ thống thông báo
    // Lớp này được định nghĩa là abstract, tức là không thể khởi tạo trực tiếp. Nó sẽ được kế thừa và implement bởi các lớp cụ thể.
    public abstract class NotificationBase
    {
        // Trạng thái bật/tắt của thông báo
        protected bool isEnabled;

        // Constructor
        protected NotificationBase()
        {
            // Mặc định là tắt
            this.isEnabled = false;
        }

        // Phương thức trừu tượng: hiển thị thông báo
        public abstract void ShowNotification(string message);

        // Phương thức trừu tượng: kiểm tra xem có cần thông báo không
        public abstract bool ShouldNotify(DateTime currentDate, List<PlanItem> jobs);
    }
}
