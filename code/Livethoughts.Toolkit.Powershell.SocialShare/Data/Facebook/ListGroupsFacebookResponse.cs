using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Livethoughts.Toolkit.Powershell.SocialShare.Data.Facebook
{
    class ListGroupsFacebookResponse
    {
        public List<ListGroupsFacebookResponseGroup> data { get; set; }
        public FacebookPagingResponse paging { get; set; }
        public string id { get; set; }
    }
}
