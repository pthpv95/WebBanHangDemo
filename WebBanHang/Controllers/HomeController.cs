using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;
using CaptchaMvc.HtmlHelpers;
using CaptchaMvc;
using System.Web.Security;

namespace WebBanHang.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public ActionResult Index()
        {
            //Tao viewbag de lay list sp tu csdl
            //New nike shoes
            var lstNike = db.SanPhams.Where(n => n.MaLoaiSP == 1 && n.Moi == 1).ToList();
            ViewBag.ListNike = lstNike;
            //New addias shoes
            var lstAdidas = db.SanPhams.Where(n => n.MaLoaiSP == 2 && n.Moi == 1).ToList();
            ViewBag.ListAdidas = lstAdidas;
            //New New balance shoes
            var lstNB = db.SanPhams.Where(n => n.MaLoaiSP == 5).ToList();
            ViewBag.ListNewBalance = lstNB;
            return View();
        }
        public ActionResult MenuPartial()
        {
            var lstSP = db.SanPhams;
            return PartialView(lstSP);
        }
        //Dang ky
        [HttpGet]
        public ActionResult DangKy1()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy1(ThanhVien tv)
        {
            return View();
        }
        //
        [HttpGet]
        public ActionResult DangKy()
        {
            ViewBag.CauHoi = new SelectList(LoadCauHoi());
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(ThanhVien tv)
        {
            ViewBag.CauHoi = new SelectList(LoadCauHoi());
            //Kiem tra captcha
            if (this.IsCaptchaValid("Captcha is not valid"))
            {
            
                ViewBag.ThongBao = "Them thanh cong";
                //Them thong tin khach hang vao csdl
                db.ThanhViens.Add(tv);
                db.SaveChanges();
                return View();
            }
            ViewBag.ThongBao = "Sai ma captcha";
            return View();
        }
        //load cau hoi bi mat
        public List<string> LoadCauHoi()
        {
            List<string> lstCauHoi = new List<string>();
            lstCauHoi.Add("Con vat ma ban yeu thich");
            lstCauHoi.Add("Ca si ma ban yeu thich");
            lstCauHoi.Add("Ten truyen tranh ban yeu thich");
            return lstCauHoi;
        }
        // dang ky
        [HttpPost]
        public ActionResult DangNhap(FormCollection f)
        {
            //Dùng session để lưu tài khoản
            //string sTaiKhoan = f["txtTenDangNhap"].ToString();
            //string sMatKhau = f["txtMatKhau"].ToString();
            //ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.TaiKhoan == sTaiKhoan && n.MatKhau == sMatKhau);
            //if (tv != null)
            //{
            //    Session["TaiKhoan"] = tv;
            //    return Content("<script>window.location.reload();</script>");
            //}
            //return Content("Tai khoan hoac mat khau khong dung!");

            string TaiKhoan = f["txtTenDangNhap"].ToString();
            string MatKhau = f["txtMatKhau"].ToString();
            //Truy vấn kiểm tra đăng nhập lấy thông tin thành viên
            ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.TaiKhoan == TaiKhoan && n.MatKhau == MatKhau);
            if (tv != null)
            {
                Session["TaiKhoan"] = tv;
                //lấy list quyền  thành viên tương ứng với loại tv
                var lstQuyen = db.LoaiThanhVien_Quyen.Where(n => n.MaLoaiTV == tv.MaLoaiTV);
                string Quyen = "";
                foreach (var item in lstQuyen)
                {
                    Quyen += item.Quyen.MaQuyen + ",";
                }
                Quyen = Quyen.Substring(0, Quyen.Length - 1);
                PhanQuyen(tv.TaiKhoan, Quyen);
                return Content("<script>window.location.reload();</script>");
            }
            return Content("Tai khoan hoac mat khau khong dung!");
        }

        public void PhanQuyen(string TaiKhoan,string Quyen)
        {
            FormsAuthentication.Initialize();
            var ticket = new FormsAuthenticationTicket(1, "TaiKhoan", DateTime.Now, DateTime.Now.AddHours(3),
                false, Quyen, FormsAuthentication.FormsCookiePath);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
            if (ticket.IsPersistent) cookie.Expires = ticket.Expiration;
            Response.Cookies.Add(cookie);
        }
        //Tao trang ngăn chặn quyền truy cập
        public ActionResult LoiPhanQuyen()
        {
            return View();
        }

        //Dang xuat
        public ActionResult DangXuat(FormCollection f)
        {
            Session["TaiKhoan"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index","Home");
        }
    }
}