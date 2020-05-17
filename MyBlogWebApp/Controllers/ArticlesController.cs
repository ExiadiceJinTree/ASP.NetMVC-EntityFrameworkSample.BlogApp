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
using System.Runtime.CompilerServices;

namespace MyBlogWebApp.Controllers
{
    /// <summary>
    /// Articles Controller。
    /// コントローラー生成テンプレート"Entity Frameworkを使用した、ビューがあるMVC5コントローラー"で自動生成。
    /// コントローラー自動生成時のオプション:
    ///   ・モデルクラス=Article  ・データコンテキストクラス=BlogContext  ・非同期コントローラーアクションの使用=true
    ///   ・ビュー:
    ///     ・ビューの生成=true  ・スクリプトライブラリの参照=true  ・レイアウトページの使用=true(~/Views/Shared/_Layout.cshtml)
    /// </summary>
    [CategoryFilter]  // Categoryに関するAction Filter属性の指定により、本コントローラの各アクションが実行される際に、指定のAction Filterの処理が実行される。
    public class ArticlesController : Controller
    {
        private BlogContext db = new BlogContext();


        #region Actions for Article

        // GET: Articles
        [AllowAnonymous]  // 記事一覧には認証していない状態でもアクセスできるようにする。
        public async Task<ActionResult> Index()
        {
            return View(await db.Articles.ToListAsync());
        }

        // GET: Articles/Details/5
        [AllowAnonymous]  // 記事内容には認証していない状態でもアクセスできるようにする。
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = await db.Articles.FindAsync(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // GET: Articles/Create
        [Authorize(Roles = "Owners")]  // 記事作成は管理者のみアクセスできるように設定。
        public ActionResult Create()
        {
            return View();
        }

        // POST: Articles/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owners")]  // 記事作成は管理者のみアクセスできるように設定。 * 念のためPOSTアクションもアクセス制限する。
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,Body,CategoryName")] Article article)  // Bindをデフォルトから変更
        //public async Task<ActionResult> Create([Bind(Include = "Id,Title,Body,CreatedDateTime,ModifiedDateTime")] Article article)
        {
            if (ModelState.IsValid)
            {
                // 記事の投稿日時、更新日時に現在日時をセット
                article.CreatedDateTime = DateTime.UtcNow;
                article.ModifiedDateTime = DateTime.UtcNow;

                // 入力されたカテゴリ名(CategoryNameプロパティ値)を元に、記事に指定のカテゴリを設定する。
                var specifiedCategory = db.Categories.Where(c => c.CategoryName.Equals(article.CategoryName)).FirstOrDefault();
                if (specifiedCategory == null)
                {
                    // 指定のカテゴリが存在しない場合は、そのカテゴリを新規作成
                    specifiedCategory = new Category() {
                        CategoryName = article.CategoryName,
                        //Articles = new List<Article>() { article },  // * 明示的なコード不要
                        ArticleCount = 1,  // 記事件数は初記事なので1件
                    };
                    db.Categories.Add(specifiedCategory);  // DBに指定のカテゴリを追加
                }
                else
                {
                    // 指定のカテゴリが既存の場合は、そのカテゴリを更新
                    //specifiedCategory.Articles.Add(article);  // * 明示的なコード不要
                    specifiedCategory.ArticleCount++;  // 記事件数をインクリメント
                    db.Entry(specifiedCategory).State = EntityState.Modified;  // DBの指定のカテゴリを更新
                }
                article.Category = specifiedCategory;  // 記事にカテゴリをセット

                db.Articles.Add(article);  // DBに指定の記事を追加

                await db.SaveChangesAsync();
                
                return RedirectToAction("Index");
            }

            return View(article);
        }

        // GET: Articles/Edit/5
        [Authorize(Roles = "Owners")]  // 記事編集は管理者のみアクセスできるように設定。
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = await db.Articles.FindAsync(id);
            if (article == null)
            {
                return HttpNotFound();
            }

            article.CategoryName = article.Category.CategoryName;  // Editビューにカテゴリ名を表示できるように、ArticleモデルのCategoryNameプロパティに値を設定する。

            return View(article);
        }

