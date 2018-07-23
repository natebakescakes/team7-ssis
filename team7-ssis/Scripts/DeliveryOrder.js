$(document).ready(function ()
{  
     var poTable = $('#myDOTable').DataTable({
        ajax: {
            url: "api/receivegoods/all",
            dataSrc: ""
        },

        columns: [
            { data: "DeliveryOrderNo" },
            { data: "PurchaseOrderNo" },
            { data: "SupplierName" },
            { data: "OrderDate" },
            { data: "Status" }
        ],
        select: "single"
    });  
})
