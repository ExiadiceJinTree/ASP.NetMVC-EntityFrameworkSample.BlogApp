/// <summary>
/// EntityFrameworkの自動Migration機能を有効にするための設定。
/// NuGetパッケージマネージャーコンソールで、以下のコマンドを実行することで、Migrationsフォルダと本クラスファイルを自動生成。
///   > Enable-Migrations -EnableAutomaticMigrations
/// * Migration機能: Modelの変更に合わせてDBにその内容が反映(作成・変更)される仕組み。
/// </summary>
namespace MyBlogWebApp.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MyBlogWebApp.Models.BlogContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;  // データ損失となるモデルの変更(列の削除等)についても自動マイグレーション(DBへの自動反映)を許可する。 * 自動生成後の手動追加設定。
        }

        protected override void Seed(MyBlogWebApp.Models.BlogContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