        // POST: Articles/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owners")]  // 記事編集は管理者のみアクセスできるように設定。 * 念のためPOSTアクションもアクセス制限する。
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Body,CategoryName")] Article article)  // Bindをデフォルトから変更
        //public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Body,CreatedDateTime,ModifiedDateTime")] Article article)
        {
            if (ModelState.IsValid)
            {
                //***** DBから取得したArticleを元に変更を行う。 *****
                Article dbArticle = await db.Articles.FindAsync(article.Id);
                if (dbArticle == null)
                {
                    return HttpNotFound();
                }

                dbArticle.Title = article.Title;
                dbArticle.Body = article.Body;
                dbArticle.ModifiedDateTime = DateTime.UtcNow;  // 記事の更新日時に現在日時をセット
                dbArticle.CategoryName = article.CategoryName;

                // 記事のカテゴリを更新、及び、カテゴリと記事の関係を更新
                Category preCategory = dbArticle.Category;  // 記事更新前に記事に設定されていた元カテゴリ
                if (!preCategory.CategoryName.Equals(article.CategoryName))  // 記事の元カテゴリと現在指定カテゴリが異なる場合
                {
                    // 元カテゴリから該当記事を削除&記事件数をデクリメント
                    preCategory.Articles.Remove(dbArticle);
                    preCategory.ArticleCount--;
                    db.Entry(preCategory).State = EntityState.Modified;

                    // 入力されたカテゴリ名(CategoryNameプロパティ値)を元に、記事に指定のカテゴリを設定する。
                    var specifiedCategory = db.Categories.Where(c => c.CategoryName.Equals(article.CategoryName)).FirstOrDefault();
                    if (specifiedCategory == null)
                    {
                        // 指定のカテゴリが存在しない場合は、そのカテゴリを新規作成
                        specifiedCategory = new Category()
                        {
                            CategoryName = article.CategoryName,
                            //Articles = new List<Article>() { article },  // * 明示的なコード不要
                            ArticleCount = 1,  // 記事件数は初記事なので1件
                        };
                        db.Categories.Add(specifiedCategory);  // DBに指定のカテゴリを追加
                    }
                    else
                    {
                        // 指定のカテゴリが既存の場合は、そのカテゴリを更新
                        //specifiedCategory.Articles.Add(article);  // * 明示的なコード不要
                        specifiedCategory.ArticleCount++;  // 記事件数をインクリメント
                        db.Entry(specifiedCategory).State = EntityState.Modified;  // DBの指定のカテゴリを更新
                    }
                    dbArticle.Category = specifiedCategory;  // 記事にカテゴリをセット
                }

                //db.Entry(article).State = EntityState.Modified;
                db.Entry(dbArticle).State = EntityState.Modified;

                await db.SaveChangesAsync();
                
                return RedirectToAction("Index");
            }
            return View(article);
        }

        // GET: Articles/Delete/5
        [Authorize(Roles = "Owners")]  // 記事削除は管理者のみアクセスできるように設定。
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = await db.Articles.FindAsync(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owners")]  // 記事削除は管理者のみアクセスできるように設定。 * 念のためPOSTアクションもアクセス制限する。
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Article article = await db.Articles.FindAsync(id);

            // 記事が属しているカテゴリに対して、該当記事を削除&記事件数をデクリメント
            Category category = article.Category;
            if (category != null)
            {
                //category.Articles.Remove(article);  // 明示的なコード不要
                category.ArticleCount--;
                db.Entry(category).State = EntityState.Modified;
            }

            // 記事に紐づく全コメントを削除
            article.Comments.Clear();

            db.Articles.Remove(article);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        #endregion  // Actions for Article


        #region Actions for Comment

        // POST: Articles/CreateComment
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]  // コメント作成は認証していない状態でもアクセスできるようにする。
        public async Task<ActionResult> CreateComment([Bind(Include = "Body,ArticleId")] Comment comment)
        {
            Article article = await db.Articles.FindAsync(comment.ArticleId);
            if (article == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

            comment.CreatedDateTime = DateTime.UtcNow;  // コメントの更新日時に現在日時をセット
            comment.Article = article;

            db.Comments.Add(comment);
            await db.SaveChangesAsync();

            // コメント投稿後は元記事内容ビューに戻る
            return RedirectToAction(nameof(Details), new { id = comment.ArticleId });
        }

        // GET: Articles/DeleteComment/5
        [Authorize(Roles = "Owners")]  // コメント削除は管理者のみアクセスできるように設定。
        public async Task<ActionResult> DeleteComment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = await db.Comments.FindAsync(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Articles/DeleteComment/5
        [HttpPost, ActionName("DeleteComment")]  // ActionName属性を指定することでUrlのアクション名とアクションメソッド名を別名にできる。
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owners")]  // 記事削除は管理者のみアクセスできるように設定。 * 念のためPOSTアクションもアクセス制限する。
        public async Task<ActionResult> DeleteCommentConfirmed(int id)  // DeleteアクションのGetとPostのメソッド名を同じにすると引数が重複してしまいエラーになるため、メソッド名を変える必要がある。
        {
            Comment comment = await db.Comments.FindAsync(id);
            
            /*
            Article article = comment.Article;
            if (article != null)
            {
                article.Comments.Remove(comment);
                db.Entry(article).State = EntityState.Modified;
            }
            */

            db.Comments.Remove(comment);

            await db.SaveChangesAsync();

            // コメント削除後は元記事内容ビューに戻る
            return RedirectToAction(nameof(Details), new { id = comment.ArticleId });
        }

        #endregion  // Actions for Comment


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
