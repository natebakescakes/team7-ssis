$(document).ready(function () {

    var table = $('#myTable').DataTable({
        select: true,
        sAjaxSource: "/api/supplier/all",
        sAjaxDataProp: "",
        pageLength: '5',
        columns: [
            { "data": "SupplierCode", "autoWidth": true },
            { "data": "Name", "autoWidth": true },
            { "data": "ContactName", "autoWidth": true },
            { "data": "PhoneNumber", "autoWidth": true },
            { "data": "FaxNumber", "autoWidth": true },
            { "data": "Address", "autoWidth": true },


        ],
        select: {
            style: 'single'
        }


    });

    table.on('deselect', function (e, dt, type, indexes) {

        $('.collapse').collapse("hide");
    });
    table.on('select', function (e, dt, type, indexes) {

        $('.collapse').collapse("show");
    });

    $('#myTable tbody').on('click', 'tr', function () {
        var rowdata = table.row(this).data();
        var xid = rowdata.SupplierCode;

        $.ajax({
            type: 'GET',
            url: '/api/supplierapi/',
            dataType: 'json',
            data: { id: xid },
            success: function (data) {
                var i;
                for (i in data) {
                    $('#supplier-form').find('[id="' + i + '"]').val(data[i]);
                }
            }
        });

    });

    $(".addsupplier-form").submit(function (event) {
        $.ajax({
            type: "POST",
            url: '/supplier/Save',
            data: $('.addsupplier-form').serialize(),
            success: function (data) {
                if (data.status) {
                    $('#myModal').modal('hide');
                    table.ajax.reload();
                }
            }
        })

        event.preventDefault();
    });



})
