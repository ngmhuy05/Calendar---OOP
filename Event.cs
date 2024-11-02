using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ThiCuoiKy
{
     class JobEventChange
    {
        private event EventHandler _edited;
        private event EventHandler _deleted;
        public event EventHandler Edited
        {
            add { _edited += value; }
            remove { _edited -= value; }
        }

        public event EventHandler Deleted
        {
            add { _deleted += value; }
            remove { _deleted -= value; }
        }

        public void OnEdited(object sender, EventArgs e)
        {
            _edited?.Invoke(sender, e);
        }

        public void OnDeleted(object sender, EventArgs e)
        {
            _deleted?.Invoke(sender, e);
        }
    }
}
