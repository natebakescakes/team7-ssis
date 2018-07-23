$(document).ready(function () {
    $('#mySATable').DataTable({

        sAjaxSource: "/api/stockadjustment/all",
        sAjaxDataProp: "",
        columns: [
            { "data": "StockAdjustmentId", "autoWidth": true },
            { "data": "CreatedBy", "autoWidth": true },
            { "data": "ApprovedBySupervisor", "autoWidth": true },
            { "data": "CreatedDateTime", "autoWidth": true },
            { "data": "StatusName", "autoWidth": true },

        ],
        createdRow: function (row, data, dataIndex) {
            if (data.Status == "Delivered") {
                $('td', row).eq(4).addClass('delivered');
            }
            if (data.Status == "Partially Delivered") {
                $('td', row).eq(4).addClass('partially-delivered');
            }
            if (data.Status == "Awaiting Delivery") {
                $('td', row).eq(4).addClass('awaiting-delivery');
            }
        },
    });
    //Apply Customer Search on jquery Datatables here
//    var oTable = $('#mySATable').dataTable();
//    $('#btnSearch').click(function () {
//        oTable.columns[4].search($('#addStatus').val().trim());
//    oTable.draw();
//});
});










