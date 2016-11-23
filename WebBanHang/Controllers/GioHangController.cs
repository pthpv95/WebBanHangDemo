using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;
namespace WebBanHang.Controllers
{
    public class GioHangController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // lấy giỏ hàng
        public List<ItemGioHang> LayGioHang()
        {
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                // neu session gio hang chua ton tai thi khoi tao gio hang
                lstGioHang = new List<ItemGioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang; 
        }

        //Thêm giỏ hàng thông thường load lai trang 
        public ActionResult ThemGioHang(int MaSP, string strURL)
        {
            //Kiem tra sp co tồn tại trong csdl hay không
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if(sp == null)
            {
                //trag đương dẫn ko hợp lệ
                Response.StatusCode = 404;
                return null;
            }
            //Lây giỏ hàng
            List<ItemGioHang> lstGioHang = LayGioHang();
            //Kiểm tra sp đã tồn tại trong giỏ hàng ?
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if(spCheck != null)
            {
                //kiểm tra sớ lượng tồn trước khi cho khách đặt hàng
                if(sp.SoLuongTon < spCheck.SoLuong)
                {
                    return View("ThongBao");
                }
                spCheck.SoLuong++;
                spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
                return Redirect(strURL);
            }

            ItemGioHang itemGH = new ItemGioHang(MaSP);
                if (sp.SoLuongTon < itemGH.SoLuong)
                {
                    return View("ThongBao");
                }
            lstGioHang.Add(itemGH);
            return Redirect(strURL);
        }

        // tính tồng số lượng
        public double TinhTongSoLuong()
        {
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if(lstGioHang== null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.SoLuong);
        }

