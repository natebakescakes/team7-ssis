$(document).ready(function () {
    $('#mySATable').DataTable({

        sAjaxSource: "/api/manageitem/selectitems",
        sAjaxDataProp: "",
        columns: [
            { "data": "ItemCode", "autoWidth": true },
            { "data": "Description", "autoWidth": true },
            { "data": "UnitPrice", "autoWidth": true },

        ]
    });
    //Apply Customer Search on jquery Datatables here
    //    var oTable = $('#mySATable').dataTable();
    //    $('#btnSearch').click(function () {
    //        oTable.columns[4].search($('#addStatus').val().trim());
    //    oTable.draw();
    //});



});