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
            }
            
        });

        $('#edit-btn').show();
        $(".button-set").hide();
        disableInput();


    });
})