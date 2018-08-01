$(document).ready(function () {

    var oTable = $('#myOutstandingTable').DataTable({

        ajax: {
            url: "outstandingitems",

            dataSrc: "",
        },

        columns: [

            { data: "Item Code" },

            { data: "Description" },

            { data: "Outstanding Qty" },

        ]
    });
}