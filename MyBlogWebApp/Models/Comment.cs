using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyBlogWebApp.Models
{
    /// <summary>
    /// Blog記事のコメントモデル。
    /// </summary>
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("コメント")]
        public string Body { get; set; }

        //[Required]  // 必ず設定すべき値だが、必須入力項目ではなくControllerで自動設定するため、Required属性指定はしない。
        [DisplayName("投稿日時")]
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        /// コメントが付与された記事。
        /// * CommentモデルとArticleモデル間のナビゲーションプロパティ。
        /// </summary>
        public virtual Article Article { get; set; }


        /// <summary>
        /// コメントが付与される記事のId。
        /// コメント投稿時にどの記事に対するコメントなのかを管理するために用いる。
        /// </summary>
        [NotMapped]
        public int ArticleId { get; set; }
    }
}