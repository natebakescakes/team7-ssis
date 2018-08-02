$(document).ready(function () {


    var oTable = $('#dptTable').DataTable({
        ajax: {
            url: "/api/department/all",
            dataSrc: ""
        },
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

    var delegationTable = $('#delgTable').DataTable({
        ajax: {
            url: "/api/delegation/all",
            dataSrc: ""
        },
        columns: [
            { "data": "DelegationId", "autowidth"   : true},
            { "data": "Recipient", "autowidth": true },
            { "data": "StartDate", "autowidth": true },
            { "data": "EndDate", "autowidth": true },
            { 
                defaultContent: "<button class='btn btn-danger' id='disable-btn' style='font-size: 12px'><i class='fa fa-edit'></i></button>"     
            }
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

    $('#departmentoptions').submit(function (event) {

        $.ajax({
            type: "POST",
            url: '/department/SaveOptions',
            data: $('#departmentoptions').serialize(),
            success: function (data) {
                if (data.status==0) {  
                    alert("Department information did not update!"); 
                    delegationTable.ajax.reload();
                }
                if ((data.status == 1) || (data.status==2)){
                    alert("Department information has been updated!");
                    delegationTable.ajax.reload();
                }
                if (data.status == 3) {
                    alert("Please choose dates for delegating manager! ");
                }
            }
        });
        event.preventDefault();
    });
    
    $('#delgTable tbody').on('click', '#disable-btn', function (event) {
        var code = delegationTable.row($(this).parents('tr')).data().DelegationId;
        var Dstatus = "2";

        var sendInfo = {
            DelegationId : code,
            DelegationStatus : Dstatus
        };
      
        $.ajax({
            type: "POST",
            url: '/department/SaveStatus',
            data : sendInfo,
            success: function (data) {
                if (data.status) {
                    alert("Delegated Manager has been cancelled");
                    delegationTable.ajax.reload();
                }
            }
        });
    })

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
    $("#Startdate").datepicker({
        format: 'dd/mm/yyyy',
        autoclose: true,
        startDate: "0d"
    }).on('changeDate', function (selected) {
        var startDate = new Date(selected.date.valueOf());
        $('#Enddate').datepicker('setStartDate', startDate);
    }).on('clearDate', function (selected) {
        $('#Enddate').datepicker('setStartDate', null);
    });
 
    $("#Enddate").datepicker({
        format: 'dd/mm/yyyy',
        autoclose: true
    }).on('changeDate', function (selected) {
        var endDate = new Date(selected.date.valueOf());
        $('#Startdate').datepicker('setEndDate', endDate);
    }).on('clearDate', function (selected) {
        $('#Startdate').datepicker('setEndDate', null);
    });
   

});