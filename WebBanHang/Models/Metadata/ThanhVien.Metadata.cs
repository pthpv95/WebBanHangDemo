using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBanHang.Models
{
    [MetadataTypeAttribute(typeof(ThanhVienMetadata))]
    public partial class ThanhVien
    {
        internal sealed class ThanhVienMetadata
        {
            [DisplayName("Ma thanh vien")]
            public int MaThanhVien { get; set; }
            [Required(ErrorMessage = "{0} không được để trống")]
            [DisplayName("Tai khoan")]
            public string TaiKhoan { get; set; }
            [Required(ErrorMessage = "{0} không được để trống")]
            [DisplayName("Mat khau")]
            public string MatKhau { get; set; }
            [Required(ErrorMessage = "{0} không được để trống")]
            [DisplayName("Ho ten")]
            public string HoTen { get; set; }
            [Required(ErrorMessage = "{0} không được để trống")]
            [DisplayName("Dia chi")]
            public string DiaChi { get; set; }
            [Required(ErrorMessage = "{0} không được để trống")]
            [DisplayName("Email")]
            public string Email { get; set; }
            [Required(ErrorMessage = "{0} không được để trống")]
            [DisplayName("So dien thoai")]
            public string SoDienThoai { get; set; }
            [Required(ErrorMessage = "{0} không được để trống")]
            [DisplayName("Cau hoi")]
            public string CauHoi { get; set; }
            [Required(ErrorMessage = "{0} không được để trống")]
            [DisplayName("Cau tra loi")]
            public string CauTraLoi { get; set; }
            [Required(ErrorMessage = "{0} không được để trống")]
            [DisplayName("Ma loai thanh vien")]
            public Nullable<int> MaLoaiTV { get; set; }
        }
    }
}