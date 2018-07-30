
$(document).ready(function () {

 
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

            this.api().columns([4]).every(function () {

                var column = this;

                var select = $('<select multiple id="sel1" title="All Statuses" data-width="auto" data-style="btn-sm" class=" selectpicker  " ></select>')

                    .prependTo($('.dataTables_filter'))

                var download = $('<a class=" btn  btn-primary pull-left mr-3 btn-sm btn" href="#"><i class="fa fa-download" ></i>  Download Selected</a>').prependTo($('#poTable_length'))

                var select1 = $('#sel1').on('change', function () {

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
            url: "/api/receivegoods/" + pon,
            dataSrc: ""
        },
        columns:
            [
                { data: "DeliveryOrderNo" },
                { data: "PurchaseOrderNo" },
                { data: "SupplierName" },
                { data: "CreatedDate" },
                { data: "Status" },
                { defaultContent: '<input type="button" value="i" id="ibtn" />' }
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

    // clicks i button from view delivery orders when purchase order no given
    $('#myRGTable tbody').on('click', '#ibtn', function (e) {

        var dno = rgTable.row($(this).parents('tr')).data().DeliveryOrderNo;

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
                { defaultContent: '<input type="button" value="i" id="infobtn" />' }
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


    $('#myOutstandingTable tbody').on("change", "#qty", function () {

        alert("hi");
        var cell = oTable.cell(this.parentElement);

        // assign the cell with the value from the <input> element 

        cell.data($(this).val()).draw();

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
                {
                   defaultContent:'<input type="textbox" id="qty" />'
                },
                { data: "RemainingQuantity" },
                {
                    defaultContent: '<input type="checkbox" id="vcheck" />'
                }
            ],
        select: "single"
    });

    var k = 0;
    //checkbox 
    $('#myOutstandingTable tbody').on('change', '#vcheck', function (e) {
        var rowSelected = oTable.row($(this).parents('tr')).data().ItemCode;
        
        //if ($(this).is(":checked")) {
        //    k = 1;
        //}
        //else {
        //    k = 0;
        //}


        //alert(k);
    });

  
    $('#submitbtn').click(function () {
        var mydata = oTable.rows().data().toArray();

        var details = new Array();
        mydata[0].ReceivedQty = 12;
        mydata[1].ReceivedQty = 0;
        mydata[2].ReceivedQty = 2;


        
        for (var i = 0; i < mydata.length; i++) {

            if (mydata[i].ReceivedQty > mydata[i].QuantityOrdered) {
                alert("Received Quantity cannot be greater than Quantity Ordered");
            }
        }

        alert(k);


            var o = {

                "PurchaseOrderNo": pon,

                "ItemCode": mydata[i].ItemCode,

                "Description": mydata[i].Description,

                "QtyOrdered": mydata[i].QuantityOrdered,

                "ReceivedQty": mydata[i].ReceivedQty,

                "RemainingQuantity": mydata[i].RemainingQuantity,

                "CheckBoxStatus":k
            };

            details.push(o);
        }


        $.ajax({

            type: "POST",

            url: "/DeliveryOrder/Save",

            dataType: "json",

            data: JSON.stringify(details),

            contentType: "application/json",

            cache: true,

            success: function (data) {

                alert("Delivery Order information has been successfully saved");
                //if (data == success)
                //    window.location.href = "/DeliveryOrder/Index";
            }

        });
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

    $('#VRDObtn').click(function () {

        alert(pon);

        var form1 = document.createElement("form");

        var element1 = document.createElement("input");

        form1.method = "POST";

        form1.action = "/deliveryorder/ReceivedGoodsPurchaseOrderView";

        element1.value = pon;

        element1.name = "pon";

        element1.type = "hidden";

        form1.appendChild(element1);

        document.body.appendChild(form1);

        form1.submit();

    });

    // clicks i button from view delivery orders when purchase order no given
    $('#myTable tbody').on('click', '#infobtn', function (e) {

        var pno = Table.row($(this).parents('tr')).data().PurchaseOrderNo;
        alert(pno);


        var form = document.createElement("form");

        var element1 = document.createElement("input");

        form.method = "POST";

        form.action = "/purchaseorder/doconfirmationview";

        element1.value = pno;

        element1.name = "pno";

        element1.type = "hidden";

        form.appendChild(element1);

        document.body.appendChild(form);

        form.submit();

    });

    $('#Viewbtn').click(function () {

        alert(pon);

        var form1 = document.createElement("form");

        var element1 = document.createElement("input");

        form1.method = "POST";

        form1.action = "/purchaseorder/ReceivedGoodsPurchaseOrderView";

        element1.value = pon;

        element1.name = "pon";

        element1.type = "hidden";

        form1.appendChild(element1);

        document.body.appendChild(form1);

        form1.submit();

    });
});

 //    //var thisRow = oTable.row($(this).parents('tr'));
    //    //var receivedCell = thisRow.cell(4);
    //    //var outstandingCell = thisRow.cell(2);
    //    //var remainingCell = thisRow.cell(3);
    //    alert(cell);
    //    // assign the cell with the value from the <input> element 

    //    //var remainingValue = outstandingCell.data() - receivedCell.data();
    //    //alert(remainingValue);
    //    //remainingCell.data(remainingValue).draw();

