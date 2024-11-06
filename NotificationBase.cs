using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThiCuoiKy
    //Lớp này được định nghĩa là abstract, tức là không thể khởi tạo trực tiếp. Nó sẽ được kế thừa và implement bởi các lớp cụ thể.
    public abstract class NotificationBase
    {
        //Trạng thái bật/tắt của thông báo
        protected bool isEnabled;

        protected NotificationBase()
        {
            this.isEnabled = false;
        }

        //Phương thức trừu tượng dùng để hiển thị thông báo
        public abstract void ShowNotification(string message);

        //Phương thức trừu tượng dùng để kiểm tra xem có cần hiển thị thông báo hay không
        public abstract bool ShouldNotify(DateTime currentDate, List<PlanItem> jobs);
    }
}
