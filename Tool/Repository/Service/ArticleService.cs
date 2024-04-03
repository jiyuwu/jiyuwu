using IService;
using Model;
using Model.Common;
using Repository.DBHelper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace repository.Service
{
    public class ArticleService : IArticleService
    {
        SQLiteHelper helper = new SQLiteHelper();
        public void NotExitCreateTable()
        {

        }
        public string AddNew(Article Information)
        {
            Information.CreateTime = DateTime.Now;
            Information.Id = Guid.NewGuid().ToString();
            Information.EditTime = DateTime.Now;
            if (helper.Insert<Article>(Information) > 0)
            {
                return Information.Id;
            }
            return "";
        }

        public string ChangeState(ChangeState changeState)
        {
            Article Information = helper.GetById<Article>(changeState.Id);
            Information.EditTime = DateTime.Now;
            Information.Status = changeState.State;
            if (helper.Update(Information) > 0)
            {
                return Information.Id;
            }
            return "";
        }
        public Article GetInfoById(string Id)
        {
            return helper.GetById<Article>(Id);
        }

        public List<Article> GetList(ref PageModel pageModel)
        {
            return helper.GetAll<Article>(ref pageModel);
        }

        public string Update(Article Information)
        {
            Article old = helper.GetById<Article>(Information.Id);
            old.Content = Information.Content;
            old.EditTime = DateTime.Now;
            old.Status = Information.Status;
            if (helper.Update(old) > 0)
            {
                return old.Id;
            }
            return "";
        }
    }

    #region ef赠给朋友的代码

    //public string AddNew(Article Information)
    //{
    //    using (EFContext db = new EFContext())
    //    {
    //        Information.CreateTime = DateTime.Now;
    //        Information.Id= Guid.NewGuid().ToString();
    //        Information.EditTime = DateTime.Now;
    //        db.Articles.Add(Information);
    //        db.SaveChanges();
    //        return Information.Id;
    //    }
    //}

    //public string ChangeState(ChangeState changeState)
    //{
    //    using (EFContext db = new EFContext())
    //    {
    //        Article info = db.Articles.Where(e => e.Id == changeState.Id).FirstOrDefault();
    //        if (!string.IsNullOrWhiteSpace(info.Id))
    //        {
    //            info.EditTime = DateTime.Now;
    //            info.Status = changeState.State;
    //        }
    //        db.Entry(info).State = EntityState.Modified;
    //        db.SaveChanges();
    //        return info.Id;
    //    }
    //}

    //public int GetCount(PageModel pageModel)
    //{
    //    using (EFContext db = new EFContext())
    //    {
    //        var count = (from e in db.Articles
    //                     where (e.Title.Contains(pageModel.TitleName) || pageModel.TitleName == "")
    //                     select e).Where(e => e.Status != 0).Count();
    //        return count;
    //    }
    //}

    //public Article GetInfoById(string Id)
    //{
    //    using (EFContext db = new EFContext())
    //    {
    //        var user = (from e in db.Articles
    //                    where e.Id == Id
    //                    select e).FirstOrDefault();
    //        return user;
    //    }
    //}

    //public List<Article> GetList(PageModel pageModel)
    //{
    //    using (EFContext db = new EFContext())
    //    {
    //        var user = (from e in db.Articles
    //                    where (e.Title.Contains(pageModel.TitleName) || pageModel.TitleName == "")
    //                    select e).Where(e => e.Status != 0).OrderByDescending(e => e.CreateTime).Skip((pageModel.CurrentPage - 1) * pageModel.NumCount).Take(pageModel.NumCount).ToList();
    //        return user;
    //    }
    //}

    //public string Update(Article Information)
    //{
    //    using (EFContext db = new EFContext())
    //    {
    //        Article info = db.Articles.Where(e => e.Id == Information.Id).FirstOrDefault();
    //        if (!string.IsNullOrWhiteSpace(info.Id))
    //        {
    //            info.Content = Information.Content;
    //            info.EditTime = DateTime.Now;
    //            info.Status = Information.Status;
    //        }
    //        db.Entry(info).State = EntityState.Modified;
    //        db.SaveChanges();
    //        return info.Id;
    //    }
    //}
    #endregion
}
