using MyBlogWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MyBlogWebApp.Controllers
{
    /// <summary>
    /// Loginのコントローラー。
    /// コントローラー生成テンプレート"MVC5コントローラー - 空"で自動生成。
    /// </summary>
    [AllowAnonymous]  // 本コントローラーには認証していない状態でもアクセスできるようにする。
    public class LoginController : Controller
    {
        /// <summary>
        /// ユーザー認証をするためのメンバーシッププロバイダー。
        /// </summary>
        private readonly CustomMembershipProvider _membershipProvider = new CustomMembershipProvider();


        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        // 自動生成されたコントローラーにはGetアクセスのIndexアクションメソッドしかないのでPostアクセスのIndexアクションメソッドを作成。
        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "LoginId,Password")] LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                if (_membershipProvider.ValidateUser(loginViewModel.LoginId, loginViewModel.Password))  // ユーザー認証がOKの場合
                {
                    // 認証Cookieを設定することによって認証状態を保持するようにする。
                    // * FormsAuthenticationクラス: ASP.NETに用意されているWebアプリケーションのフォーム認証サービスを管理するためのクラス。これを用いて認証状態を保持する。
                    // * SetAuthCookieメソッド: ユーザー名をCookieに保持する。
                    FormsAuthentication.SetAuthCookie(loginViewModel.LoginId, false);

                    // ブログ記事一覧ビューへ遷移する。
                    return RedirectToAction("Index", "Articles");
                }
            }

            ViewBag.Message = "Failed to login.";
            return View(loginViewModel);
        }

        //GET: Login/SignOut
        public ActionResult SignOut()
        {
            // 認証時にセットした認証Cookieを削除。
            FormsAuthentication.SignOut();

            // ログイン画面に戻す
            return RedirectToAction(nameof(Index));
        }
    }
}