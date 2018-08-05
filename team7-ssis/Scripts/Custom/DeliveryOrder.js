
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
                { defaultContent: '<button id="infobtn" class="btn btn-default mb-3"><i class="fa fa-info-circle" aria-hidden="true"></i></button>' }
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

                    column.search(val !== '' ? '^' + val.split(',').join('$|^') + '$' : '', true, false).draw();

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
                { defaultContent: '<button id="ibtn" class="btn btn-default mb-3"><i class="fa fa-info-circle" aria-hidden="true"></i></button>' } 
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

                select = $('#sel1').on('change', function () {

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
        pageLength: 5,
        columns:
            [
                { data: "PurchaseOrderNo" },
                { data: "SupplierName" },
                { data: "CreatedDate" },
                { data: "Status" },
                { defaultContent: '<button id="infobtn" class="btn btn-default mb-3"><i class="fa fa-info-circle" aria-hidden="true"></i></button>' }
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
            url: "/api/getpurchaseorderdetails/" + pon,
            dataSrc: ""
        },
        columns:
            [
                { data: "ItemCode" },
                { data: "Description" },
                { data: "QuantityOrdered" },
                { data: "ReceivedQuantity",
                    render: function (data, type, row, meta) {
                        return '<input class="qty" id="' + row.ItemCode + '" type="number" min="0" value="' + data + '"/>';
                    }
                },
                { data: "RemainingQuantity" },

                {
                    data: "CheckBoxStatus",
                    render: function (data, type, row, meta) {
                        var html = '<a class="cancelPOD btn btn-default btn disabled" ><i class="fa fa-close"></i></a>';

                        if (data == 0) {

                            html = '<a  class="cancelPOD btn btn-default btn "  ><i class="fa fa-close"></i></a>';

                        }

                        return html;
                    }
                }
            ],
        select: "single"
    });
   
    // change the value of the cell in the datatable with an input field
    $(document).on("change", ".qty", function () {

        var cell = oTable.cell(this.parentElement);
        cell.data($(this).val()).draw();
   
        var rowIdx = document.getElementById(this.id).parentElement.parentElement;
        
        var QuantityReceived = document.getElementById(this.id).value;

        var QtyOrdered = $('#myOutstandingTable').DataTable().row(document.getElementById(this.id).parentElement.parentElement).data().QuantityOrdered;
       // alert(QtyOrdered);

        if (QuantityReceived <= QtyOrdered) {
            var RemainingQty = parseInt(QtyOrdered) - parseInt(QuantityReceived);
            oTable.cell(rowIdx, 4).data(RemainingQty).draw();
        }
        else
         alert("ReceivedQuantity cannot be greater than outstanding Quantity");   

    });

    // cancel button
    $(document).on("click", ".cancelPOD", function () {

        var itemCode = oTable.row($(this).parents('tr')).data().ItemCode;
        
        $.ajax({

            type: "POST",

            url: "/DeliveryOrder/ChangeStatus",

            dataType: "json",

            data: JSON.stringify({ PurchaseOrderNo: pon, itemCode: itemCode }),

            contentType: "application/json",

            cache: true,

            success: function (data) {
                alert("Purchase Order Detail Cancelled");
                oTable.ajax.reload();
            }
        });
    });

  
    $('#submitbtn').click(function () {

        var mydata = oTable.rows().data().toArray();

        var DOFN = document.getElementById("DeliveryOrderFileName").value;
       
        var IFN = document.getElementById("InvoiceFileName").value;
  
        var details = new Array();
 
        for (var i = 0; i < mydata.length; i++) {

            var o = {

                "PurchaseOrderNo": pon,

                "ItemCode": mydata[i].ItemCode,

                "Description": mydata[i].Description,

                "QtyOrdered": mydata[i].QuantityOrdered,

                "ReceivedQty": mydata[i].ReceivedQuantity,

                "RemainingQuantity": mydata[i].RemainingQuantity,

                "DeliveryOrderFileName": DOFN,

                "InvoiceFileName": IFN
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
                window.location.href = "/DeliveryOrder";
            }

                //error: function () {
                //    $('#info').html('<p>An error has occurred</p>');
                //    oTable.ajax.reload();
                //}
        });
    });

   
    //for DOConfirmationPage-outstanding items
    var dTable = $('#myDOTable').DataTable({
        "paging": false,
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

        var ponum = mydata[0].PurchaseOrderNo;


        if (mydata.length != 0) {

            var form = document.createElement("form");

            var element1 = document.createElement("input");

            form.method = "POST";

            form.action = "/deliveryorder/receivegoodsview";

            element1.value = ponum;

            element1.name = "pon";

            element1.type = "hidden";

            form.appendChild(element1);

            document.body.appendChild(form);

            form.submit();
        }

        else {
            alert('Please select one purchase order!');
        }

    });

    $('#VRDObtn').click(function () {

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

        var form = document.createElement("form");

        var element1 = document.createElement("input");

        form.method = "POST";

        form.action = "/purchaseorder/details/"

        element1.value = pno;

        element1.name = "poNum";

        element1.type = "hidden";

        form.appendChild(element1);

        document.body.appendChild(form);

        form.submit();

    });

    $('#Viewbtn').click(function () {

        var form1 = document.createElement("form");

        var element1 = document.createElement("input");

        form1.method = "POST";

        form1.action = "/purchaseorder/details/";

        element1.value = pon;

        element1.name = "poNum";

        element1.type = "hidden";

        form1.appendChild(element1);

        document.body.appendChild(form1);

        form1.submit();

    });
});
