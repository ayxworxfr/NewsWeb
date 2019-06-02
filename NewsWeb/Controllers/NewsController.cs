using NewsWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsWeb.Controllers
{
    public class NewsController : Controller
    {
        // GET: /News/         
        NewsDBContext db = new NewsDBContext();
        static int page_now = 1;                // 当前页
        static int page_total = 0;              // 总页数
        static int page_pre = 0;                // 上一页
        static int page_next = 2;               // 下一页
        static int page_show = 2;               // 一页展示数量
        
        public ActionResult FilterCa(string categorie)
        {
            var cat = db.Categorys.SingleOrDefault(p => p.CategoryName == categorie);

            var articles = db.Articles.Where(p => p.ArticleId == cat.CategoryId);
            ViewBag.categorys = db.Categorys.ToList();
            //var articles = db.Articles.Where(p => p.ArticleId == cat.CategoryId).OrderBy(o => o.ArticleId).Skip(((int)page_num - 1) * page_show).Take(page_show);
            return View("Show", articles);
        }
        public ActionResult Show(int? page_num)
        {
            if (page_num == null)
                page_num = 1;

            page_pre = (int)page_num - 1;
            page_next = (int)page_num + 1;
            page_total = db.Articles.Count() / page_show + 1;
            if (page_pre < 1) page_pre = 1;
            if (page_next > page_total) page_next = page_total;
            ViewBag.categorys = db.Categorys.ToList();
            ViewBag.page_pre = "/News/Show?page_num=" + page_pre;
            ViewBag.page_next = "/News/Show?page_num=" + page_next;
            ViewBag.page_tail = "/News/Show?page_num=" + page_total;

            var articles = db.Articles.OrderBy(o => o.ArticleId).Skip(((int)page_num - 1) * page_show).Take(page_show);
            return View("Show", articles);


            /*
            var GenreQry = from d in db.Movie
                           orderby d.Genre
                           select d.Genre;
            GenreLst.AddRange(GenreQry.Distinct());
            ViewBag.movieGenre = new SelectList(GenreLst);
            var movies = from m in db.Movie
                         select m;
            if (!String.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s =>; s.Title.Contains(searchString));
            }
            if (!string.IsNullOrEmpty(movieGenre))
            {
                movies = movies.Where(x =>; x.Genre == movieGenre);
            }
            */
        }
        [HttpPost]
        public ActionResult FindNews(string intro)
        {
            if (String.IsNullOrEmpty(intro))
            {
                return View();
            }

            var article = db.Articles.SingleOrDefault(p => p.Intro == intro);
            if (article == null)
            {
                return Json("error");
            }
            else
            {
                return Json(article);
            }
        }

        public ActionResult Index(int? pageno)
        {
            if (pageno == null)
                pageno = 1;

            int prepage = (int)pageno - 1;
            int nextpage = (int)pageno + 1;
            int pagecount = db.Articles.Count() / 6 + 1;
            if (prepage < 1) prepage = 1;
            if (nextpage > pagecount) nextpage = pagecount;

            ViewBag.page = "<a href='/news/index?pageno=1'>首页</a>&nbsp;&nbsp;&nbsp;&nbsp;";
            ViewBag.page += "<a href='/news/index?pageno=" + prepage + "'>上一页</a>&nbsp;&nbsp;&nbsp;&nbsp;";
            ViewBag.page += pageno + "</a>&nbsp;&nbsp;&nbsp;&nbsp;";

            ViewBag.page += "<a href='/news/index?pageno=" + nextpage + "'>下一页</a>&nbsp;&nbsp;&nbsp;&nbsp;";
            ViewBag.page += "<a href='/news/index?pageno=" + pagecount + "'>末页</a>&nbsp;&nbsp;&nbsp;&nbsp;";

            var q = db.Articles.OrderBy(o => o.ArticleId).Skip(((int)pageno - 1) * 6).Take(6);
            return View("index", q);

        }
        //public ActionResult Index1(int? pageno, int? kind)
        //{
        //    if (pageno == null)
        //        pageno = 1;

        //    int prepage = (int)pageno - 1;
        //    int nextpage = (int)pageno + 1;
        //    if (kind == null)
        //    {
        //        kind = 0;
        //    }
        //    z = (int)kind;
        //    var news = from m in db.Articles select m;
        //    if (kind != 0)
        //    {
        //        news = news.Where(s => s.CategoryId.Equals(z));
        //    }
        //    int pagecount = news.Count() / 6 + 1;
        //    if (prepage < 1) prepage = 1;
        //    if (nextpage >= pagecount) nextpage = pagecount;

        //    ViewBag.page = "<a href='/news/index1?pageno=1&&kind=" + kind + "'>首页</a>&nbsp;&nbsp;&nbsp;&nbsp;";
        //    ViewBag.page += "<a href='/news/index1?pageno=" + prepage + "&&kind=" + kind + "'>上一页 </a>&nbsp;&nbsp;&nbsp;&nbsp;";
        //    ViewBag.page += pageno + "</a>&nbsp;&nbsp;&nbsp;&nbsp;";

        //    ViewBag.page += "<a href='/news/index1?pageno=" + nextpage + "&&kind=" + kind + "'>下一页</a>&nbsp;&nbsp;&nbsp;&nbsp;";
        //    ViewBag.page += "<a href='/news/index1?pageno=" + pagecount + "&&kind=" + kind + "'>末页 </a>&nbsp;&nbsp;&nbsp;&nbsp;";
        //    now = (int)pageno;

        //    return View();
        //}
        //public ActionResult Index2()
        //{
        //    var news = from m in db.Articles select m;
        //    if (z != 0)
        //    {
        //        news = news.Where(s => s.CategoryId.Equals(z));
        //    }
        //    var q = news.OrderBy(o => o.ArticleId).Skip(((int)now - 1) * 6).Take(6);
        //    return Json(q, JsonRequestBehavior.AllowGet);
        //    //     return Json(db.Articles, JsonRequestBehavior.AllowGet); 
        //}
        //public ActionResult Index3(int? id)
        //{
        //    Article article = db.Articles.Find(id); //找到             
        //    if (article == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(article); //放到视图里访问 
        //}
        public ActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(db.Categorys, "CategoryId", "CategoryName");
            return View();
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(Article a)
        {
            if (ModelState.IsValid) //如果数据符合要求 （ 按照规则校验） 
            {
                db.Articles.Add(a);
                db.SaveChanges();
            }
            return RedirectToAction("index");
        }
        public ActionResult Edit(int id)
        {
            Article a = db.Articles.SingleOrDefault(m => m.ArticleId == id);
            ViewData["CategoryId"] = new SelectList(db.Categorys, "CategoryId", "CategoryName", a.CategoryId);
            return View(a);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(Article article) //修改的时候提交的数据时一个类 
        {
            if (ModelState.IsValid)
            {
                db.Entry(article).State = EntityState.Modified;// 修改 
                db.SaveChanges(); //保存                 
                return RedirectToAction("index");
            }

            return View("index");
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }
        [HttpPost]
        public ActionResult Delete(int id) //删除的时候提交的数据只是id  和修改不一样 
        {
            Article article = db.Articles.Find(id); //找到             
            db.Articles.Remove(article);            //删除             
            db.SaveChanges();                       //保存             
            return RedirectToAction("index");
        }

        public ActionResult Details(int? id) //删除的时候提交的数据只是id  和修改不一样	 
        {
            Article article = db.Articles.Find(id); //找到	 
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article); //放到视图里访问	 	
        }

    }
}