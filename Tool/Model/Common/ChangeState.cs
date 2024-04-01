using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Common
{
    public class ChangeState
    {
        public string Method { get; set; }//操作
        public string Id { get; set; }//Id
        public int State { get; set; } //状态 1.启用 2.禁用
    }
}
