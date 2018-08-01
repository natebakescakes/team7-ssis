$(document).ready(function () {

    var inventory = $("#inventory").val();
    //Generate purchase order datatable

    var simple_dropdown = function (data, type, row, meta) {
        //alert(data);
        if (data != null) {
            // $('td:eq(4)', row).html('<select class="supplier" id=supplier' + data.ItemCode + '></select>');
            var html = '<select class="supplier" id="supplier' + data + '">';

            var itemNo = data;
            var id = "supplier" + itemNo;

            $.ajax({

                type: "POST",
                url: "/api/purchaseOrder/getsupplier",
                dataType: "json",
                data: { '': itemNo },
                contentType: 'application/x-www-form-urlencoded',
                async: false,
                success: function (result) {

                    for (var supplier in result) {
                        if (result.hasOwnProperty(supplier)) {
                            var s = result[supplier];
                            if (s.Priority == 1) {
                                html += '<option value="' + s.Priority + '" selected="selected">' + s.Name + '</option>';
                            }
                            else {
                                html += '<option value="' + s.Priority + '">' + s.Name + '</option>';
                            }
                            //var x = document.getElementById(id);
                            //var option = document.createElement("option");
                            //option.text = s.Name;
                            //option.value = s.Priority;
                            //x.add(option);
                            // document.getElementById(id).value = 1;

                        }
                    }

                    html += '</select>';
                    
                }

            });
            return html;
        }
    }

    var generatePOTbl = $('#generatePoTable').DataTable({
        ajax: {
            type: "POST",
            url: "/api/inventory/items",
            dataType: "json",
            data: { '': inventory},
            contentType: 'application/x-www-form-urlencoded',
            cache: false,
            dataSrc: ""

        },
        columns: [
            {
                title: "Item Code",
                data: "ItemCode"
            },
            {
                title: "Description",
                data: "Description"
            },
            {
                title: "Quantity",
                data: "Quantity",
                render: function (data, type, row, meta) {
                    //alert(row.ItemCode);
                    var html = '<input class="edit" id="' + row.ItemCode + '"type="number" ' +
                        'value="' + data + '"/>';
                    return html;
                } 
            },
            {
                title: "Unit Price",
                data: "UnitPriceDecimal",
                render: $.fn.dataTable.render.number(',', '.', 2, '$')
            },
            {
                
                data: "ItemCategoryName",
                render: simple_dropdown
            },
            {
                title: "Amount",
                data: "TotalPrice", 
                render: $.fn.dataTable.render.number(',', '.', 2, '$')
            },
            { defaultContent: '<i class="fa fa-times pointer" aria-hidden="true"></i>' }
        ],
        select: "api",
        dom: "tp",
        autoWidth: true,
        cache:true

       

    });

   


    //Add item popup datatable
    var addItemTable = $('#generateAddItem').DataTable({
        ajax: {
            url: "/api/manage/items",
            dataSrc: ""
        },
        pageLength: 5,
        columns: [
            {
                data:"ImagePath",
                render: function (data,type,row,meta) {
                    var html = '<img class="img-thumbnail img-responsive myImage" alt="Cinque Terre"" src="/Images/'+data+'.jpg" />';
                    return html;
                }
            },
            { data: "ItemCode" },
            { data: "ItemCategoryName"},
            { data: "Description"},
            { data: "Quantity" },
            { data: "ReorderLevel"},
            { data: "ReorderQuantity"},
            { data: "Uom" },
            {
                data: "UnitPrice",
                render: $.fn.dataTable.render.number(',', '.', 2, '$')}
        ],
        select: "single",
        dom: "ftp",
        pageLength:4
    });


    //Button to open up Add item popup
    $('#addItemButton').click(function () {

        document.getElementById("generateAmount").value = 0;
        document.getElementById("generateUnitPrice").value = 0;
        document.getElementById("generateQty").value = 0;

        $('#generateModal').modal({
            backdrop: 'static',
        });
    });


    
    // Adds item to purchase order
    $('#addToPOBtn').click(function () {
            var data = $('#generateAddItem').DataTable().rows({ selected: true }).data().toArray();
            
            var qty = parseInt($('#generateQty').val());
            
        if ((data.length > 0) && (qty > 0)) {
            var itemNo = data[0].ItemCode;
            var amount = parseInt($('#generateAmount').val());
            var id = "supplier" + data[0].ItemCode; //i.toString();
            generatePOTbl.row.add(
                {
                    "ItemCode": data[0].ItemCode,
                    "Description": data[0].Description,
                    "Quantity": qty,
                    "UnitPriceDecimal": data[0].UnitPrice,
                    "ItemCategoryName": data[0].ItemCode,
                    "TotalPrice": amount
                }).draw();
            document.getElementById("generateAmount").value = 0;
            document.getElementById("generateUnitPrice").value = 0;
            document.getElementById("generateQty").value = 0;
            $('#generateModal').modal('hide');
            $('#modalMsg').html('');
            
        }
        else {
            $('#modalMsg').html('You must select an item and a valid quantity.');
            console.log("Error");
        }

       // }
        
    })

    //cancel purchase order detail
    $('#generatePoTable tbody').on('click', 'i.fa.fa-times', function () {
        generatePOTbl
            .row($(this).parents('tr'))
            .remove()
            .draw();
    });

    

    //show item details on click of each row in add items popup page
    $('#generateAddItem tbody').on('click', 'tr', function () {

        
       // document.getElementById("imageAddItem").src = $($("#generateAddItem").DataTable().row(this)).find("img").attr('src');
        document.getElementById("imageAddItem").src = $(this).find("img").attr("src");

        var unitPrice = addItemTable.row(this).data().UnitPrice;
        
        var data = addItemTable.row(this).data();
        
        document.getElementById("generateUnitPrice").value = unitPrice;
        document.getElementById("generateQty").value = 0;
        document.getElementById("generateAmount").value = 0;
        
        document.getElementById("itemnumber").innerHTML = data.ItemCode;
        document.getElementById("instock").innerHTML = data.Quantity;
        document.getElementById("description").innerHTML = data.Description;
        document.getElementById("reorderlevel").innerHTML = data.ReorderLevel;
        document.getElementById("reorderquantity").innerHTML = data.ReorderQuantity;         
        document.getElementById("category").innerHTML = data.ItemCategoryName;
        document.getElementById("uom").innerHTML = data.Uom;
            
            });

    $("#generateQty").change(function () {
        
        document.getElementById("generateAmount").value = document.getElementById("generateUnitPrice").value * document.getElementById("generateQty").value;
    });

    $("#generateUnitPrice").change(function () {
        
        document.getElementById("generateAmount").value = document.getElementById("generateUnitPrice").value * document.getElementById("generateQty").value;
    });

    

    $(document).on("change", ".edit", function () {

        var cell = generatePOTbl.cell(this.parentElement);
        // assign the cell with the value from the <input> element 
        cell.data($(this).val()).draw();

        //alert(this.id);
        var rowIdx = document.getElementById(this.id).parentElement.parentElement;
        
        //works in console
       // alert($('#generatePoTable').DataTable().rows().data()[0].TotalPrice);
        //var quantity = parseInt(cell.data());

        //alert(row(generatePOTbl.this.parentElement));

        var quantity = document.getElementById(this.id).value;
        var amount = $('#generatePoTable').DataTable().row(document.getElementById(this.id).parentElement.parentElement).data().UnitPriceDecimal;

        

       // alert("AMOUNT "+generatePOTbl.row($(this.id).closest('tr')).data().UnitPriceDecimal);
       // var amount = parseFloat(generatePOTbl.row(this.parentElement).data().UnitPriceDecimal);

        var totalAmount = parseInt(quantity) * parseFloat(amount);
        generatePOTbl.cell(rowIdx, 5).data(totalAmount).draw();
       


        
            //.draw();
       // document.getElementById("")

        
    });



    //SAVE
    $("#generateforms").click(function () {

        var datatableData = generatePOTbl.rows().data().toArray();

        if (datatableData.length > 0) {

            var details = new Array();

            for (var i = 0; i < datatableData.length; i++) {

                var id = "#supplier" + datatableData[i].ItemCode; // (i + 1).toString();
               // alert(id);
                //alert(datatableData[i].ItemCode);

                var o = {
                    "ItemCode": datatableData[i].ItemCode,
                    "Description": datatableData[i].Description,
                    "QuantityOrdered": datatableData[i].Quantity,
                    "UnitPrice": datatableData[i].UnitPriceDecimal,
                    "SupplierName": $(id).children("option").filter(":selected").text(),
                    "SupplierPriority": $(id).children("option").filter(":selected").val(),
                    "Amount": datatableData[i].TotalPrice
                };
                //alert(JSON.stringify(o));
                details.push(o);
            }

            $.ajax({

                type: "POST",
                url: "/purchaseOrder/save",
                dataType: "json",
                data: JSON.stringify(details),
                contentType: "application/json",
                cache: true,
                success: function(result) {

                    alert("IN SUCCESS FUNCTION OF AJAX CALL TO POST THE PO DETAILS TO CONTROLLER TO SAVE    " + result.purchaseOrders);

                    url = $("#successUrl").val();
                    alert(url);

                    var form = document.createElement("form");
                    var element1 = document.createElement("input");
                    form.method = "POST";
                    form.action = url;

                    element1.value = result.purchaseOrders;
                    element1.name = "purchaseOrderIds";
                    element1.type = "hidden";
                    form.appendChild(element1);

                    document.body.appendChild(form);

                    form.submit();

                },
                error: function (msg) {
                    //console.log(msg.responseJSON.Message);
                    $('#errorAlert').removeAttr('hidden').html("An unexpected error occured.");
                }

            });

        }
        else {
            $('#errorAlert').removeAttr('hidden').html("Please add some items to your Purchase Order!.");
        }
       
        
    });
        

    
    var PONums = $("#PONums").val();

    var successPOTable = $('#successPoTable').DataTable({

        ajax: {
            type: "POST",
            url: "/api/purchaseOrder/success",
            dataType: "json",
            data: { '': PONums },
            contentType: 'application/x-www-form-urlencoded',
            cache: false,
            dataSrc: ""
        },
        columns: [
            { data: "PurchaseOrderNo" },
            { data: "SupplierName" },
            { defaultContent: '<button id="infobutton" class="btn btn-default mb-3"><i class="fa fa-info-circle" aria-hidden="true"></i></button>' }
        ],
        dom: "t",
        select: "single"
    });

    $(document).on("click", "#infobutton", function (e) {
        //alert("hi");
        var purchaseOrder = successPOTable.row($(this).parents('tr')).data().PurchaseOrderNo;
        url = $("#detailsUrl").val();

        var form = document.createElement("form");
        var element1 = document.createElement("input");
        form.method = "POST";
        form.action = url;

        element1.value = purchaseOrder;
        element1.name = "poNum";
        element1.type = "hidden";
        form.appendChild(element1);

        document.body.appendChild(form);

        form.submit();
        

    });

    $('#successback').on('click', function (e) {
        document.location.href = '/PurchaseOrder';
    });


    $(document).on("click", "#downloadselectedsuccess", function () {

        var data = $('#successPoTable').DataTable().rows({ selected: true }).data().toArray();
     
        ponum = data[0].PurchaseOrderNo;

        var form = document.createElement("form");
        var element1 = document.createElement("input");
        form.method = "POST";
        form.action = "/PurchaseOrder/downloadselectedpdf";

        element1.value = ponum;
        element1.name = "purchaseOrderNum";
        element1.type = "hidden";
        form.appendChild(element1);

        document.body.appendChild(form);

        form.submit();


    });

    $(document).on("click", "#ViewPrintButton", function () {

        var data = $('#successPoTable').DataTable().rows({ selected: true }).data().toArray();

        ponum = data[0].PurchaseOrderNo;

        var form = document.createElement("form");
        var element1 = document.createElement("input");
        form.method = "POST";
        form.action = "/PurchaseOrder/viewselectedpdf";

        element1.value = ponum;
        element1.name = "purchaseOrderNum";
        element1.type = "hidden";
        form.appendChild(element1);

        document.body.appendChild(form);

        form.submit();

    });

    $(document).on('change', '.supplier', function (e) {

        alert("HELLO");
        var rowIdx = $(this).parents('tr');
        var itemCode = generatePOTbl.row($(this).parents('tr')).data().ItemCode;
        var id = "supplier" + itemCode;
        var priority = document.getElementById(id).value;

        $.ajax({

            type: "POST",
            url: "/PurchaseOrder/getitemprice",
            dataType: "json",
            data: JSON.stringify({ itemCode: itemCode , priority: priority}),
            contentType: "application/json",
            cache: true,
            success: function (result) {
                alert(result.itemPrice);
                generatePOTbl.cell(rowIdx, 3).data(result.itemPrice).draw();
                var quantity=$('#generatePoTable').DataTable().row(document.getElementById(id).parentElement.parentElement).data().Quantity;
               // alert(generatePOTbl.row($(this).parentElement.parentElement).data().Quantity);

                var newAmount = parseInt(quantity) * result.itemPrice;
                generatePOTbl.cell(rowIdx, 5).data(newAmount).draw();

            }
        });

    });

    

});
