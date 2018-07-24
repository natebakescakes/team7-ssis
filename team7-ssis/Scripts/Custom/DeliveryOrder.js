$(document).ready(function () {

    var action_dropbox = function (data, type, row, meta) {

        if (data == "Delivered") {

            return '<select  class="action form-control form-control-sm"><option value="-1">Action</option><option value="0">View Purchase Order details</option> <option value=1>View Related Delivery Orders</option></select>';

        }

        else if (data == "Partially Delivered") {

            return '<select  class=" action form-control form-control-sm"><option value="-1">Action</option><option value="0">View Purchase Order details</option> <option value=1>View Related Delivery Orders</option><option value=2 > Receive Goods</ option></select>';

        }

        else {

            return '<select class="action form-control form-control-sm"><option value="-1">Action</option><option value="0">View Purchase Order details</option> <option value=2 > Receive Goods</ option></select>';

        }

    }
    //Receive goods- View DeliveryOrders

    var pTable = $('#myPOTable').DataTable({
        ajax: {
            url: "/api/receivegoods/all",
            dataSrc: ""
        },
        columns:
            [
                { defaultContent: '<input type="checkbox" class="checkbox" />' },
                { data: "DeliveryOrderNo" },
                { data: "PurchaseOrderNo" },
                { data: "SupplierName" },
                { data: "CreatedDate" },
                { data: "Status" }
            ],

        createdRow: function (row, data, dataIndex) {

            if (data.Status == "Partially Delivered") {

                $('td', row).eq(5).addClass('partially-delivered');

            }

            if (data.Status == "Awaiting Delivery") {

                $('td', row).eq(5).addClass('awaiting-delivery');

            }

        }
    });

    //for Index Page
    var Table = $('#myTable').DataTable({
        ajax: {
            url: "api/outstandingpo/all",
            dataSrc: ""
        },
        columns:
            [
                { defaultContent: '<input type="checkbox" class="checkbox" />' },
                { data: "PurchaseOrderNo" },
                { data: "SupplierName" },
                { data: "CreatedDate" },
                { data: "Status" }
            ],
        select: "single",

        createdRow: function (row, data, dataIndex) {

            if (data.Status == "Partially Delivered") {

                $('td', row).eq(5).addClass('partially-delivered');

            }

            if (data.Status == "Awaiting Delivery") {

                $('td', row).eq(5).addClass('awaiting-delivery');
            }
        }
    });

    //for receivegoodsview-outstanding items
    var oTable = $('#myOutstandingTable').DataTable({
        ajax: {
            url: "/api/purchaseOrder/{ id }",
            dataSrc: ""
        },

        columns:
            [
                {data: "ItemCode"},
                {data: "Description"},
                {data: "QuantityOrdered"},
                { defaultContent: '<input type="textbox" class="textbox" />'},
                {data: "RemainingQuantity"},
                {defaultContent: '<input type="checkbox" class="checkbox" />' }
            ]
    });

    //for DOConfirmationPage-outstanding items
    var dTable = $('#myDOTable').DataTable({
        ajax: {
            url: "/api/outstandingitems/all",
            dataSrc: ""
        },

        columns:
            [
                { data: "ItemCode" },
                { data: "Description" },
                { data: "QuantityOrdered" },
                { data: "ReceivedQuantity" }
            ]
    });
});
