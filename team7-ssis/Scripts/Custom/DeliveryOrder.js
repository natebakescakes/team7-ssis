
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
                { defaultContent: '<input type="button" value="i" id="infobtn" />'}
            ],

        createdRow: function (row, data, dataIndex) {

            if (data.Status == "Partially Delivered") {

                $('td', row).eq(4).addClass('partially-delivered');

            }

            if (data.Status == "Awaiting Delivery") {

                $('td', row).eq(4).addClass('awaiting-delivery');

            }
        },

        initComplete: function () { // After DataTable initialized

            this.api().columns([4]).every(function () {

                var column = this;

                var select = $('<select multiple id="sel1" title="All Statuses" data-width="auto" data-style="btn-sm" class=" selectpicker  " ></select>')

                    .prependTo($('.dataTables_filter'))

                var download = $('<a class=" btn  btn-primary pull-left mr-3 btn-sm btn" href="#"><i class="fa fa-download" ></i>  Download Selected</a>').prependTo($('#poTable_length'))

                var select = $('#sel1').on('change', function () {

                    var val = $(this).val() + '';

                    column.search(val != '' ? '^' + val.split(',').join('$|^') + '$' : '', true, false).draw();

                });


                column.data().unique().sort().each(function (d, j) {

                    select.append('<option value="' + d + '">' + d + '</option>')

                });

                $('.selectpicker').selectpicker();

            }); // this.api function
        }
    });

    var pon = $("#poNo").val();
    var don = $("#doNo").val();

    //Receive goods- View DeliveryOrders - purchaseordernumber

    var rgTable = $('#myRGTable').DataTable({
        ajax: {
            url: "/api/receivegoods/"+ pon,
            dataSrc: ""
        },
        columns:
            [
                { data: "DeliveryOrderNo" },
                { data: "PurchaseOrderNo" },
                { data: "SupplierName" },
                { data: "CreatedDate" },
                { data: "Status" },
                { defaultContent: '<input type="button" value="i" id="infobtn" />' }
            ],

        createdRow: function (row, data, dataIndex) {

            if (data.Status == "Partially Delivered") {

                $('td', row).eq(4).addClass('partially-delivered');

            }

            if (data.Status == "Awaiting Delivery") {

                $('td', row).eq(4).addClass('awaiting-delivery');

            }

        },

        initComplete: function () { // After DataTable initialized

            this.api().columns([3]).every(function () {

                var column = this;

                var select = $('<select multiple id="sel1" title="All Statuses" data-width="auto" data-style="btn-sm" class=" selectpicker  " ></select>')

                    .prependTo($('.dataTables_filter'))

                var download = $('<a class=" btn  btn-primary pull-left mr-3 btn-sm btn" href="#"><i class="fa fa-download" ></i>  Download Selected</a>').prependTo($('#poTable_length'))

                var select = $('#sel1').on('change', function () {

                    var val = $(this).val() + '';

                    column.search(val != '' ? '^' + val.split(',').join('$|^') + '$' : '', true, false).draw();

                });


                column.data().unique().sort().each(function (d, j) {

                    select.append('<option value="' + d + '">' + d + '</option>')

                });

                $('.selectpicker').selectpicker();

            }); // this.api function
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
                { data: "Status" },
                {defaultContent: '<input type="button" value="i" id="infobtn" />'}
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
                { defaultContent: '<input type="checkbox" class="checkbox" id="vcheck"/>' }
            ],
        select: "single"
    });

   
    //for DOConfirmationPage-outstanding items
    var dTable = $('#myDOTable').DataTable({
        ajax: {
            url: "/api/deliveryorderdetails/"+ don,
            dataSrc: ""
        },

        columns:
            [
                { data: "ItemCode" },
                { data: "Description" },
                { data: "QtyOrdered" },
                { data: "ReceivedQty" }
            ]
    });

    //checkbox 
    $('#myOutstandingTable tbody').on('change', '#vcheck', function (e) {
        var rowSelected = oTable.row($(this).parents('tr')).data().ItemCode;
        alert(rowSelected);
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

    // clicks i button from view delivery orders
    $('#myPOTable tbody').on('click', '#infobtn', function (e) {
      
        var dno = pTable.row($(this).parents('tr')).data().DeliveryOrderNo;

        alert(dno);

        var form = document.createElement("form");

        var element1 = document.createElement("input");

        form.method = "POST";

        form.action = "/deliveryorder/doconfirmationview";

        element1.value = dno;

        element1.name = "dno";

        element1.type = "hidden";

        form.appendChild(element1);

        document.body.appendChild(form);

        form.submit();
    });
      

    // clicks i button from view delivery orders when purchase order no given
    $('#myRGTable tbody').on('click', '#infobtn', function (e) {

        var dno = rgtable.row($(this).parents('tr')).data().DeliveryOrderNo;
        alert(dno);


        var form = document.createElement("form");

        var element1 = document.createElement("input");

        form.method = "POST";

        form.action = "/deliveryorder/doconfirmationview";

        element1.value = dno;

        element1.name = "dno";

        element1.type = "hidden";

        form.appendChild(element1);

        document.body.appendChild(form);

        form.submit();

    });

    // clicks i button from view delivery orders when purchase order no given
    $('#myTable tbody').on('click', '#infobtn', function (e) {

        var pno = Table.row($(this).parents('tr')).data().PurchaseOrderNo;
        alert(pno);


        var form = document.createElement("form");

        var element1 = document.createElement("input");

        form.method = "POST";

        form.action = "/deliveryorder/doconfirmationview";

        element1.value = pno;

        element1.name = "pno";

        element1.type = "hidden";

        form.appendChild(element1);

        document.body.appendChild(form);

        form.submit();

    });
});
