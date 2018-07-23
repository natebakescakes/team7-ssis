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

        ]
    });
    //Apply Customer Search on jquery Datatables here
//    var oTable = $('#mySATable').dataTable();
//    $('#btnSearch').click(function () {
//        oTable.columns[4].search($('#addStatus').val().trim());
//    oTable.draw();
//});
});










