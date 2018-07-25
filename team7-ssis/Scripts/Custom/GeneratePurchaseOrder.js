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
        dom: "tp"
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

    $('#addToPOBtn').click(function () {
        var data = $('#generateAddItem').DataTable().rows({ selected: true }).data().toArray();
        var itemNo = data[0].ItemCode;

        //$.ajax({

        //    type: "POST",
        //    url: "/PurchaseOrder/getitemsupplier",
        //    dataType: "json",
        //    data: JSON.stringify({ itemCode: itemNo }),
        //    contentType: "application/json",
        //    cache: true,
        //    success: function (data) {

        //        if (data.Count() != null) {
        //            alert("Success");
        //        }
        //    }
        //});

        
        var qty = parseInt($('#generateQty').val());
        var amount = parseInt($('#generateAmount').val());
        generatePOTbl.row.add([data[0].ItemCode, data[0].Description, '<input id="editTextBox" type="text" value="' + qty + '" />', data[0].UnitPrice, '<select id="supervisorDropBox"><option>HELLO</option></select>', amount]).draw();
        document.getElementById("generateAmount").value = 0;
        document.getElementById("generateUnitPrice").value = 0;
        document.getElementById("generateQty").value = 0;
        $('#generateModal').modal('hide');
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
        
    });

    $("#generateQty").change(function () {
        
        document.getElementById("generateAmount").value = document.getElementById("generateUnitPrice").value * document.getElementById("generateQty").value;
    });

    $("#generateUnitPrice").change(function () {
        
        document.getElementById("generateAmount").value = document.getElementById("generateUnitPrice").value * document.getElementById("generateQty").value;
    });

});
