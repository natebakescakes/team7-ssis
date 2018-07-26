$(document).ready(function () {
    var saUrl = $("#StockAdjustmentId").val();

    var table = $('#myTable').DataTable({

            ajax: {
                url: "/api/stockadjustment/detail/"+saUrl,
                dataSrc: ""
            },
            
    columns: [
        {
            "data": "ItemCode"
        },
        {
            "data": "Description"
        },
        {
            "data": "Reason",
            "render": function (data, type, row, meta) {
                var html = '<input class="actual_1" type="textbox" ' +
                    'value="' + data + '" min="0"/>';
                return html;
            }
        },
        {
            "data": "UnitPrice"
        },
        {
            "data": "Adjustment",
            "render": function (data, type, row, meta) {
                    var html = '<input class="actual" type="number" ' +
                        'value="' + data + '" min="0"/>';
                    return html;
                }
            }        
        ],
        createdRow: function (row, data, index) {
            if (data['UnitPrice'] < 250) {//操作次数大于1000的变红显示
                $('td', row).eq(3).css('font-weight', "bold").css("color", "green");
            }
        },
           autowidth: true,
             select: 'api'

    });

    $(document).on("blur", ".actual", function () {
        // grab the cell that the td refers to, which is the parent of the <input> element
        var cell = table.cell(this.parentElement);
        // assign the cell with the value from the <input> element
        cell.data($(this).val()).draw();
    });
    $(document).on("blur", ".actual_1", function () {
        // grab the cell that the td refers to, which is the parent of the <input> element
        var cell = table.cell(this.parentElement);
        // assign the cell with the value from the <input> element
        cell.data($(this).val()).draw();
    });


    $('#cancelBtn').click(function () {
        //direct to home page
        window.location.href = "/StockAdjustment";
    });

    $('#saveBtn').click(function () {
        var data = $('#myTable').dataTable().fnGetData();
        var reqIdArray = [];
        for (i = 0; i < data.length; i++) {
            var obj = { StockAdjustmentId:saUrl,ItemCode:data[i][0], Reason: data[i][3], Adjustment: data[i][4] };
            reqIdArray[i] = obj;

        }
        alert(data);
        $.ajax({
            url: '/api/stockadjustment/update',
            contentType: 'application/json',
            data: JSON.stringify(reqIdArray),
            dataType: "json",
            type: "POST",
            traditional: true,
            success: function (responseJSON) {

                window.location.replace("/StockAdjustment");
            }
        });
    });
});