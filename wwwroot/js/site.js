

//$(document).ready(function () {
//    var products = [];
//    var savedProducts = [];
//    var productList = [];

//    $.ajax({
//        url: "/Products/GetProducts",
//        type: "GET",
//        success: function (response) {
//            console.log("Ayo" + response)
//            products = response;
//        },
//        error: function (e) {
//            console.log("no" + e);
//        }
//    })



//    var grid = $("#productsGrid").kendoGrid({
//        dataSource: {

//            data: savedProducts,
//            schema: {
//                model: {
//                    fields: {
//                        productName: { type: "string" },
//                        quantity: { type: "number", validation: { min: 1 } },
//                        price: { type: "number" },
//                        totalPrice: { type: "number" },
//                        //totalPrice: { type: "number" },
//                    }
//                }
//            }
//        },
//        editable: true,
//        toolbar: ["create"],
//        scrollable: true,
//        columns: [
//            {
//                field: "productName",
//                title: "Tên sản phẩm",
//                editor: productSelectList,
//                template: "#= productName #"
//            },
//            {
//                field: "quantity",
//                title: "Số lượng",
//                editor: quantityInput,
//                template: "#= quantity #"
//            },
//            {
//                field: "price",
//                title: "Đơn giá",
//                template: "#= price #"
//            },
//            {
//                field: "totalPrice",
//                title: "Thành tiền",
//            },
//            {
//                command: {
//                    text: "Xóa",
//                    click: function (e) {
//                        e.preventDefault();


//                        var tr = $(e.target).closest("tr");
//                        var dataItem = this.dataItem(tr);
//                        console.log("event:", e, "\n e.target: ", e.target, "\ntr: ", tr, "\ndataItem: ", dataItem, "\nDatasource: ", this.dataSource)
//                        if (confirm("Bạn có chắc chắn muốn xóa sản phẩm này?")) {
//                            this.dataSource.remove(dataItem);
//                        }
//                    }
//                },
//                title: "Hành động"
//            }
//        ]
//    }).data("kendoGrid");

//    grid.tbody.on("change", "td", function (e) {
//        var idx = $(e.target).closest("tr").index();
//        var price = grid.dataSource.view()[idx].price;
//        var quantity = grid.dataSource.view()[idx].quantity;
//        var totalPrice = price * quantity;
//        grid.dataSource.view()[idx].totalPrice = totalPrice;
//        var totalPriceCell = $(e.target).closest("tr").find("td:nth-child(4)");
//        totalPriceCell.html(totalPrice.toFixed(2));
//    })

//    $("#btnAdd").click(function () {
//        //await $("#btnSave").click();
//        var data = grid.dataSource.view();

//        // Map each entry to a promise for calculateTotalPrice
//        savedProducts = data.map(function (d) {
//            return {
//                productId: d.productId,
//                quantity: d.quantity,
//                totalPrice: d.price * d.quantity,
//            };
//        });
//        if (savedProducts.length == 0) {
//            alert("Lưu ít nhất một sản phẩm để tạo hóa đơn");
//            return;
//        }
//        var json = {
//            invoiceId: $("#invoiceId").val(),
//            invoiceDate: $("#invoiceDate").val(),
//            customerId: $("#customerId").val(),
//        };
//        $.ajax({
//            url: "/Invoices/Create",
//            type: "POST",
//            data: json,
//            success: function (response) {
//                if (response) {
//                    console.log("s length", savedProducts.length);
//                    savedProducts.forEach(function (p) {
//                        console.log("product p", p);
//                        console.log(typeof p)
//                        var invoiceDetail = {
//                            InvoiceId: $("#invoiceId").val(),
//                            ProductId: p.productId,
//                            Quantity: p.quantity,
//                            TotalPrice: p.totalPrice,
//                        };
//                        console.log(typeof invoiceDetail);
//                        console.log("i", invoiceDetail)
//                        $.ajax({
//                            url: "InvoiceDetails/Create",
//                            type: "POST",
//                            data: invoiceDetail,
//                            success: function (response) {
//                                alert("Thêm hóa đơn mới thành công " + response);
//                                $("#CreateInvoiceModal").modal("hide");
//                                window.location.href = "/Invoices";
//                            },
//                            error: function (error) {
//                                alert(error);
//                                console.log(error);
//                            }
//                        })
//                    });


//                } else {
//                    alert("Số liệu không hợp lệ");
//                }
//            },
//            error: function (error) {
//                alert("Lỗi khi thêm hóa đơn: " + error);
//            },
//        });

//    });

//    $("#btnAddMore").click(function () {
//        //alert("We");
//        var json = {
//            invoiceId: $("#invoiceId").val(),
//            invoiceDate: $("#invoiceDate").val(),
//            customerId: $("#customerId").val(),
//        };
//        $.ajax({
//            url: "/Invoices/CreateNew",
//            type: "POST",
//            data: json,
//            success: function (response) {
//                if (response) {
//                    alert("Thêm hóa đơn mới thành công" + response);
//                    $("#invoiceId").val("");
//                    $("#invoiceDate").val("");
//                    $("#customerId").val("");
//                }
//                else {
//                    alert("Số liệu không hợp lệ");
//                }
//            },
//            error: function (error) {
//                alert("Lỗi khi thêm hóa đơn: " + error);
//            }
//        })
//    });

//    function productSelectList(container, options) {
//        console.log(container, options);
//        $('<select data-bind="value:' + options.field + '"></select>')
//            .appendTo(container)
//            .kendoDropDownList({
//                dataSource: products,
//                dataTextField: "productName",
//                dataValueField: "productId",
//                optionLabel: "Chọn sản phẩm",
//                change: function () {
//                    var selectedProduct = this.dataItem();
//                    console.log("this là:", this);
//                    console.log("dataItem:", selectedProduct);
//                    options.model.productName = selectedProduct ? selectedProduct.productName : "";
//                    options.model.productId = selectedProduct ? selectedProduct.productId : null;
//                    options.model.price = selectedProduct ? selectedProduct.price : 0;
//                    grid.refresh();
//                    //options.model.totalPrice = selectedProduct? calculateTotalPrice() : 0;
//                }
//            });
//    }

//    // Custom numeric input editor for quantity
//    function quantityInput(container, options) {
//        $('<input type="number" data-bind="value:' + options.field + '" class="k-input"/>')
//            .appendTo(container)
//            .attr("min", 1); // Minimum value for quantity
//    }

////    // Utility function to get product name by ID
//    function getProductName(productId) {
//        var product = products.find(p => p.ProductId == productId);
//        return product ? product.ProductName : "";
//    }

//    function calculateTotalPrice(price, quantity) {
//        return price * quantity;
//    }
//})