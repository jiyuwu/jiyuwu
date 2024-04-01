using IService;
using Model;
using Model.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace repository.Service
{
    public class ArticleService: IArticleService
    {
        public string AddNew(Article Information)
        {
            using (EFContext db = new EFContext())
            {
                Information.CreateTime = DateTime.Now;
                Information.Id= Guid.NewGuid().ToString();
                Information.EditTime = DateTime.Now;
                db.Articles.Add(Information);
                db.SaveChanges();
                return Information.Id;
            }
        }

        public string ChangeState(ChangeState changeState)
        {
            using (EFContext db = new EFContext())
            {
                Article info = db.Articles.Where(e => e.Id == changeState.Id).FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(info.Id))
                {
                    info.EditTime = DateTime.Now;
                    info.Status = changeState.State;
                }
                db.Entry(info).State = EntityState.Modified;
                db.SaveChanges();
                return info.Id;
            }
        }

        public int GetCount(PageModel pageModel)
        {
            using (EFContext db = new EFContext())
            {
                var count = (from e in db.Articles
                             where (e.Title.Contains(pageModel.TitleName) || pageModel.TitleName == "")
                             select e).Where(e => e.Status != 0).Count();
                return count;
            }
        }

        public Article GetInfoById(string Id)
        {
            using (EFContext db = new EFContext())
            {
                var user = (from e in db.Articles
                            where e.Id == Id
                            select e).FirstOrDefault();
                return user;
            }
        }

        public List<Article> GetList(PageModel pageModel)
        {
            using (EFContext db = new EFContext())
            {
                var user = (from e in db.Articles
                            where (e.Title.Contains(pageModel.TitleName) || pageModel.TitleName == "")
                            select e).Where(e => e.Status != 0).OrderByDescending(e => e.CreateTime).Skip((pageModel.CurrentPage - 1) * pageModel.NumCount).Take(pageModel.NumCount).ToList();
                return user;
            }
        }

        public string Update(Article Information)
        {
            using (EFContext db = new EFContext())
            {
                Article info = db.Articles.Where(e => e.Id == Information.Id).FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(info.Id))
                {
                    info.Content = Information.Content;
                    info.EditTime = DateTime.Now;
                    info.Status = Information.Status;
                }
                db.Entry(info).State = EntityState.Modified;
                db.SaveChanges();
                return info.Id;
            }
        }
    }
}
