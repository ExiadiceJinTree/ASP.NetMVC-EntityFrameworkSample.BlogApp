using MyBlogWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBlogWebApp.Controllers
{
    /// <summary>
    /// Categoryに関するAction Filter属性クラス。
    /// * Custom Action Filter:
    /// 　・複数のコントローラーやアクションで同じ処理を効率よく実装する仕組み。
    /// 　・クラス名は、"FilterAttribute"で終わる必要がある。
    /// 　・FilterAttributeクラスを継承し、IActionFilterインターフェースを実装する必要がある。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]  // AttributeUsage属性: Filterをどこ(クラスやメソッド等)に適用するかを決定する。ここではコントローラクラスに適用するため、適用対象にClassを指定。
    public class CategoryFilterAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            using (var db = new BlogContext())
            {
                // カテゴリの一覧を取得 - ソート順: 属する記事の件数の降順
                var categories = db.Categories.OrderByDescending(c => c.ArticleCount).ToList();

                // ViewBagにカテゴリ一覧をセット
                filterContext.Controller.ViewBag.Categories = categories;  // ViewBagには、本メソッド引数のfilterContextから利用
            }
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
        }
    }
}