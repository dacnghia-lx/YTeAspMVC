using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YTeAspMVC.Daos;

namespace YTeAspMVC.Controllers
{
    public class PostController : Controller
    {
        PostDao postDao = new PostDao();
        // GET: Post
        public ActionResult Index(string keyword)
        {
            ViewBag.List = String.IsNullOrWhiteSpace(keyword) ? postDao.GetAll() : postDao.Search(keyword);
            ViewBag.Keyword = keyword;
            Session.Add("Active", "New");
            return View();
        }

        public ActionResult Detail(int id)
        {
            ViewBag.Post = postDao.GetPost(id);
            return View();
        }
    }
}
