$(document).ready(function () {
    var oTable = $('#myTable').DataTable({
        ajax: {
            url: "/api/reqdetail/all",
            dataSrc: ""
        },
        columns: [
            { "data": "Requisition", "autoWidth": true },
            { "data": "ItemCode", "autoWidth": true },
            { "data": "Description", "autoWidth": true },
            { "data": "Quantity", "autoWidth": true },
            { "data": "Status", "autoWidth": true },
        ],
        select: 'single'
    });
})

// See if DataTable selection works
//$('#myTable').click(function () {
//    var table = $('#myTable').DataTable();
//    console.log(table.rows({ selected: true }).data().toArray());
//})

$('#processAllRequisitionButton').click(function () {

    var data = $('#myTable').DataTable().rows({ selected: true }).data().toArray();
    var reqIdArray = [];
    for (i = 0; i < data.length; i++) {
        reqIdArray[i] = data[i].Requisition;
    }

    // call the appropriate controller with the data
    $.ajax({
        type: "POST",
        contentType: 'application/json',
        url: "/api/processrequisitions",
        data: JSON.stringify(reqIdArray),
        dataType: "json",
        success: function (rid) {
            message = reqIdArray.length + " Requisitions processed. Retrieval Form #" + rid + " created.";
            window.location.replace("/Requisition/StationeryRetrieval?rid=" + rid + "&msg=" + encodeURIComponent(message));
        }
    });

    
})