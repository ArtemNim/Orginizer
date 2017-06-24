using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Orginizer
{
    public class Note_Parent
    {
        public DateTime date { get; set; }
      
        public string text { get; set; }

        public string state { get; set; }

        public bool notify { get; set; }
    }
}
