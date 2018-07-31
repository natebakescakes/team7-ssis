$(document).ready(function ()
{  
    var oTable = $('#myTable').DataTable({
        ajax: {
            url: "api/receivegoods/all",
            dataSrc: "",
                },

                columns: [
                    {data: "DeliveryOrderNo"},
                    {data: "PurchaseOrder_PurchaseOrderNo"},
                    {data: "Supplier_SupplierCode"}
                ],
                select: "single"
            });  
})