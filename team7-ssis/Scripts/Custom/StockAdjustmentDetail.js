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
            "data": "Reason"
        },
        {
            "data": "UnitPrice"
        },
        {
            "data": "Adjustment",
            "render": function (data, type, row, meta) {
                    var html = '<input class="actual" type="number" id="height" name="actual"' +
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
        "columnDefs":[
            {
                "targets": [2],
                "data": "Reason",
                "render": function (data, type, full) {
                    return "Update"
                }
            }

        ],
           autowidth: true,
             select: 'api'

    });

    $(document).on("blur", ".actual", function () {
        // grab the cell that the td refers to, which is the parent of the <input> element
        var cell = table.cell(this.parentElement);
        // assign the cell with the value from the <input> element
        cell.data($(this).val()).draw();
    });
                });