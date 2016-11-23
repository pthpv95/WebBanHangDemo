using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;

namespace WebBanHang.Controllers
{
   [Authorize(Roles = "QuanTri,QuanLySanPham")]
    public class KhachHangController : Controller
    {
        // GET: KhachHang
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        [Authorize(Roles = "QuanLySanPham")]
        public ActionResult Index()
        {
            var lstKH = db.KhachHangs;
            return View(lstKH);
        }
        public ActionResult TruyVan1DoiTuong()
        {
            //var lstKH = from kh in db.KhachHangs where kh.MaKH==2 select kh ;
            //KhachHang khach = lstKH.FirstOrDefault();
            KhachHang khach = db.KhachHangs.SingleOrDefault(n => n.MaKH == 2);
            return View(khach);
        }
        [Authorize(Roles = "QuanTri")]
        public ActionResult SortDuLieu()
        {
            List<KhachHang> lstKH = db.KhachHangs.OrderByDescending(n => n.TenKH).ToList();
            return View(lstKH);
        }

        public ActionResult GroupDuLieu()
        {
            List<ThanhVien> lstKH = db.ThanhViens.OrderByDescending(n => n.TaiKhoan).ToList();
            return View(lstKH);
        }
    }
}