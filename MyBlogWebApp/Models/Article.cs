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
    /// Blog記事モデル。
    /// </summary>
    public class Article
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("タイトル")]
        public string Title { get; set; }
        
        [Required]
        [DisplayName("本文")]
        public string Body { get; set; }

        //[Required]  // 必ず設定すべき値だが、必須入力項目ではなくControllerで自動設定するため、Required属性指定はしない。
        [DisplayName("投稿日時")]
        public DateTime CreatedDateTime { get; set; }

        //[Required]  // 必ず設定すべき値だが、必須入力項目ではなくControllerで自動設定するため、Required属性指定はしない。
        [DisplayName("更新日時")]
        public DateTime ModifiedDateTime { get; set; }

        /// <summary>
        /// 記事が属するカテゴリ。
        /// * ArticleモデルとCategoryモデル間のナビゲーションプロパティ。
        /// </summary>
        public virtual Category Category { get; set; }

        /// <summary>
        /// 記事に付与されたコメントのコレクション。
        /// * ArticleモデルとCommentモデル間のナビゲーションプロパティ。
        /// </summary>
        public virtual ICollection<Comment> Comments { get; set; }


        /// <summary>
        /// 記事が属するカテゴリのカテゴリ名。
        /// 記事編集時にカテゴリを指定するために用い、入力値を保持する。
        /// </summary>
        [NotMapped]
        [DisplayName("カテゴリ")]
        public string CategoryName { get; set; }
    }
}