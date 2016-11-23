using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;
using System.Net.Mail;
namespace WebBanHang.Controllers
{
    public class QuanLyDonHangController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: QuanLyDonHang
        
        public ActionResult ChuaThanhToan()
        {
            var lst = db.DonDatHangs.Where(n => n.DaThanhToan == false).OrderBy(n => n.NgayDat);
            return View(lst);
        }

        public ActionResult ChuaGiao()
        {
            var lst = db.DonDatHangs.Where(n => n.TinhTrangDonHang == false && n.DaThanhToan == true).OrderBy(n=>n.NgayGiao);
            return View(lst);
        }
        public ActionResult DaGiaoDaThanhToan()
        {
            var lst = db.DonDatHangs.Where(n => n.DaThanhToan == true && n.DaThanhToan == true);
            return View(lst);
        }
        [HttpGet]
        public ActionResult DuyetDonHang(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonDatHang model = db.DonDatHangs.SingleOrDefault(n => n.MaDDH == id);
            if(model == null)
            {
                return HttpNotFound();
            }
            //lấy ds chi tiết cho người dùng thấy
            var lstChiTietDDH = db.ChiTietDonDatHangs.Where(n => n.MaDDH==id);
            ViewBag.ListChiTietDH = lstChiTietDDH;
            return View(model);
        }
        [HttpPost]
        public ActionResult DuyetDonHang(DonDatHang ddh) 
        {
            DonDatHang ddhUpdate = db.DonDatHangs.SingleOrDefault(n => n.MaDDH == ddh.MaDDH);
            ddhUpdate.DaThanhToan = ddh.DaThanhToan;
            ddh.TinhTrangDonHang = ddhUpdate.TinhTrangDonHang;
            db.SaveChanges();
            //lấy ds chi tiết cho người dùng thấy
            var lstChiTietDDH = db.ChiTietDonDatHangs.Where(n => n.MaDDH == ddh.MaDDH);
            ViewBag.ListChiTietDH = lstChiTietDDH;
            return View(ddhUpdate);

        }
        public void GuiEmail(string Title, string ToEmail, string FromEmail, string PassWord, string Content)
        {
            // goi email
            MailMessage mail = new MailMessage();
            mail.To.Add(ToEmail); // Địa chỉ nhận
            mail.From = new MailAddress(ToEmail); // Địa chửi gửi
            mail.Subject = Title; // tiêu đề gửi
            mail.Body = Content; // Nội dung
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com"; // host gửi của Gmail
            smtp.Port = 587; //port của Gmail
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential
            (FromEmail, PassWord);//Tài khoản password người gửi
            smtp.EnableSsl = true; //kích hoạt giao tiếp an toàn SSL
            smtp.Send(mail); //Gửi mail đi
        }

        public ActionResult GuiMailPartial(FormCollection f)
        {
            string tieude = f["TieuDe"].ToString();
            string email = f["Email"].ToString();
            string noidung = f["NoiDung"].ToString();
            GuiEmail(tieude, email, "thehienpv@gmail.com", "0985445290H", noidung);
            return RedirectToAction("DaGiaoDaThanhToan", "QuanLyDonHang");
        }
        
    }
}