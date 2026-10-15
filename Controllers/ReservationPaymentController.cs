using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Encodings.Web;
using System.Collections.Generic;

namespace GiapDepTrai.Models
{
    public class ReservationPaymentController : Controller
    {
        private readonly QlChuoiNhaHang1Context _context;
        public ReservationPaymentController(QlChuoiNhaHang1Context context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult CreatePayment(int reservationId, int customerId)
        {  
            var reservation = _context.TblReservations
                .Include(r => r.TblReservationMenuItems)
                .ThenInclude(c => c.MenuItem)
                .Include(r => r.Customer)
                .Include(r => r.TblReservationPayments)
                .FirstOrDefault(r => r.CustomerId == customerId && r.ReservationId == reservationId);

            if (reservation == null)
            {
                return NotFound();
            }
            var count_amount = _context.TblReservations
                    .Include(r => r.TblReservationMenuItems)
                        .ThenInclude(ri => ri.MenuItem)
                    .FirstOrDefault(r => r.ReservationId == reservationId);

                decimal tongTien = 0;

            if (count_amount != null)
            {
                tongTien = count_amount.TblReservationMenuItems
                    .Sum(ri => ri.Quantity * (ri.MenuItem.Price ?? 0) * 0.5m);

                ViewBag.Amount = tongTien;
                }
            

            if (reservation.TblReservationPayments == null)
            {
                reservation.TblReservationPayments = new List<TblReservationPayment>();
            }

            return View(reservation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePayment(TblReservationPayment payment, int customerId, int reservationId)
        {
            var currentDate = DateTime.Now;

            System.Diagnostics.Debug.WriteLine($"Nhận dữ liệu: PaymentMethod: {payment?.PaymentMethod}, AmountPaid: {payment?.AmountPaid}, CustomerId: {customerId}, ReservationId: {reservationId}, Reservation: {(payment?.Reservation != null ? payment.Reservation.ReservationId : "null")}");
            
            var reservation = _context.TblReservations
                .Include(r => r.Customer)
                .Include(r => r.TblReservationPayments)
                .FirstOrDefault(r => r.ReservationId == reservationId);

            if (reservation == null)
            {
                System.Diagnostics.Debug.WriteLine($"Không tìm thấy reservation với ReservationId: {reservationId}");
                return NotFound();
            }

            if (ModelState.ContainsKey("payment.Reservation"))
            {
                ModelState.Remove("payment.Reservation");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    payment.ReservationId = reservationId;
                    payment.PaymentDate = currentDate;
                    payment.CustomerId = customerId;
                    payment.Reservation = reservation;

                    System.Diagnostics.Debug.WriteLine($"Trước khi lưu: PaymentMethod: {payment.PaymentMethod}, AmountPaid: {payment.AmountPaid}, ReservationId: {payment.ReservationId}, CustomerId: {payment.CustomerId}, PaymentDate: {payment.PaymentDate}, Reservation: {(payment.Reservation != null ? payment.Reservation.ReservationId : "null")}");

                    _context.TblReservationPayments.Add(payment);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = JavaScriptEncoder.Default.Encode("Thanh toán thành công!");
                    System.Diagnostics.Debug.WriteLine("Lưu thành công vào database.");
                    return RedirectToAction("Result" ,new {paymentId = payment.PaymentId});
                }
                catch (Exception ex)
                {
                    var errorMessage = $"Lỗi khi lưu thanh toán: {ex.Message}";
                    if (ex.InnerException != null)
                    {
                        errorMessage += $"\nInnerException: {ex.InnerException.Message}";
                        if (ex.InnerException.InnerException != null)
                        {
                            errorMessage += $"\nInnerInnerException: {ex.InnerException.InnerException.Message}";
                        }
                    }
                    System.Diagnostics.Debug.WriteLine(errorMessage);
                    ModelState.AddModelError("", $"Đã xảy ra lỗi khi lưu thanh toán: {ex.Message}");
                }
            }
            else
            {
                foreach (var error in ModelState)
                {
                    foreach (var err in error.Value.Errors)
                    {
                        System.Diagnostics.Debug.WriteLine($"ModelState Error - Key: {error.Key}, Error: {err.ErrorMessage}");
                    }
                }
            }

            if (reservation.TblReservationPayments == null)
            {
                reservation.TblReservationPayments = new List<TblReservationPayment>();
            }
            return View(reservation);
        }
        [HttpGet]
        public IActionResult Result(int paymentId)
        {
            var reservation = _context.TblReservationPayments
                .Include(r => r.Reservation)
                .ThenInclude(c => c.Customer)
                .FirstOrDefault(s => s.PaymentId == paymentId);
            if (reservation is null)
            {
                return NotFound();
            }
            return View(reservation);
        }
        [HttpGet]
        public IActionResult History(int page = 1, int pageSize = 10)
        {
            var customerId = HttpContext.Session.GetInt32("CustomerID");
            var query = _context.TblReservationPayments
                .Include(r => r.Reservation)
                .ThenInclude(c => c.Customer)
                .Where(c => c.CustomerId == customerId)
                .ToList();
            if (query is null)
            {
                return NotFound();
            }
            int totalItems = query.Count();
            var reservations = query
                .OrderByDescending(r => r.ReservationId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            return View(reservations);
        }
    }
}