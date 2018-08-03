$(document).ready(function () {

    var table = $('#myTable').DataTable({

        sAjaxSource: "/api/itemcategory/all",
        sAjaxDataProp: "",  
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

    var single = $('#myTable tbody').on('click', 'tr', function () {
        var rowdata = table.row(this).data();
        var xid = rowdata.ItemCategoryId;
        $('.collapse').collapse("show");
        $.ajax({
            type: 'GET',
            url: '/api/itemcategoryapi/',
            dataType: 'json',
            data: { id: xid },
            success: function (data) {
                var i;
                for (i in data) {
                    $('#itemcategorydetails').find('[id="' + i + '"]').val(data[i]);
                }
                document.getElementById('ItemCategoryId').innerText = xid;
            }
        });

        $('#edit-btn').show();
        $(".button-set").hide();
        disableInput();



    });

    $(".additemcategory-form").submit(function (event) {

        if ($('.additemcategory-form').find('#Name').val() === '') {
            alert("Item Category Name cannot be empty. Please enter an Item Category Name.");
            event.preventDefault();
        }
            if ($('.additemcategory-form').find('#Name').val() !== '') {
                $.ajax({
                    type: "POST",
                    url: '/itemcategory/Save',
                    data: $('.additemcategory-form').serialize(),
                    success: function (data) {
                        if (data.status) {
                            $('#myModal').modal('hide');
                            table.ajax.reload();                         
                            
                        }
                        
                    }
                    
                });
     
        }
        event.preventDefault();
    });

    $('#itemcategorydetails').submit(function (event) {
        $.ajax({
            type: "POST",
            url: '/itemcategory/Save',
            data: $('#itemcategorydetails').serialize(),
            success: function (data) {
                if (data.status) {
                    alert("Item Category information has been successfully updated");
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
        $('#itemcategorydetails')
            .find('input')
            .prop('disabled', false);
  
        $('#itemcategorydetails')
            .find('select')
            .prop('disabled', false);
      
    }

    function disableInput() {
        $('#itemcategorydetails')
            .find('input')
            .prop('disabled', true);
    
        $('#itemcategorydetails')
            .find('select')
            .prop('disabled', true);
    }

});