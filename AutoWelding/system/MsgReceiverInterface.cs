using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AutoWelding.system
{
    public interface MsgReceiverInterface
    {
        void HandleMsg(ref Message m);
    }
}
