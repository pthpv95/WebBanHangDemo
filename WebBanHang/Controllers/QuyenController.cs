using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;
namespace WebBanHang.Controllers
{
    [Authorize(Roles ="QuanLyQuyen")]
    public class QuyenController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: Quyen
        [HttpGet]
        public ActionResult Index()
        {
            return View(db.Quyens.OrderBy(n=>n.TenQuyen));
        }

        
    }
}