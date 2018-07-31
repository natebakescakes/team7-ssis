$(document).ready(function () {
    var oTable = $('#myTable').DataTable({
        ajax: {
            url: "/api/reqdetail/all",
            dataSrc: ""
        },
        order: [[0, 'desc']],
        columns: [
            { "data": "Requisition", "autoWidth": true },
            { "data": "ItemCode", "autoWidth": true },
            { "data": "Description", "autoWidth": true },
            { "data": "Quantity", "autoWidth": true },
            { "data": "Status", "autoWidth": true },
        ],
        select: 'os'
    });
})

// Click "Process All Requisitions" button
$('#processAllRequisitionButton').click(function (e) {
    e.preventDefault();
    var data = $('#myTable').DataTable().rows().data().toArray();
    process(data);
})

// Click "Process All Requisitions" button
$('#processSelectedButton').click(function (e) {
    e.preventDefault();
    var data = $('#myTable').DataTable().rows({ selected: true }).data().toArray();
    process(data);
})

function process(data) {
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
}