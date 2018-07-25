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
        select: "api"
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
        select: "single"
    });

    $('#addItemButton').click(function () {
        $('#generateModal').modal({
            backdrop: 'static',
        });
    })

    //$('#saveAsBtn').click(function () {
    //    myTable.row.add([2, 3, 4]).draw();
    //})

    $('#addToPOBtn').click(function () {
        var data = $('#generateAddItem').DataTable().rows({ selected: true }).data().toArray();
        
        var qty = parseInt($('#generateQty').val());
        var amount = parseInt($('#generateAmount').val());
        generatePOTbl.row.add([data[0].ItemCode, data[0].Description, '<input type="text" value="'+qty+'"/>',data[0].UnitPrice,'<select><option>HELLO</option></select>',amount]).draw();
        $('#generateModal').modal('hide');
    })

    $('#generatePoTable tbody').on('click', 'i.fa.fa-times', function () {
        generatePOTbl
            .row($(this).parents('tr'))
            .remove()
            .draw();
    });
});
