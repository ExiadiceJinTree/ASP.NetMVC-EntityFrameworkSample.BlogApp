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
    /// Blog記事のカテゴリモデル。
    /// </summary>
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [StringLength(200)]  // フィールドの最大文字長を設定。string型にUnique設定すると、Databaseマイグレーション時にエラーとなる(SQLServerのindex keyの最大長が900Byteのため)。最大長を設定することで解決。
        [DisplayName("カテゴリ名")]
        public string CategoryName { get; set; }

        /// <summary>
        /// カテゴリに属する記事のコレクション。
        /// CategoryモデルとArticleモデル間のナビゲーションプロパティ。
        /// </summary>
        public virtual ICollection<Article> Articles { get; set; }

        /// <summary>
        /// カテゴリに属する記事の件数。
        /// * Articlesコレクションプロパティから件数を取得できるが、毎回コレクション要素数を取得するのは効率が悪いため、記事件数を予め保持するためのプロパティ。
        /// </summary>
        [DisplayName("記事件数")]
        public int ArticleCount { get; set; }
    }
}