using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace NewsWeb.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        /// <summary> 

        /// <summary> 
        /// 用户名 
        /// </summary> 
        [Display(Name = "用户名", Description = "4-20个字符。")]
        [Required(ErrorMessage = "×")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "×")] public string UserName { get; set; }
        /// <summary> 
        /// 密码 
        /// </summary> 
        [Display(Name = "密码")]
        [Required(ErrorMessage = "×")]
        [StringLength(512)]
        public string Password { get; set; }
        /// <summary> 
        /// 性别【0-男；1-女；2-保密】 
        /// </summary> 
        [Display(Name = "性别", Description = "0-男；1-女；2-保密")]
        [Required(ErrorMessage = "×")] [Range(0, 2, ErrorMessage = "×")] public byte Gender { get; set; }
        /// <summary> 
        /// Email 
        /// </summary> 
        [Display(Name = "Email", Description = "请输入您常用的Email。")]
        [Required(ErrorMessage = "×")]

        public string Email { get; set; }
        /// <summary> 
        /// 密保问题 
        /// </summary> 
        [Display(Name = "密保问题", Description = "请正确填写，在您忘记密码时用户找回密码。4-20个字符。")]
        [Required(ErrorMessage = "×")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "×")] public string SecurityQuestion { get; set; }
        /// <summary> 
        /// 密保答案 
        /// </summary> 
        [Display(Name = "密保答案", Description = "请认真填写，忘记密码后回答正确才能找回密码。2-20个字符。")]
        [Required(ErrorMessage = "×")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "×")] public string SecurityAnswer { get; set; }
        /// <summary> 
        /// QQ号码 
        /// </summary> 
        [Display(Name = "QQ号码", Description = "6-12位数")]
        //  [RegularExpression("^[1-9][0-9]{4-13}$", ErrorMessage = "×")] 
        [StringLength(12, MinimumLength = 6, ErrorMessage = "×")] public string QQ { get; set; }
        /// <summary> 
        /// 电话号码 
        /// </summary> 
        [Display(Name = "电话号码", Description = "常用的联系电话（手机或固话），固话格式为：区号 - 号码。")]
        //  [RegularExpression("^[0-9-]{11-13}$", ErrorMessage = "×")]
        public string Tel { get; set; }
        /// <summary> 
        /// 联系地址 
        /// </summary> 
        [Display(Name = "联系地址", Description = "常用地址，最多 80个字符。")]
        [StringLength(80, ErrorMessage = "×")] public string Address { get; set; }
        /// <summary> 
        /// 邮编 
        /// </summary> 
        [Display(Name = "邮编", Description = "6位字符。")]
        [RegularExpression("^[0-9]{6}$", ErrorMessage = "×")]
        public string PostCode { get; set; }
        /// <summary> 
        /// 注册时间         /// </summary> 
        public DateTime? RegTime { get; set; }
        /// <summary> 
        /// 上次登录时间 
        /// </summary> 
        public DateTime? LastLoginTime { get; set; }

    }

    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
    public class Article
    {
        [Key]
        public int ArticleId { get; set; }
        /// <summary> 
        ///  栏目 id 
        /// </summary> 
        [Display(Name = "栏目ID")]
        [Required(ErrorMessage = "×")]
        public int CategoryId { get; set; }
        /// <summary> 
        ///  来源 
        /// </summary> 
        [Display(Name = "来源")]
        [StringLength(255, ErrorMessage = "×")]
        public string Source { get; set; }
        /// <summary> 
        ///  作者 
        /// </summary> 
        [Display(Name = "作者")]
        [StringLength(50, ErrorMessage = "×")]
        public string Author { get; set; }
        /// <summary> 
        ///  摘要 
        /// </summary>  [NotMapped] 
        [Display(Name = "摘要")]
        public string Intro { get; set; }
        /// <summary> 
        ///  内容 
        /// </summary> 
        [Display(Name = "内容")]
        [Required(ErrorMessage = "×")]
        [DataType(DataType.Html)]
        public string Content { get; set; }
    }
    public class NewsDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categorys { get; set; }
        public DbSet<Article> Articles { get; set; }
    }
}