using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;
using System.IO;
namespace WebBanHang.Controllers
{
    public class QuanLySanPhamController : Controller
    {
        // GET: QuanLySanPham
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public ActionResult Index()
        {
            return View(db.SanPhams.Where(n=>n.DaXoa==false));
        }
        [HttpGet]
        public ActionResult TaoMoi()
        {
            //load dropdown list nha cung cap
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.MaNCC),"MaNCC","TenNCC");
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai");
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX");


            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult TaoMoi(SanPham sp, HttpPostedFileBase [] HinhAnh)
        {
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.MaNCC), "MaNCC", "TenNCC");
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai");
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX");
            int loi = 0;
            for (int i = 0; i < HinhAnh.Count(); i++)
            {
                if (HinhAnh[i] != null)
                {
                    //Kiểm tra nội dung hình ảnh
                    if (HinhAnh[i].ContentLength > 0)
                    {
                        //Kiểm tra định dạng hình ảnh
                        if (HinhAnh[i].ContentType != "image/jpeg" && HinhAnh[i].ContentType != "image/png" && HinhAnh[i].ContentType != "image/gif" && HinhAnh[i].ContentType != "image/jpg")
                        {
                            ViewBag.upload += "Hình ảnh" + i + " không hợp lệ <br />";
                            loi++;
                        }
                        else
                        {
                            //Kiểm tra hình ảnh tồn tại

                            //Lấy tên hình ảnh
                            var fileName = Path.GetFileName(HinhAnh[0].FileName);
                            //Lấy hình ảnh chuyển vào thư mục hình ảnh 
                            var path = Path.Combine(Server.MapPath("~/Content/Pictures"), fileName);
                            //Nếu thư mục chứa hình ảnh đó rồi thì xuất ra thông báo
                            if (System.IO.File.Exists(path))
                            {
                                ViewBag.upload1 = "Hình " + i + "đã tồn tại <br />";
                                loi++;

                            }
                        }
                    }
                }

            }
            if (loi > 0)
            {
                return View(sp);
            }
            sp.HinhAnh = HinhAnh[0].FileName;
            sp.HinhAnh1 = HinhAnh[1].FileName;
            sp.HinhAnh2 = HinhAnh[2].FileName;
            sp.HinhAnh3 = HinhAnh[3].FileName;



            //if (HinhAnh.ContentLength > 0)
            //{
            //    // bước 1 lấy tên Hình ảnh
            //    var fileName = Path.GetFileName(HinhAnh.FileName);
            //    //Lây hỉnh ảnh move folder HinhAnh
            //    var path = Path.Combine(Server.MapPath("~/Content/Pictures"), fileName);
            //    //Nếu thư mục đó chứa hình ảnh đó ròi thì xuất ra thông báo
            //    if (System.IO.File.Exists(path))
            //    {
            //        ViewBag.UpLoad = "Hình đã tồn tại";
            //        return View();
            //    }
            //    else
            //    {
            //        // lấy hình ảnh đưa vào thư mục pictures
            //        HinhAnh.SaveAs(path);
            //        sp.HinhAnh = fileName;
            //    }
            //}
            db.SanPhams.Add(sp);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ChinhSua(int? id)
        {
            //Lấy sp cần chinh3 sửa
            if(id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                return HttpNotFound();
            }
            //Load dropdown list NCC, MaLoaiSP, MaNSX
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.MaNCC), "MaNCC", "TenNCC",sp.MaNCC);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai",sp.MaLoaiSP);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX",sp.MaNSX);
            return View(sp);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChinhSua(SanPham model)
        {
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.MaNCC), "MaNCC", "TenNCC", model.MaNCC);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai", model.MaLoaiSP);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX", model.MaNSX);

            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //Action xóa sp
        [HttpGet]
        public ActionResult Xoa(int? id)
        {
            //Lấy sp cần chinh3 sửa
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                return HttpNotFound();
            }
            //Load dropdown list NCC, MaLoaiSP, MaNSX
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.MaNCC), "MaNCC", "TenNCC", sp.MaNCC);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai", sp.MaLoaiSP);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX", sp.MaNSX);

            return View(sp);
        }
        [HttpPost]
        public ActionResult Xoa(int id)
        {
            //Lấy sp cần chinh3 sửa
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                return HttpNotFound();
            }
            db.SanPhams.Remove(sp);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}