        // Tính tổng tiền
        public decimal TinhTongTien()
        {
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.ThanhTien);
        }

        public ActionResult GioHangPartial()
        {
            if (TinhTongSoLuong() == 0)
            {
                ViewBag.TongSoLuong = 0;
                ViewBag.TongTien = 0;
                return PartialView();
            }
            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongSoTien = TinhTongTien(); 
            return PartialView();
        }

        // GET: GioHang
        public ActionResult XemGioHang()
        {
            //Lấy item giỏ hàng
            List<ItemGioHang> lstGioHang = LayGioHang();
            return View(lstGioHang);
        }
        
        //Chỉnh sửa giỏ hàng
        [HttpGet]
        public ActionResult SuaGioHang(int MaSP)
        {
            //Kiểm tra session giò hàng tồn tại ?
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lấy giỏ hàng từ session
            List<ItemGioHang> lstGH = LayGioHang();
            //kiểm tra sp đó có tồn tại trong giỏ hàng hay không
            ItemGioHang spCheck = lstGH.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //Lay list Gio hang de tao giao dien
            ViewBag.GioHang = lstGH;
            //Neu ton tai =>
            return View(spCheck);
        }
        //Cập nhật giỏ hàng
        //Xử lý cập nhật giỏ hàng
        [HttpPost]
        public ActionResult CapNhatGioHang(ItemGioHang itemGH)
        {
            //Kiểm tra số lượng tồn 
            SanPham spCheck = db.SanPhams.Single(n => n.MaSP == itemGH.MaSP);
            if (spCheck.SoLuongTon < itemGH.SoLuong)
            {
                return View("ThongBao");
            }
            //Cập nhật số lượng trong session giỏ hàng 
            //Bước 1: Lấy List<GioHang> từ session["GioHang"]
            List<ItemGioHang> lstGH = LayGioHang();
            //Bước 2: Lấy sản phẩm cần cập nhật từ trong list<GioHang> ra
            ItemGioHang itemGHUpdate = lstGH.Find(n => n.MaSP == itemGH.MaSP);
            //Bước 3: Tiến hành cập nhật lại số lượng cũng thành tiền
            itemGHUpdate.SoLuong = itemGH.SoLuong;
            itemGHUpdate.ThanhTien = itemGHUpdate.SoLuong * itemGHUpdate.DonGia;
            return RedirectToAction("XemGioHang");
        }


        //xóa giỏ hàng
        public ActionResult XoaGioHang(int MaSP)
        {
            //Kiểm tra sp có tồn tai trong csdl hay không
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if(sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lấy giỏ hàng từ session
            List<ItemGioHang> lstGH = LayGioHang();
            //kiểm tra sp đó có tồn tại trong giỏ hàng hay không
            ItemGioHang spCheck = lstGH.SingleOrDefault(n => n.MaSP == MaSP);
            if(spCheck == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //xòa item trong giỏ hàng
            lstGH.Remove(spCheck);
            return RedirectToAction("XemGioHang");
        }

        //Đặt hàng 
        public ActionResult DatHang(KhachHang kh)
        {
            //Kiểm tra sp có tồn tai trong csdl hay không
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            KhachHang khang = new KhachHang();
            if (Session["TaiKhoan"] == null)
            {
                //Thêm khach hang vao bang khach hang dối với kh vảng lai
                khang = kh;
                db.KhachHangs.Add(khang);
                db.SaveChanges();
            }
            else
            {
                //Đối với KH là thành viên
                ThanhVien tv = Session["TaiKhoan"] as ThanhVien;
                khang.TenKH = tv.HoTen;
                khang.DiaChi = tv.DiaChi;
                khang.Email = tv.Email;
                khang.SoDienThoai = tv.SoDienThoai;
                db.KhachHangs.Add(khang);
                db.SaveChanges();
            }
            //Thêm đơn hàng
            DonDatHang ddh = new DonDatHang();
            ddh.MaKH = khang.MaKH;
            ddh.NgayDat = DateTime.Now;
            ddh.TinhTrangDonHang = false;
            ddh.DaThanhToan = false;
            ddh.UuDai = 0;
            db.DonDatHangs.Add(ddh);
            db.SaveChanges();
            //Thêm chi tiết đơn đặt hàng
            List<ItemGioHang> lst = LayGioHang();
            foreach(var item in lst)
            {
                ChiTietDonDatHang ctddh = new ChiTietDonDatHang();
                ctddh.MaDDH = ddh.MaDDH;
                ctddh.MaSP = item.MaSP;
                ctddh.TenSP = item.TenSP;
                ctddh.SoLuong = item.SoLuong;
                ctddh.DonGia = item.DonGia;
                db.ChiTietDonDatHangs.Add(ctddh);
                
            }
            db.SaveChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XemGioHang");
        }


        //Thêm giỏ hàng bằng Ajax
        public ActionResult ThemGioHangAjax(int MaSP)
        {
            //Kiem tra sp co tồn tại trong csdl hay không
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                //trag đương dẫn ko hợp lệ
                Response.StatusCode = 404;
                return null;
            }
            //Lây giỏ hàng
            List<ItemGioHang> lstGioHang = LayGioHang();
            //Kiểm tra sp đã tồn tại trong giỏ hàng ?
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck != null)
            {
                //kiểm tra sớ lượng tồn trước khi cho khách đặt hàng
                if (sp.SoLuongTon < spCheck.SoLuong)
                {
                    return Content("<script> alert(\"Sản phẩm đã hết hàng!\")</script>");
                }
                spCheck.SoLuong++;
                spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
                ViewBag.TongSoLuong = TinhTongSoLuong();
                ViewBag.TongTien = TinhTongTien();
                return PartialView("GioHangPartial");
            }

            ItemGioHang itemGH = new ItemGioHang(MaSP);
            if (sp.SoLuongTon < itemGH.SoLuong)
            {
                return Content("<script> alert(\"Sản phẩm đã hết hàng!\")</script>");
            }
            lstGioHang.Add(itemGH);
            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongTien = TinhTongTien();
            return PartialView("GioHangPartial");
        }
    }
}