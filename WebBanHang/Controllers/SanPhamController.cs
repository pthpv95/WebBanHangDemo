using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;
using PagedList;
namespace WebBanHang.Controllers
{
    public class SanPhamController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: SanPham
        //public ActionResult SanPham1()
        //{
        //    var lstSanPhamLTM = db.SanPhams.Where(n => n.MaLoaiSP == 2 && n.Moi == 1);
        //    ViewBag.ListSP = lstSanPhamLTM;

        //    var lstTshirt = db.SanPhams.Where(n => n.MaLoaiSP == 4);
        //    ViewBag.ListTshirt = lstTshirt;

        //    return View();
        //}
        //public ActionResult SanPham2()
        //{
        //    var lstSanPhamLTM = db.SanPhams.Where(n => n.MaLoaiSP == 2 && n.Moi == 1);
        //    ViewBag.ListSP = lstSanPhamLTM;
        //    return View();
        //}
        //public ActionResult SanPhamPartial()
        //{
        //    //Lay du lieu sp la nike va moi
        //    var lstSanPhamLTM = db.SanPhams.Where(n => n.MaLoaiSP == 2 && n.Moi == 1);
        //    return PartialView(lstSanPhamLTM);
        //}
        [ChildActionOnly]
        public ActionResult SanPhamStyle1Partial()
        {
            return PartialView();
        }
        [ChildActionOnly]
        public ActionResult SanPhamStyle2Partial()
        {
            return PartialView();
        }
        public ActionResult XemChiTiet(int? id,string tensp)
        {
            if(id == null){
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id );
            if (sp == null)
            {
                return HttpNotFound();
            }
            return View(sp);
        }
        //action load sp theo ma loai sp va nsx
        public ActionResult SanPham(int? MaLoaiSP,int? MaNSX,int? page)
        {
            //kiểm tra phân quyền đơn giản
            //if(Session["TaiKhoan"]==null || Session["TaiKhoan"].ToString() == "")
            //{
            //    return RedirectToAction("Index", "Home");
            //}

            if (MaLoaiSP == null || MaNSX == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var lstSP = db.SanPhams.Where(n => n.MaLoaiSP == MaLoaiSP && n.MaNSX == MaNSX);
            if (lstSP.Count() == 0)
            {
                return HttpNotFound(); 
            }
            //Thực hiện chức năng phân trang
            //Tạo biến số sp trên trang
            int PageSize = 3;
            //Tạo biến thứ 2 số trang hiện tại
            int PageNumber = (page ?? 1);
            ViewBag.MaLoaiSP = MaLoaiSP;
            ViewBag.MaNSX = MaNSX;

            return View(lstSP.OrderBy(n=>n.MaSP).ToPagedList(PageNumber,PageSize));
        }
    }
}