using IService;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class ArticleBll
    {
        private readonly IArticleService _service;
        public ArticleBll(IArticleService serviceProvider) {
            _service = serviceProvider;
        }
        public string addArticle(Article article)
        {
            string id= _service.AddNew(article);
            return id;
        }

    }
}
