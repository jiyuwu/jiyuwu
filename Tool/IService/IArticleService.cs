using Model;
using Model.Common;
using System.Collections.Generic;

namespace IService
{
    public interface IArticleService: IServiceSupport
    {
        string AddNew(Article Information);
        List<Article> GetList(PageModel pageModel);
        int GetCount(PageModel pageModel);
        Article GetInfoById(string Id);
        string Update(Article Information);
        string ChangeState(ChangeState changeState);
    }
}
