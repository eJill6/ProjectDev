using BackSideWeb.Model.Entity.MM;
using JxBackendService.Model.Entity.Base;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BackSideWeb.Models.ViewModel
{
    public class AdminBookingRefundInputModel
    {
        public string RefundId { get; set; }
        public byte Status { get; set; }
        public string? Memo { get; set; }
        public string? ExamineMan { get; set; }
    }
}
