using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThiCuoiKy
{
    // Định nghĩa NotifyForm kế thừa từ Form để tạo một cửa sổ thông báo.
    public partial class NotifyForm : Form
    {
        public NotifyForm(string message)
        {
            // Khởi tạo các thành phần giao diện của form, bao gồm các control như lbNotify (label hiển thị thông báo) và btnOK (nút đóng).
            InitializeComponent();

            // Gán chuỗi thông báo truyền vào (message) cho thuộc tính Text của lbNotify, để nội dung này xuất hiện trên form khi nó được hiển thị.
            lbNotify.Text = message;

            // Sự kiện Click của btnOK để khi nút này được nhấn, hàm btnOK_Click sẽ được gọi và thực thi hành động đóng form.
            btnOK.Click += new EventHandler(btnOK_Click);
        }

        // Đóng thông báo khi nhấn nút OK
        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
    // Tóm gọn: lớp NotifyForm hiển thị thông báo qua lbNotify, đăng ký sự kiện đóng khi nút OK được nhấn.
    
    private void lbNotify_Click(object sender, EventArgs e)
    {

    }
}
