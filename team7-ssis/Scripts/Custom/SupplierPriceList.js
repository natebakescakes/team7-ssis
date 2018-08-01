
$(document).ready(function () {
    var url = window.location.pathname;
    var Sid = url.substring(url.lastIndexOf('/') + 1);
    var api = '/api/supplier/pricelist/' + Sid;
    var table = $('#myTable').DataTable({
        
        ajax: {
            url: api,
            dataSrc:''
        },
        deferRender: true,
        columns: [
            { "data": "ItemCode", "autoWidth": true },
            { "data": "ItemCategoryName", "autoWidth": true },
            { "data": "Description", "autoWidth": true },
            { "data": "Uom", "autoWidth": true },
            {
                "data": "Price", "autoWidth": true,
                render: function (data, type, row) {
                    return '<input class="form-control"  style="border:none" id=Price" type="number" disabled  min="0.01" step="0.01" max="2500" value = ' + row.Price + '  >';
                }
            },
            {
                defaultContent: "<button class='btn btn-danger' id='edit' style='font-size: 12px'><i class='fa fa-edit'></i></button>" 
                
            }
            
        ],


    });

    var rowedit = $('#myTable tbody').on('click', '#edit', function (e) {

        var rowdata = table.row($(this).parents('tr')).data();
        var counter = 0;
        $($(this).parents('tr')).find("td").each(function () {
            counter++;

            if ($(this).children().hasClass("btn-danger")) {
                $(this).html("<button id='save-btn' class='btn btn-success' style='font-size: 12px'><i class='fa fa-save'></i></button>");
            }
        
            if ($(this).children().hasClass("form-control")) {
                ($(this).find("input")).prop("disabled", false);
            }

        })
        
    });

    var rowsave = $('#myTable tbody').on('click', '#save-btn', function (e) {
       
        var code = table.row($(this).parents('tr')).data().ItemCode;
        var counter = 0;
        var price = 0;
        $($(this).parents('tr')).find("td").each(function () {
            counter++;

            if ($(this).children().hasClass("btn-success")) {
                $(this).html("<button class='btn btn-danger' id='edit' style='font-size: 12px'><i class='fa fa-edit'></i></button>");
            }

            if ($(this).children().hasClass("form-control")) {  
                price = ($(this).find("input")).val();
                ($(this).find("input")).prop("disabled", true);
            }

        })
 
        var sendInfo = {
            ItemCode:code,
            SupplierCode: Sid,
            Price: price
        };

      
        $.ajax({
            type: "POST",
            url: '/supplier/updateitemprice',
            data: sendInfo, 
            success: function (data) {
                if (data.status) {
                    alert("Item Price information has been successfully updated");
                    table.ajax.reload();
                }
            }
        });
       
        e.preventDefault();
  
    });

});