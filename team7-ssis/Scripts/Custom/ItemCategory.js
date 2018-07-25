$(document).ready(function () {

    var table = $('#myTable').DataTable({

        sAjaxSource: "/api/itemcategory/all",
        sAjaxDataProp: "",
        pageLength: '5',
        columns: [
            { "data": "ItemCategoryId", "autoWidth": true },
            { "data": "Name", "autoWidth": true },
            { "data": "Description", "autoWidth": true },
            { "data": "StatusName", "autoWidth": true }
            
        ],
        select: {
            style: 'single'
        }

    });

});