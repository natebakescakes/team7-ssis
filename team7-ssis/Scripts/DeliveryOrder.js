$(document).ready(function ()
{  
    var oTable = $('#myTable').DataTable({
        ajax: {
            url: "api/receivegoods/all",
            dataSrc: "",
                },

                columns: [
                    {data: "DeliveryOrderNo"},
                    {data: ""},
                    {data: "Supplier_SupplierCode"}
                ],
                select: "single"
    });  

    var oTable = $('#myPOTable').DataTable({
        ajax: {
            url: "api/receivegoods/all",
            dataSrc: "",
        },

        columns: [
            { data: "DeliveryOrderNo" },
            { data: "PurchaseOrder_PurchaseOrderNo" },
            { data: "Supplier_SupplierCode" }
        ],
        select: "single"
    });  
})