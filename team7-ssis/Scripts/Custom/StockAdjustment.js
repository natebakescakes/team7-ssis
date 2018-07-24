$(document).ready(function () {
    var $table = $('#mySATable');

    var datatb1=$table.DataTable({

        ajax: {
            url: "api/stockadjustment/all",
            dataSrc: ""
        },
        //sAjaxSource: "api/stockadjustment/all",
        //sAjaxDataProp: "",

        "columnDefs": [

            {

                "targets": -1,

                "render": function (data, type, full, meta) {

                    return "<input type = 'button' id = 'testButton' value = 'View'>"

                }

            }

        ],
        columns: [
            { data: "StockAdjustmentId"},
            { data: "CreatedBy" },
            { data: "ApprovedBySupervisor" },
            { data: "CreatedDateTime"},
            {data: "StatusName" },
            {data:  null}

        ],
        autowidth: true,
        select: "single",
        createdRow: function (row, data, dataIndex) {
            if (data.StatusName == "Approved") {
                $('td', row).eq(4).addClass('delivered');
            }
            if (data.StatusName == "Pending Approval") {
                $('td', row).eq(4).addClass('partially-delivered');
            }
            if (data.StatusName == "Rejected") {
                $('td', row).eq(4).addClass('awaiting-delivery');
            }
        },

        initComplete: function () {
            var api = this.api();
            api.columns().indexes().flatten().each(function (i) {
                var column = api.column(i);
                var select = $('<select><option value=""></option></select>')
                    .appendTo($(column.footer()).empty())
                    .on('change', function () {
                        var val = $.fn.dataTable.util.escapeRegex(
                            $(this).val()
                        );
                        column
                            .search(val ? '^' + val + '$' : '', true, false)
                            .draw();
                    });
                column.data().unique().sort().each(function (d, j) {
                    select.append('<option value="' + d + '">' + d + '</option>')
                });
            });
        }

    });

  


   
    $('#mySATable tbody').on("click", "#testButton", function () {

        ////获取行

        var row = $("table#mySATable tr").index($(this).closest("tr"));

        ////获取某列（从0列开始计数）的值

        var Id = $("table#mySATable").find("tr").eq(row).find("td").eq(0).text();

        //var status = $("table#mySATable").find("tr").eq(row).find("td").eq(4).text();

        //alert(Id + "是" + status;
       
        window.location.href = "StockAdjustment/Details/" + Id;

    });

});










