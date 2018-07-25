$(document).ready(function () {

    
    var oTable = $('#dptTable').DataTable({

        sAjaxSource: "api/department/all",
        sAjaxDataProp: "",

        columns: [
            { "data": "DepartmentCode", "autowidth": true },
            { "data": "DepartmentName", "autowidth": true },
            { "data": "DepartmentHead", "autowidth": true },
            { "data": "DepartmentRep", "autowidth": true },
            { "data": "CollectionPoint", "autowidth": true },
            { "data": "ContactName", "autowidth": true },
            { "data": "PhoneNumber", "autowidth": true },
            { "data": "FaxNumber", "autowidth": true }
        ],
        select: 'single'
    });


    var single = $('#dptTable tbody').on('click', 'tr', function () {
        var rowdata = oTable.row(this).data();
        var xid = rowdata.DepartmentCode;
        $('.collapse').collapse("show");
        $.ajax({
            type: 'GET',
            url: '/api/departmentapi/',
            dataType: 'json',
            data: { id: xid },
            success: function (data) {
                var i;
                for (i in data) {
                    $('#departmentdetails').find('[id="' + i + '"]').val(data[i]);
                }
                document.getElementById('DepartmentRepresentative').innerText = xid;
            }
            
        });

        $('#edit-btn').show();
        $(".button-set").hide();
        disableInput();


    });
    $(".adddepartment-form").submit(function (event) {
        $.ajax({
            type: "POST",
            url: '/department/Save',
            data: $('.adddepartment-form').serialize(),
            success: function (data) {
                if (data.status) {
                    $('#myModal').modal('hide');
                    oTable.ajax.reload();
                }
            }
        });

        event.preventDefault();
    });
    $('#departmentdetails').submit(function (event) {
        $.ajax({
            type: "POST",
            url: '/department/Save',
            data: $('#departmentdetails').serialize(),
            success: function (data) {
                if (data.status) {
                    alert("Department information has been successfully updated");
                    cancelbtn.click();
                    oTable.ajax.reload();
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
        $('#departmentdetails')
            .find('input')
            .prop('disabled', false);
        $('#departmentdetails')
            .find('textarea')
            .prop('disabled', false);
        $('#departmentdetails')
            .find('select')
            .prop('disabled', false);
    }

    function disableInput() {
        $('#departmentdetails')
            .find('input')
            .prop('disabled', true);
        $('#departmentdetails')
            .find('textarea')
            .prop('disabled', true);
        $('#departmentdetails')
            .find('select')
            .prop('disabled', true);
    }
})