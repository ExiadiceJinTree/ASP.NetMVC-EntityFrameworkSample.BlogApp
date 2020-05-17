using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyBlogWebApp.Models;

namespace MyBlogWebApp.Controllers
{
    /// <summary>
    /// Categories Controller。
    /// コントローラー生成テンプレート"Entity Frameworkを使用した、ビューがあるMVC5コントローラー"で自動生成。
    /// コントローラー自動生成時のオプション:
    ///   ・モデルクラス=Category  ・データコンテキストクラス=BlogContext  ・非同期コントローラーアクションの使用=true
    ///   ・ビュー:
    ///     ・ビューの生成=true  ・スクリプトライブラリの参照=true  ・レイアウトページの使用=true(~/Views/Shared/_Layout.cshtml)
    /// * カテゴリの登録、更新の方法は、記事の登録/更新時に一緒に行われる方法のみとするため、本コントローラーにはカテゴリの登録、更新アクション&ビューは用意しない。
    /// </summary>
    [CategoryFilter]  // Categoryに関するAction Filter属性の指定により、本コントローラの各アクションが実行される際に、指定のAction Filterの処理が実行される。
    public class CategoriesController : Controller
    {
        private BlogContext db = new BlogContext();

        // GET: Categories
        [AllowAnonymous]  // カテゴリ一覧には認証していない状態でもアクセスできるようにする。
        public async Task<ActionResult> Index()
        {
            return View(await db.Categories.ToListAsync());
        }

        // GET: Categories/Details/5
        [AllowAnonymous]  // カテゴリ一覧には認証していない状態でもアクセスできるようにする。
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        [Authorize(Roles = "Owners")]  // カテゴリ削除は管理者のみアクセスできるように設定。
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owners")]  // カテゴリ削除は管理者のみアクセスできるように設定。 * 念のためPOSTアクションもアクセス制限する。
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Category category = await db.Categories.FindAsync(id);

            // 削除対象カテゴリに属している記事が存在する場合は、カテゴリの削除は不可としエラーとする
            if (category.ArticleCount > 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

            db.Categories.Remove(category);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
