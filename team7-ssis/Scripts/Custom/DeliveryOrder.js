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
                { data: "DeliveryOrderNo" },
                { data: "PurchaseOrderNo" },
                { data: "SupplierName" },
                { data: "CreatedDate" },
                { data: "Status" },
                { defaultContent: '<input type="button" value="i" id="infobutton" />' }
            ],

        createdRow: function (row, data, dataIndex) {

            if (data.Status == "Partially Delivered") {

                $('td', row).eq(4).addClass('partially-delivered');

            }

            if (data.Status == "Awaiting Delivery") {

                $('td', row).eq(4).addClass('awaiting-delivery');

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
                { data: "PurchaseOrderNo" },
                { data: "SupplierName" },
                { data: "CreatedDate" },
                { data: "Status" }
            ],
        select: "single",

        createdRow: function (row, data, dataIndex) {

            if (data.Status == "Partially Delivered") {

                $('td', row).eq(3).addClass('partially-delivered');

            }

            if (data.Status == "Awaiting Delivery") {

                $('td', row).eq(3).addClass('awaiting-delivery');
            }
        }
    });

    var pon = $("#poNo").val();
    var don = $("#doNo").val();

    //for receivegoodsview-outstanding items
    var oTable = $('#myOutstandingTable').DataTable({
        ajax: {
            url: "/api/purchaseorder/details/" + pon,
            dataSrc: ""
        },

        columns:
            [
                { data: "ItemCode" },
                { data: "Description" },
                { data: "QuantityOrdered" },
                { defaultContent: '<input type="textbox" class="textbox" />' },
                { data: "RemainingQuantity" },
                { defaultContent: '<input type="checkbox" class="checkbox" />' }
            ],
        select: "single"
    });

    //for DOConfirmationPage-outstanding items
    var dTable = $('#myDOTable').DataTable({
        ajax: {
            url: "api/deliveryorderdetails/"+don,
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

    $('#confirm').click(function () {
        var mydata = $('#myTable').DataTable().rows({ selected: true }).data().toArray();
        alert(mydata[0].PurchaseOrderNo);
        var ponum = mydata[0].PurchaseOrderNo;

        var form = document.createElement("form");

        var element1 = document.createElement("input");

        form.method = "POST";

        form.action = "/deliveryorder/receivegoodsview";

        element1.value = ponum;

        element1.name = "ponum";

        element1.type = "hidden";

        form.appendChild(element1);

        document.body.appendChild(form);

        form.submit();

    });

    //$('#myTable tbody').on('click', '#edit', function (e) 
    $('#infobutton').on('click', function (e) {
        var mydata = $('#myDOTable').DataTable().rows({ selected: true }).data().toArray();
        alert(mydata[0].DeliveryOrderNo);
        var donum = mydata[0].DeliverOrderNo;

        var form = document.createElement("form");

        var element1 = document.createElement("input");

        form.method = "POST";

        form.action = "/deliveryorder/doconfirmationview";

        element1.value = donum;

        element1.name = "donum";

        element1.type = "hidden";

        form.appendChild(element1);

        document.body.appendChild(form);

        form.submit();

    });
});
