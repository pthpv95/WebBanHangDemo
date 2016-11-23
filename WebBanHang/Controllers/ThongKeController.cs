using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;
namespace WebBanHang.Controllers
{
    public class ThongKeController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: ThongKe
        public ActionResult Index()
        {
            ViewBag.SoNguoiTruyCap = HttpContext.Application["SoNguoiTruyCap"].ToString();//Lấy số lượng người truy cập.
            ViewBag.SoNguoiDangOnline = HttpContext.Application["SoNguoiDangOnline"].ToString();
            ViewBag.ThongKeThanhVien = ThongKeThanhVien();
            ViewBag.ThongKeDoanhThu = ThongKeDoanhThu();
            ViewBag.ThongKeDonHang = ThongKeDonHang();
            return View();
        }
        public double ThongKeDonHang()
        {
            double lstDonHang = db.DonDatHangs.Count();
            return lstDonHang;
        }
        public int ThongKeThanhVien()
        {
            int tv = db.ThanhViens.Count();
            return tv;
        }
        public decimal ThongKeDoanhThu()
        {
            decimal lstDoanhThu = db.ChiTietDonDatHangs.Sum(n => n.SoLuong * n.DonGia).Value;
            return lstDoanhThu;
        }
        public decimal ThongKeDoanhThuThang(int Thang, int Nam)
        {
            var lstDDH = db.DonDatHangs.Where(n => n.NgayDat.Value.Month == Thang && n.NgayDat.Value.Year == Nam);
            //duyet chi tiet don dat hang va lay tong tien cua tat ca cac san pham thuoc don hang do
            decimal TongTien = 0;
            foreach (var item in lstDDH)
            {
                TongTien += decimal.Parse(item.ChiTietDonDatHangs.Sum(n => n.SoLuong * n.DonGia).Value.ToString());
            }
            return TongTien;
        }
    }
}