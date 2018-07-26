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

                "targets": 5,

                "render": function (data, type, full, meta) {

                    return "<input type = 'button' id = 'testButton' value = 'View'>";

                }

            },
            {

                "targets": -1,

                "render": function (data, type, full, meta) {

                    return "<input type = 'button' id = 'testButton2' value = 'delete'>";

                }
            }

        ],
        columns: [
            { data: "StockAdjustmentId"},
            { data: "CreatedBy" },
            { data: "ApprovedBySupervisor" },
            { data: "CreatedDateTime"},
            { data: "StatusName" },
            { data: null },
            {
                defaultContent: null
            }
        ],
        autowidth: true,
        select: "single",
        createdRow: function (row, data, dataIndex) {
            if (data.status.StatusName == "Approved") {
                $('td', row).eq(4).addClass('delivered');
            }
            if (data.status.StatusName == "Pending Approval") {
                $('td', row).eq(4).addClass('partially-delivered');
            }
            if (data.status.StatusName == "Rejected") {
                $('td', row).eq(4).addClass('awaiting-delivery');
            }
            if (data.status.StatusName == "Draft") {
                $('td', row).eq(4).addClass('awaiting-delivery');
            }
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

    $('#mySATable tbody').on("click", "#testButton2", function () {

        ////获取行

        var row = $("table#mySATable tr").index($(this).closest("tr"));

        ////获取某列（从0列开始计数）的值

        var Id = $("table#mySATable").find("tr").eq(row).find("td").eq(0).text();
        //delete this one and refresh page

        $.ajax({
            type: "POST",
            url: '/api/stockadjustment/delete',
            data: JSON.stringify(Id),
            success: function (data) {
                if (data.status) {
                    alert("Item Price information has been successfully updated");
                    table.ajax.reload();
                }
            }
        });

    });

});










