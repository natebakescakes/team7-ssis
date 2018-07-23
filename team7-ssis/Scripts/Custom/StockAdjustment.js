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
            { data: "StockAdjustmentId", "autoWidth": true },
            { data: "CreatedBy", "autoWidth": true },
            { data: "ApprovedBySupervisor", "autoWidth": true },
            { data: "CreatedDateTime", "autoWidth": true },
            {data: "StatusName", "autoWidth": true },
            {data:  null}

        ],
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


        

    });


   
    $('#mySATable tbody').on("click", "#testButton", function () {

        ////获取行

        var row = $("table#mySATable tr").index($(this).closest("tr"));

        ////获取某列（从0列开始计数）的值

        var Id = $("table#mySATable").find("tr").eq(row).find("td").eq(0).text();

        //var job = $("table#mySATable").find("tr").eq(row).find("td").eq(4).text();

        //alert(Id + "是" + job);
       
        window.location.href = "StockAdjustment/Details/" + Id;

    });

});










