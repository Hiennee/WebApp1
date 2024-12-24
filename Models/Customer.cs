using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp1.Models;

public partial class Customer
{
    [Required(ErrorMessage = "Mã khách hàng không được để trống")]
    [RegularExpression("^KH\\d+$", ErrorMessage = "Mã KH phải bắt đầu bằng KH")]
    public string CustomerId { get; set; } = null!;
    [Required(ErrorMessage = "Tên khách hàng không được để trống")]
    public string CustomerName { get; set; } = null!;
    [Required(ErrorMessage = "Số điện thoại không được để trống")]
    [RegularExpression("^0\\d{8,9}$", ErrorMessage = "Số điện thoại không hợp lệ")]
    public string Phone { get; set; } = null!;

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}
