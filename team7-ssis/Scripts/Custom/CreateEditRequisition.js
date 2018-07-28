$(document).ready(function () {

    // render main table
    var myTable = $('#myTable').DataTable({
        columns: [
            { title: "ItemCode" },
            { title: "Description" },
            { title: "Quantity" },
            { defaultContent: '<i class="fa fa-times pointer" aria-hidden="true"></i>' }
        ],
        select: "api"
    });

    // render Item table
    var addItemTable = $('#addItemTable').DataTable({
        ajax: {
            url: "/api/manage/items",
            dataSrc: ""
        },
        pageLength: 5,
        columns: [
            { "defaultContent": '<div class="thumbnail__small"></div>' },
            { "data": "ItemCode", "autoWidth": true },
            { "data": "ItemCategoryName", "autoWidth": true },
            { "data": "Description", "autoWidth": true },
            { "data": "Quantity", "autoWidth": true },
            { "data": "Uom", "autoWidth": true },
        ],
        select: "single"
    });

    // Display the Add Item modal
    $('#addItemBtn').click(function () {
        $('#myModal').modal({
            backdrop: 'static',
        });
    })

    // Add items to myTable
    $('#addToReq').click(function () {
        var data = $('#addItemTable').DataTable().rows({ selected: true }).data().toArray();
        //console.log(data);
        //TODO: write some logic that will add quantity to the requisitionDetail
        var qty = parseInt($('#qtyInput').val());

        if ((data.length > 0) && (qty > 0)) {
            myTable.row.add([data[0].ItemCode, data[0].Description, qty]).draw();
            $('#myModal').modal('hide');
            $('#modalMsg').html('');
        } else {
            $('#modalMsg').html('You must select an item and a valid quantity.');
            console.log("Error");
        }
        
    })

    // Delete row
    $('#myTable tbody').on('click', 'i.fa.fa-times', function () {
        myTable
            .row($(this).parents('tr'))
            .remove()
            .draw();
    });

    // Click 'Confirm' button
    $('#confirmBtn').click(function () {
        var tableData = myTable.rows().data().toArray();

        if (tableData.length > 0) {
            var data = [];

            for (i = 0; i < tableData.length; i++) {
                data[0] = { ItemCode: tableData[i][0], Qty: tableData[i][2] }
            }

            $.ajax({
                type: "POST",
                contentType: 'application/json',
                url: "/api/createrequisition",
                data: JSON.stringify(data),
                dataType: "json",
                success: function (rid) {
                    // redirect to ManageRetrieval with a msg
                    window.location.replace("/Requisition/ManageRequisitions?rid=" + encodeURIComponent(rid));
                },
                error: function (msg) {
                    //console.log(msg.responseJSON.Message);
                    $('#errorAlert').removeAttr('hidden').html("An unexpected error occured.");
                }
            });
        } else {
            $('#errorAlert').removeAttr('hidden').html("Please add some items to your Requisition.");
        }

    })


})