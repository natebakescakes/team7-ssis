$(document).ready(function () {

    var table = $('#myTable').DataTable({
       
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


    var single = $('#myTable tbody').on('click', 'tr', function () {
        var rowdata = table.row(this).data();
        var xid = rowdata.SupplierCode;
        $('.collapse').collapse("show");
        $.ajax({
            type: 'GET',
            url: '/api/supplierapi/',
            dataType: 'json',
            data: { id: xid },
            success: function (data) {
                var i;
                for (i in data) {
                    $('#supplierdetails').find('[id="' + i + '"]').val(data[i]);
                }
            }
        });

        $('#edit-btn').show();
        $(".button-set").hide();
        disableInput();


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
        });

        event.preventDefault();
    });

    $('#pricelist-btn').on('click', function () {
        var code = $('#SupplierCode').val();
        $('#pricelist-btn').attr('href', '/supplier/supplierpricelist/' +code);
       

    });

    $('#supplierdetails').submit(function (event) {
        $.ajax({
            type: "POST",
            url: '/supplier/Save',
            data: $('#supplierdetails').serialize(),
            success: function (data) {
                if (data.status) {
                    alert("Supplier information has been successfully updated");
                    cancelbtn.click();
                    table.ajax.reload();
                }
            }
        });

        event.preventDefault();
    });



    $('#edit-btn').on('click', function () {
        $('#edit-btn').hide();
        $(".button-set").show();
        enableInput();

    });

    var cancelbtn = $('#cancel-btn').on('click', function () {
        disableInput();
        $('#edit-btn').show();
        $(".button-set").hide();
    });

    function enableInput() {
        $('#supplierdetails')
            .find('input')
            .prop('disabled', false);
        $('#supplierdetails')
            .find('textarea')
            .prop('disabled', false);
        $('#supplierdetails')
            .find('select')
            .prop('disabled', false);
    }

    function disableInput() {
        $('#supplierdetails')
            .find('input')
            .prop('disabled', true);
        $('#supplierdetails')
            .find('textarea')
            .prop('disabled', true);
        $('#supplierdetails')
            .find('select')
            .prop('disabled', true);
    }
});