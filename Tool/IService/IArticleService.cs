using Model;
using Model.Common;
using System.Collections.Generic;

namespace IService
{
    public interface IArticleService: IServiceSupport
    {
        int AddNew(Article Information);
        List<Article> GetList(PageModel pageModel);
        int GetCount(PageModel pageModel);
        Article GetInfoById(int Id);
        int Update(Article Information);
        int ChangeState(ChangeState changeState);
    }
}
