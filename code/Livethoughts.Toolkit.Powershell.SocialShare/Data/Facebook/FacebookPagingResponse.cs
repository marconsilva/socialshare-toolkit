using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Livethoughts.Toolkit.Powershell.SocialShare.Data.Facebook
{
    class FacebookPagingResponse
    {
        public FacebookPagingResponseCursors cursors { get; set; }
        public string next { get; set; }
    }
}
