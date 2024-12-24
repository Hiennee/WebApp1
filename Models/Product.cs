using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp1.Models;

public partial class Product
{
    [Required(ErrorMessage = "Mã sản phẩm không được để trống")]
    [RegularExpression("^SP\\d+$", ErrorMessage = "Mã SP phải bắt đầu bằng SP")]
    public string ProductId { get; set; } = null!;
    [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
    public string ProductName { get; set; } = null!;
    [Required(ErrorMessage = "Giá tiền không được để trống")]
    public double Price { get; set; }

    public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();
}
