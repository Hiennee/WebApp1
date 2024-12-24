using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp1.Models;

public partial class InvoiceDetail
{
    public int InvoiceDetailId { get; set; }
    
    public string InvoiceId { get; set; } = null!;
    [Required(ErrorMessage = "Vui lòng nhập mã sản phẩm")]
    public string ProductId { get; set; } = null!;
    [Required(ErrorMessage = "Vui lòng nhập số lượng sản phẩm")]
    public int Quantity { get; set; }

    public double TotalPrice { get; set; }

    public virtual Invoice Invoice { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
