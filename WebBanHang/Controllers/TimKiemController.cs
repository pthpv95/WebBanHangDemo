using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;
using PagedList;
namespace WebBanHang.Controllers
{
    public class TimKiemController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: TimKiem
        [HttpGet]
        public ActionResult KQTimKiem(string sTuKhoa, int? page)
        {
            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }
            int PageSize = 3;
            int pageNumber = (page ?? 1);
            //tim kiem theo ten san pham
            var lstSP = db.SanPhams.Where(n => n.TenSP.Contains(sTuKhoa));
            var soSP = db.SanPhams.Where(n => n.TenSP.Contains(sTuKhoa)).Count();
            ViewBag.soSP = soSP;
            ViewBag.TuKhoa = sTuKhoa;
            return View(lstSP.OrderBy(n => n.TenSP).ToPagedList(pageNumber, PageSize));
        }

        [HttpPost]
        public ActionResult LayTuKhoaTimKiem(string sTuKhoa)
        {

            return RedirectToAction("KQTimKiem", new { @sTuKhoa = sTuKhoa });
        }

        public ActionResult KQTimKiemPartial(string sTuKhoa)
        {
            var lstSP = db.SanPhams.Where(n => n.TenSP.Contains(sTuKhoa));
            ViewBag.TuKhoa = sTuKhoa;
            return PartialView(lstSP.OrderBy(n => n.DonGia));
        }
    }
}