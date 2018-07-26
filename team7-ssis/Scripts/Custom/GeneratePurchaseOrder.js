$(document).ready(function () {
    var generatePOTbl = $('#generatePoTable').DataTable({
        columns: [
            { title: "ItemCode" },
            { title: "Description" },
            { title: "Quantity" },
            { title: "UnitPrice" },
            { title: "Supplier" },
            { title: "Amount" },
            { defaultContent: '<i class="fa fa-times pointer" aria-hidden="true"></i>' }
        ],
        select: "api",
        dom: "t",
        scrollY: "200px",
        scrollCollapse: true,
        autoWidth: true
       

    });

    var populate_dropbox = function (supplierList,id) {

        for (var supplier in supplierList) {
            if (supplierList.hasOwnProperty(supplier)) {
                var s = supplierList[supplier];
                var x = document.getElementById(id);
                var option = document.createElement("option");
                alert(s.Name);
                option.text = s.Name;
                alert(s.Priority);
                option.value = s.Priority;
                    x.add(option);
            }
        }

    }

    var addItemTable = $('#generateAddItem').DataTable({
        ajax: {
            url: "/api/manage/items",
            dataSrc: ""
        },
        pageLength: 5,
        columns: [
            { defaultContent: '<div class="thumbnail__small"></div>' },
            { data: "ItemCode" },
            { data: "ItemCategoryName"},
            { data: "Description"},
            { data: "Quantity" },
            { data: "ReorderLevel"},
            { data: "ReorderQuantity"},
            { data: "Uom" },
            { data: "UnitPrice"}
        ],
        select: "single",
        dom: "ftp"
    });

    $('#addItemButton').click(function () {

        document.getElementById("generateAmount").value = 0;
        document.getElementById("generateUnitPrice").value = 0;
        document.getElementById("generateQty").value = 0;

        $('#generateModal').modal({
            backdrop: 'static',
        });
    })

    //$('#saveAsBtn').click(function () {
    //    myTable.row.add([2, 3, 4]).draw();
    //})

    var i = 1;

    $('#addToPOBtn').click(function () {

        var data = $('#generateAddItem').DataTable().rows({ selected: true }).data().toArray();
        var itemNo = data[0].ItemCode;
        

        $.ajax({

            type: "POST",
            url: "/api/purchaseOrder/getsupplier",
            dataType: "json",
            data: { '': itemNo },
            contentType: 'application/x-www-form-urlencoded',
            cache: true,
            success: function (result) {
                
                var qty = parseInt($('#generateQty').val());
                var amount = parseInt($('#generateAmount').val());
                var id = "supervisor" + i.toString();
                generatePOTbl.row.add([data[0].ItemCode, data[0].Description, '<input id="edit'+id+'" class="edit" type="text" value="' + qty + '" />', data[0].UnitPrice, '<select id="'+id+'"></select>', amount]).draw();
                document.getElementById("generateAmount").value = 0;
                document.getElementById("generateUnitPrice").value = 0;
                document.getElementById("generateQty").value = 0;
                populate_dropbox(result, id);
                document.getElementById(id).value = 1;
                i++;
                $('#generateModal').modal('hide');

            }
        });

        
        
    })

    $('#generatePoTable tbody').on('click', 'i.fa.fa-times', function () {
        generatePOTbl
            .row($(this).parents('tr'))
            .remove()
            .draw();
    });

    

    $('#generateAddItem tbody').on('click', 'tr', function () {
        var unitPrice = addItemTable.row(this).data().UnitPrice;
        
        document.getElementById("generateUnitPrice").value = unitPrice;
        document.getElementById("generateQty").value = 0;
        document.getElementById("generateAmount").value = 0;

    });

    $("#generateQty").change(function () {
        
        document.getElementById("generateAmount").value = document.getElementById("generateUnitPrice").value * document.getElementById("generateQty").value;
    });

    $("#generateUnitPrice").change(function () {
        
        document.getElementById("generateAmount").value = document.getElementById("generateUnitPrice").value * document.getElementById("generateQty").value;
    });


    //SAVE
    $("#generateforms").click(function () {

        var datatableData = generatePOTbl.rows().data().toArray();
        alert(datatableData);
        alert(datatableData.length);
        var details = new Array();

        for (var i = 0; i < datatableData.length; i++) {

            alert(i);
            var id = "#supervisor" + (i + 1).toString();
            alert($(id).children("option").filter(":selected").text());

            var o = {
                "ItemCode": datatableData[i][0],
                "Description": datatableData[i][1],
                "Quantity": 15,
                "UnitPrice": datatableData[i][3],
                "Supplier": $(id).children("option").filter(":selected").text(),
                "Amount": datatableData[i][5]
            };  

            details.push(o);
        }

        $.ajax({

            type: "POST",
            url: "/purchaseOrder/save",
            dataType: "json",
            data: JSON.stringify(details),
            contentType: "application/json",
            cache: true,
            success: function (result) {

            }
            
        });

        
    });
        
        //alert(datatableData[0][2]);
        //html = $.parseHTML(datatableData[0][2]);
        //alert(JSON.stringify(html));

        //generatePOTbl.rows().each(function () {
        //    var data1 = this.data().toArray();
        //    alert(data1.length);
            
    //    });
    //});

        //foreach(DataRow row in generatePOTbl.Rows){
        //    if ($(this).children().hasClass("supervisor1")) {
        //        alert("insidxe");
        //        var hell = generatePOTbl.cell($(this)).data().draw();
        //        alert(hell);
        //    }

        //    else {
        //        alert("Hello");
        //}
        //$.each(generatePOTbl.Rows, function (i) {
           
        //    }

        //});
        

        //var data = generatePOTbl.rows().data(text).toArray();

        //alert(data[0][2]);


    

    var successPOTable = $('#successPoTable').DataTable({
        ajax: {
            type: "POST",
            url: "/api/purchaseOrder/success",
            dataType: "json",
            data: { '': PONo },
            contentType: 'application/x-www-form-urlencoded',
            cache: true
        },
        pageLength: 5,
        columns: [
            { data: "PNo" },
            { data: "SupplierName" },
            { defaultContent: '<button id="infobutton" class="btn btn-default mb-3"><i class="fa fa-info-circle" aria-hidden="true"></i></button>' }
        ],
        dom: "t"
    });




    

});
