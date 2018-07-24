$(document).ready(function () {
    var url = window.location.pathname;
    var id = url.substring(url.lastIndexOf('/') + 1);
    var api = '/api/supplier/pricelist/' + id;
    var table = $('#myTable').DataTable({
        
        sAjaxSource: api,
        sAjaxDataProp: "",
        
        columns: [
            { "data": "ItemCode", "autoWidth": true },
            { "data": "ItemCategoryName", "autoWidth": true },
            { "data": "Description", "autoWidth": true },
            { "data": "Uom", "autoWidth": true },
            { "data": "Price", "autoWidth": true },
            
        ],
        select: {
            style: 'single'
        }

    });

});