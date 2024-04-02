using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Common
{
    public class PageModel
    {
        public string TitleName { get; set; } = "";//筛选标题
        public int CurrentPage { get; set; } = 1;//当前页
        public int NumCount { get; set; } = 10; //每页数量
        public string Sord { get; set; } = "";//排序字段
        public string Sort { get; set; } = "";//排序方式
        public int Count { get; set; } = 0;//数量
        public long Id { get; set; } = 0;//默认id
        public string Token { get; set; } = "";//认证授权
        public string MyKey { get; set; } = "";//关键字搜索
    }
}
