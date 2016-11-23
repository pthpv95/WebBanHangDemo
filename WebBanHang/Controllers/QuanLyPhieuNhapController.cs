using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;
namespace WebBanHang.Controllers
{
    public class QuanLyPhieuNhapController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: QuanLyPhieuNhap
        [HttpGet]
        public ActionResult NhapHang()
        {
            //tạo dropdown list cách 1
            //ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.MaNCC), "MaNCC", "TenNCC");
            //ViewBag.ListSanPham = new SelectList(db.SanPhams.OrderBy(n => n.MaSP), "MaSP", "TenSP");
            //tạo dropdown list cách 2
            ViewBag.MaNCC = db.NhaCungCaps;
            ViewBag.ListSanPham = db.SanPhams;
            return View();
        }
        [HttpPost]
        public ActionResult NhapHang(PhieuNhap model,IEnumerable<ChiTietPhieuNhap> lstModel)
        {

            ViewBag.MaNCC = db.NhaCungCaps;
            ViewBag.ListSanPham = db.SanPhams;
            model.DaXoa = false;
            db.PhieuNhaps.Add(model);
            db.SaveChanges();
            //SaveChanges để lấy mã phiếu nhập gắn cho lstChiTietPhieuNap
            SanPham sp;
            foreach(var item in lstModel)
            {
                //Cập nhật số lương tồn
                sp = db.SanPhams.Single(n => n.MaSP == item.MaSp);
                sp.SoLuongTon += item.SoLuongNhap;
                // gán mã phiếu nhập cho tất cả chi tiết phiếu nhập
                item.MaPN = model.MaPN;
            }
            db.ChiTietPhieuNhaps.AddRange(lstModel);
            db.SaveChanges();
            return View();

        }
        [HttpGet]
        public ActionResult DSSPHetHang()
        {
            var lstSP = db.SanPhams.Where(n => n.SoLuongTon < 5 && n.DaXoa == false);
            return View(lstSP);
        }
        [HttpGet]
        public ActionResult NhapHangDon(int? id)
        {
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.MaNCC), "MaNCC", "TenNCC");
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if(sp == null)
            {
                return HttpNotFound();
            }
            return View(sp);
        }
        [HttpPost]
        public ActionResult NhapHangDon(PhieuNhap model, ChiTietPhieuNhap ctpn)
        {
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.MaNCC), "MaNCC", "TenNCC",model.MaNCC);
            model.DaXoa = false;
            model.NgayNhap = DateTime.Now;
            db.PhieuNhaps.Add(model);
            db.SaveChanges();
            SanPham sp = db.SanPhams.Single(n => n.MaSP == ctpn.MaSp);
            sp.SoLuongTon += ctpn.SoLuongNhap;
            ctpn.MaPN = model.MaPN;
            db.ChiTietPhieuNhaps.Add(ctpn);
            db.SaveChanges();
            return View(sp);
        }
    }
}