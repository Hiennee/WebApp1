using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp1.Models;

public partial class Invoice
{
    [Required(ErrorMessage = "Mã hóa đơn không được để trống")]
    [RegularExpression("^HD\\d+$", ErrorMessage = "Mã HD phải bắt đầu bằng HD")]
    public string InvoiceId { get; set; } = null!;
    [Required(ErrorMessage = "Vui lòng nhập khách hàng lập hóa đơn")]
    public string? CustomerId { get; set; }
    [Required(ErrorMessage = "Vui lòng nhập ngày lập hóa đơn")]
    public DateTime? InvoiceDate { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();
}
