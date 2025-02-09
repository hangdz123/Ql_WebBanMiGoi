﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThanhThoaiRestaurant.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;
using System.Text.Json;
using Newtonsoft.Json;
using System.Linq;
using X.PagedList;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing.Printing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ThanhThoaiRestaurant.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminDonHangController : Controller
    {
        private readonly QuanLyNhaHangContext _context;

        public AdminDonHangController(QuanLyNhaHangContext context)
        {
            _context = context;
        }
        public IActionResult Index(int? page, string search, DateTime? startDate, DateTime? endDate, int pageSize = 3)
        {
            if (HttpContext.Session.GetString("TenDangNhap") != null && (HttpContext.Session.GetString("VaiTro") == "Admin" || HttpContext.Session.GetString("VaiTro") == "LeTan" || HttpContext.Session.GetString("VaiTro") == "ThuNgan"))
            {



                var query = _context.DonHangs
     .OrderByDescending(tb => tb.NgayDatHang)
     .AsQueryable();


                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(d =>
                         d.MaDonHang.ToString().Contains(search) ||
                         d.MaHd.ToString().Contains(search) ||
                         d.NguoiNhan.Contains(search) ||
                         d.SDTNN.Contains(search) ||
                         d.DiaChiNhan.Contains(search) ||
                         d.GhiChu.Contains(search) ||
                         d.ChiTietDhs.Any(ct =>
                             ct.MaMon.ToString().Contains(search) ||
                             ct.TenMonAnDh.Contains(search) ||
                             ct.SoLuongMmdh.ToString().Contains(search)));


                }

                if (startDate.HasValue || endDate.HasValue)
                {
                    if (startDate.HasValue && !endDate.HasValue)
                    {
                        query = query.Where(d => d.NgayDatHang.Date >= startDate.Value.Date && d.NgayDatHang.Date <= DateTime.Now.Date);
                    }
                    else if (!startDate.HasValue && endDate.HasValue)
                    {
                        query = query.Where(d => d.NgayDatHang.Date <= endDate.Value.Date);
                    }
                    else if (startDate.HasValue && endDate.HasValue)
                    {
                        if (startDate.Value.Date == endDate.Value.Date)
                        {
                            query = query.Where(d => d.NgayDatHang.Date == startDate.Value.Date);
                        }
                        else
                        {
                            query = query.Where(d => d.NgayDatHang.Date >= startDate.Value.Date && d.NgayDatHang.Date <= endDate.Value.Date);
                        }
                    }
                }


                int pageNumber = page ?? 1;
                var pagedList = query.ToPagedList(pageNumber, pageSize);


                int startItem = (pageNumber - 1) * pageSize + 1;
                int endItem = Math.Min(startItem + pageSize - 1, pagedList.TotalItemCount);

                int maxVisiblePages = Math.Min(pagedList.PageCount, 5); // Tối đa 5 trang, nhưng không nhiều hơn tổng số trang
                int startPage = Math.Max(1, pageNumber - (maxVisiblePages / 2));
                int endPage = Math.Min(pagedList.PageCount, startPage + maxVisiblePages - 1);

                ViewBag.Search = search;
                ViewBag.StartDate = startDate;
                ViewBag.EndDate = endDate;

                ViewBag.TotalItems = pagedList.TotalItemCount;
                ViewBag.TotalPages = pagedList.PageCount;
                ViewBag.PageNumber = pageNumber;
                ViewBag.PageSize = pageSize;
                ViewBag.StartItem = startItem;
                ViewBag.EndItem = endItem;
                ViewBag.MaxVisiblePages = maxVisiblePages;
                ViewBag.StartPage = startPage;
                ViewBag.EndPage = endPage;
                ViewBag.StartPage = startPage;
                ViewBag.EndPage = endPage;

                return View(pagedList);
            }
            else
            {
                return Redirect("/Account/Login");
            } 
                
            
        }

        public IActionResult ChoXacNhan(int? page, string search, DateTime? startDate, DateTime? endDate, int pageSize = 3)
        {

            if (HttpContext.Session.GetString("TenDangNhap") != null && (HttpContext.Session.GetString("VaiTro") == "Admin" || HttpContext.Session.GetString("VaiTro") == "LeTan" || HttpContext.Session.GetString("VaiTro") == "ThuNgan"))
            {



                var query = _context.DonHangs
                .Where(d => d.TrangThaiDh == 1)
                .OrderByDescending(tb => tb.NgayDatHang)
                .AsQueryable();

                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(d =>
                         d.MaDonHang.ToString().Contains(search) ||
                         d.MaHd.ToString().Contains(search) ||
                         d.NguoiNhan.Contains(search) ||
                         d.SDTNN.Contains(search) ||
                         d.DiaChiNhan.Contains(search) ||
                         d.GhiChu.Contains(search) ||
                         d.ChiTietDhs.Any(ct =>
                             ct.MaMon.ToString().Contains(search) ||
                             ct.TenMonAnDh.Contains(search) ||
                             ct.SoLuongMmdh.ToString().Contains(search)));
                }

                if (startDate.HasValue || endDate.HasValue)
                {
                    if (startDate.HasValue && !endDate.HasValue)
                    {
                        query = query.Where(d => d.NgayDatHang.Date >= startDate.Value.Date && d.NgayDatHang.Date <= DateTime.Now.Date);
                    }
                    else if (!startDate.HasValue && endDate.HasValue)
                    {
                        query = query.Where(d => d.NgayDatHang.Date <= endDate.Value.Date);
                    }
                    else if (startDate.HasValue && endDate.HasValue)
                    {
                        if (startDate.Value.Date == endDate.Value.Date)
                        {
                            query = query.Where(d => d.NgayDatHang.Date == startDate.Value.Date);
                        }
                        else
                        {
                            query = query.Where(d => d.NgayDatHang.Date >= startDate.Value.Date && d.NgayDatHang.Date <= endDate.Value.Date);
                        }
                    }
                }


                int pageNumber = page ?? 1;
                var pagedList = query.ToPagedList(pageNumber, pageSize);


                int startItem = (pageNumber - 1) * pageSize + 1;
                int endItem = Math.Min(startItem + pageSize - 1, pagedList.TotalItemCount);

                int maxVisiblePages = Math.Min(pagedList.PageCount, 5); // Tối đa 5 trang, nhưng không nhiều hơn tổng số trang
                int startPage = Math.Max(1, pageNumber - (maxVisiblePages / 2));
                int endPage = Math.Min(pagedList.PageCount, startPage + maxVisiblePages - 1);

                ViewBag.Search = search;
                ViewBag.StartDate = startDate;
                ViewBag.EndDate = endDate;

                ViewBag.TotalItems = pagedList.TotalItemCount;
                ViewBag.TotalPages = pagedList.PageCount;
                ViewBag.PageNumber = pageNumber;
                ViewBag.PageSize = pageSize;
                ViewBag.StartItem = startItem;
                ViewBag.EndItem = endItem;
                ViewBag.MaxVisiblePages = maxVisiblePages;
                ViewBag.StartPage = startPage;
                ViewBag.EndPage = endPage;
                ViewBag.StartPage = startPage;
                ViewBag.EndPage = endPage;

                return View(pagedList);
            }
            else
            {
                return Redirect("/Account/Login");
            }
        }

        public IActionResult DangGiao(int? page, string search, DateTime? startDate, DateTime? endDate, int pageSize = 3)
            {
                if (HttpContext.Session.GetString("TenDangNhap") != null && (HttpContext.Session.GetString("VaiTro") == "Admin" || HttpContext.Session.GetString("VaiTro") == "LeTan" || HttpContext.Session.GetString("VaiTro") == "ThuNgan"))
                {

                

                    var query = _context.DonHangs
                     .Where(d => d.TrangThaiDh == 2)
                     .OrderByDescending(tb => tb.NgayDatHang)
                     .AsQueryable();
                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(d =>
                        d.MaDonHang.ToString().Contains(search) ||
                        d.MaHd.ToString().Contains(search) ||
                        d.NguoiNhan.Contains(search) ||
                        d.SDTNN.Contains(search) ||
                        d.DiaChiNhan.Contains(search) ||
                        d.GhiChu.Contains(search) ||
                        d.ChiTietDhs.Any(ct =>
                            ct.MaMon.ToString().Contains(search) ||
                            ct.TenMonAnDh.Contains(search) ||
                            ct.SoLuongMmdh.ToString().Contains(search)));
                }

                if (startDate.HasValue || endDate.HasValue)
                {
                    if (startDate.HasValue && !endDate.HasValue)
                    {
                        query = query.Where(d => d.NgayDatHang.Date >= startDate.Value.Date && d.NgayDatHang.Date <= DateTime.Now.Date);
                    }
                    else if (!startDate.HasValue && endDate.HasValue)
                    {
                        query = query.Where(d => d.NgayDatHang.Date <= endDate.Value.Date);
                    }
                    else if (startDate.HasValue && endDate.HasValue)
                    {
                        if (startDate.Value.Date == endDate.Value.Date)
                        {
                            query = query.Where(d => d.NgayDatHang.Date == startDate.Value.Date);
                        }
                        else
                        {
                            query = query.Where(d => d.NgayDatHang.Date >= startDate.Value.Date && d.NgayDatHang.Date <= endDate.Value.Date);
                        }
                    }
                }


                int pageNumber = page ?? 1;
                var pagedList = query.ToPagedList(pageNumber, pageSize);


                int startItem = (pageNumber - 1) * pageSize + 1;
                int endItem = Math.Min(startItem + pageSize - 1, pagedList.TotalItemCount);

                int maxVisiblePages = Math.Min(pagedList.PageCount, 5); // Tối đa 5 trang, nhưng không nhiều hơn tổng số trang
                int startPage = Math.Max(1, pageNumber - (maxVisiblePages / 2));
                int endPage = Math.Min(pagedList.PageCount, startPage + maxVisiblePages - 1);

                ViewBag.Search = search;
                ViewBag.StartDate = startDate;
                ViewBag.EndDate = endDate;

                ViewBag.TotalItems = pagedList.TotalItemCount;
                ViewBag.TotalPages = pagedList.PageCount;
                ViewBag.PageNumber = pageNumber;
                ViewBag.PageSize = pageSize;
                ViewBag.StartItem = startItem;
                ViewBag.EndItem = endItem;
                ViewBag.MaxVisiblePages = maxVisiblePages;
                ViewBag.StartPage = startPage;
                ViewBag.EndPage = endPage;
                ViewBag.StartPage = startPage;
                ViewBag.EndPage = endPage;

                return View(pagedList);
            }
            else
                {
                    return Redirect("/Account/Login");
                }
        }

        public IActionResult DaGiao(int? page, string search, DateTime? startDate, DateTime? endDate, int pageSize = 3)
            {
                if (HttpContext.Session.GetString("TenDangNhap") != null && (HttpContext.Session.GetString("VaiTro") == "Admin" || HttpContext.Session.GetString("VaiTro") == "LeTan" || HttpContext.Session.GetString("VaiTro") == "ThuNgan"))
                {


                    var query = _context.DonHangs
                 .Where(d => d.TrangThaiDh == 3)
                 .OrderByDescending(tb => tb.NgayDatHang)
                 .AsQueryable();
                    if (!string.IsNullOrEmpty(search))
                    {
                        query = query.Where(d =>
                            d.MaDonHang.ToString().Contains(search) ||
                            d.MaHd.ToString().Contains(search) ||
                            d.NguoiNhan.Contains(search) ||
                            d.SDTNN.Contains(search) ||
                            d.DiaChiNhan.Contains(search) ||
                            d.GhiChu.Contains(search) ||
                            d.ChiTietDhs.Any(ct =>
                                ct.MaMon.ToString().Contains(search) ||
                                ct.TenMonAnDh.Contains(search) ||
                                ct.SoLuongMmdh.ToString().Contains(search)));
                    }

                    if (startDate.HasValue || endDate.HasValue)
                    {
                        if (startDate.HasValue && !endDate.HasValue)
                        {
                            query = query.Where(d => d.NgayDatHang.Date >= startDate.Value.Date && d.NgayDatHang.Date <= DateTime.Now.Date);
                        }
                        else if (!startDate.HasValue && endDate.HasValue)
                        {
                            query = query.Where(d => d.NgayDatHang.Date <= endDate.Value.Date);
                        }
                        else if (startDate.HasValue && endDate.HasValue)
                        {
                            if (startDate.Value.Date == endDate.Value.Date)
                            {
                                query = query.Where(d => d.NgayDatHang.Date == startDate.Value.Date);
                            }
                            else
                            {
                                query = query.Where(d => d.NgayDatHang.Date >= startDate.Value.Date && d.NgayDatHang.Date <= endDate.Value.Date);
                            }
                        }
                    }


                    int pageNumber = page ?? 1;
                    var pagedList = query.ToPagedList(pageNumber, pageSize);


                    int startItem = (pageNumber - 1) * pageSize + 1;
                    int endItem = Math.Min(startItem + pageSize - 1, pagedList.TotalItemCount);

                    int maxVisiblePages = Math.Min(pagedList.PageCount, 5); // Tối đa 5 trang, nhưng không nhiều hơn tổng số trang
                    int startPage = Math.Max(1, pageNumber - (maxVisiblePages / 2));
                    int endPage = Math.Min(pagedList.PageCount, startPage + maxVisiblePages - 1);

                    ViewBag.Search = search;
                    ViewBag.StartDate = startDate;
                    ViewBag.EndDate = endDate;

                    ViewBag.TotalItems = pagedList.TotalItemCount;
                    ViewBag.TotalPages = pagedList.PageCount;
                    ViewBag.PageNumber = pageNumber;
                    ViewBag.PageSize = pageSize;
                    ViewBag.StartItem = startItem;
                    ViewBag.EndItem = endItem;
                    ViewBag.MaxVisiblePages = maxVisiblePages;
                    ViewBag.StartPage = startPage;
                    ViewBag.EndPage = endPage;
                    ViewBag.StartPage = startPage;
                    ViewBag.EndPage = endPage;

                    return View(pagedList);
                }
                else
                {
                    return Redirect("/Account/Login");
                }
            }

        public IActionResult Huy(int? page, string search, DateTime? startDate, DateTime? endDate, int pageSize = 3)
        {
            if (HttpContext.Session.GetString("TenDangNhap") != null && (HttpContext.Session.GetString("VaiTro") == "Admin" || HttpContext.Session.GetString("VaiTro") == "LeTan" || HttpContext.Session.GetString("VaiTro") == "ThuNgan"))
            {


                var query = _context.DonHangs
                 .Where(d => d.TrangThaiDh == 4)
                 .OrderByDescending(tb => tb.NgayDatHang)
                 .AsQueryable();
                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(d =>
                         d.MaDonHang.ToString().Contains(search) ||
                         d.MaHd.ToString().Contains(search) ||
                         d.NguoiNhan.Contains(search) ||
                         d.SDTNN.Contains(search) ||
                         d.DiaChiNhan.Contains(search) ||
                         d.GhiChu.Contains(search) ||
                         d.ChiTietDhs.Any(ct =>
                             ct.MaMon.ToString().Contains(search) ||
                             ct.TenMonAnDh.Contains(search) ||
                             ct.SoLuongMmdh.ToString().Contains(search)));
                }

                if (startDate.HasValue || endDate.HasValue)
                {
                    if (startDate.HasValue && !endDate.HasValue)
                    {
                        query = query.Where(d => d.NgayDatHang.Date >= startDate.Value.Date && d.NgayDatHang.Date <= DateTime.Now.Date);
                    }
                    else if (!startDate.HasValue && endDate.HasValue)
                    {
                        query = query.Where(d => d.NgayDatHang.Date <= endDate.Value.Date);
                    }
                    else if (startDate.HasValue && endDate.HasValue)
                    {
                        if (startDate.Value.Date == endDate.Value.Date)
                        {
                            query = query.Where(d => d.NgayDatHang.Date == startDate.Value.Date);
                        }
                        else
                        {
                            query = query.Where(d => d.NgayDatHang.Date >= startDate.Value.Date && d.NgayDatHang.Date <= endDate.Value.Date);
                        }
                    }
                }


                int pageNumber = page ?? 1;
                var pagedList = query.ToPagedList(pageNumber, pageSize);


                int startItem = (pageNumber - 1) * pageSize + 1;
                int endItem = Math.Min(startItem + pageSize - 1, pagedList.TotalItemCount);

                int maxVisiblePages = Math.Min(pagedList.PageCount, 5); // Tối đa 5 trang, nhưng không nhiều hơn tổng số trang
                int startPage = Math.Max(1, pageNumber - (maxVisiblePages / 2));
                int endPage = Math.Min(pagedList.PageCount, startPage + maxVisiblePages - 1);

                ViewBag.Search = search;
                ViewBag.StartDate = startDate;
                ViewBag.EndDate = endDate;

                ViewBag.TotalItems = pagedList.TotalItemCount;
                ViewBag.TotalPages = pagedList.PageCount;
                ViewBag.PageNumber = pageNumber;
                ViewBag.PageSize = pageSize;
                ViewBag.StartItem = startItem;
                ViewBag.EndItem = endItem;
                ViewBag.MaxVisiblePages = maxVisiblePages;
                ViewBag.StartPage = startPage;
                ViewBag.EndPage = endPage;
                ViewBag.StartPage = startPage;
                ViewBag.EndPage = endPage;

                return View(pagedList);
            }
            else
            {
                return Redirect("/Account/Login");

            }
        }

        public IActionResult Details( int id)
        {

            var donHang = _context.DonHangs.FirstOrDefault(dh => dh.MaDonHang == id);
            var chiTietDonHang = _context.ChiTietDhs.Where(ctdh => ctdh.MaDonHang == id).ToList();
            int maHoaDon = donHang.MaHd;
            var hoaDon = _context.HoaDons.FirstOrDefault(hd => hd.MaHd == maHoaDon);
            var chiTietHoaDon = _context.ChiTietHds.Where(cthd => cthd.MaHd == maHoaDon).ToList();
            var khachHang = _context.KhachHangs.FirstOrDefault(kh => kh.MaKH == donHang.MaKH);
            if (donHang == null)
            {
                return View("Error");
            }

            var ChiTietDonHang = new List<ChiTietDh>();

            var hoaDonViewModel = new HoaDonViewModel
            {
                TenKh = donHang.NguoiNhan,
                Sdtkh = donHang.SDTNN,
                EmailKh = khachHang.EmailKh,
                GioiTinhKh = khachHang.GioiTinhKh,
                DiaChiKh = donHang.DiaChiNhan,
                MaDonHang = donHang.MaDonHang,
                NgayDatHang = donHang.NgayDatHang,
                TrangThaiDh = donHang.TrangThaiDh,
                HinhThucTT = hoaDon.HinhThucTT,
                GhiChu = donHang.GhiChu,
                MaKH = donHang.MaKH,
                MaHd = donHang.MaHd,
                ChiTietDonHang = chiTietDonHang,
                ChiTietHoaDon = chiTietHoaDon,
                TongTien = (float)hoaDon.TongTien,
                TienGiam = (float)hoaDon.TienGiam,
                TienTt = (float)hoaDon.TienTt,

            };

            return View(hoaDonViewModel);
        }

        [HttpPost]
        public IActionResult UpdateTrangThai(int maDonHang, int newTrangThai)
        {
            var donHang = _context.DonHangs.FirstOrDefault(d => d.MaDonHang == maDonHang);
            if (donHang != null)
            {
                donHang.TrangThaiDh = newTrangThai;
                _context.SaveChanges();

                // Kiểm tra newTrangThai
                if (newTrangThai == 3)
                {
                    // Tìm hoá đơn với MaHd tương ứng và cập nhật trạng thái thành 2
                    var hoaDon = _context.HoaDons.FirstOrDefault(hd => hd.MaHd == donHang.MaHd);
                    if (hoaDon != null)
                    {
                        hoaDon.TrangThaiHD = 2;
                        _context.SaveChanges();
                    }
                }

                return Json(new { success = true });
            }
            return Json(new { success = false });
        }


    }

}
