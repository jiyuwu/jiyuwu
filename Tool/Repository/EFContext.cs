using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Model;
using repository.Config;
using System.IO;

namespace repository
{
    public class EFContext : DbContext
    {
        public DbSet<Article> Articles { get; set; }

        // 构造函数：指定连接字符串
        public EFContext() : base("name=MyDbContext")
        {
            EnsureDatabaseCreated();
        }
        private void EnsureDatabaseCreated()
        {
            string dbFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mydatabase.db");

            if (!File.Exists(dbFilePath))
            {
                Database.SetInitializer<EFContext>(null); // 禁用数据库初始化

                // 创建一个空的数据库以及表结构
                Database.Create();
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // 获取当前程序集
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
                // 选择所有继承自 EntityTypeConfiguration<> 的类型
                .Where(type => !string.IsNullOrEmpty(type.Namespace))
                .Where(type => type.BaseType != null && type.BaseType.IsGenericType &&
                    type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));

            // 使用反射将这些配置类添加到 modelBuilder 中
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }
        }
    }
}
