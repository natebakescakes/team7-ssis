$(document).ready(function () {
    var oTable = $('#myTable').DataTable({
        ajax: {
            url: "/api/reqdetail/all",
            dataSrc: ""
        },
        columns: [
            { "data": "ItemCode", "autoWidth": true },
            { "data": "Description", "autoWidth": true },
            { "data": "Quantity", "autoWidth": true },
            { "data": "Status", "autoWidth": true },
        ],
        select: "api"
    });
})
