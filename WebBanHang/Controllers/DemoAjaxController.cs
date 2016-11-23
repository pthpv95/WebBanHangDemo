using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;
namespace WebBanHang.Controllers
{
    public class DemoAjaxController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: DemoAjax
        public ActionResult DemoAjax()
        {
            return View();
        }
        public ActionResult LoadAjaxActionLink()
        {
            System.Threading.Thread.Sleep(2000);
            return Content("Hello Ajax");
        }
        //Xu li ajax Beginform
        public ActionResult LoadAjaxBeginForm(FormCollection collection)
        {
            string kq = collection["txt1"].ToString();
            return Content(kq);
        }
        public ActionResult LoadAjaxJQuerry(int a , int b)
        {
            
            return Content((a+b).ToString());
        }


        public ActionResult LoadSanPhamAjax()
        {
            //Lay du lieu sp la nike va moi
            //var lstSanPhamLTM = db.SanPhams.Where(n => n.MaLoaiSP == 2 && n.Moi == 1);
            var lstSanPham = db.SanPhams;
            return PartialView(lstSanPham);
        }
    }
}