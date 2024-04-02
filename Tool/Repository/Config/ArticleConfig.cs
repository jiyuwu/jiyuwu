using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace repository.Config
{
    public class ArticleConfig : EntityTypeConfiguration<Article>
    {
        public ArticleConfig()
        {
            // 设置主键
            this.HasKey(a => a.Id);
            // 配置属性映射
            this.Property(a => a.Title).IsRequired().HasMaxLength(100);
            this.Property(a => a.Content).IsRequired();
            // 配置数据库表名
            this.ToTable("Article");
        }
    }
}
