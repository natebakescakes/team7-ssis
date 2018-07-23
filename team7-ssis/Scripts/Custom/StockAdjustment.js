$(document).ready(function () {

    var $table = $('#mySATable');
    var datatb1 = $table.DataTable({
        language: {
            search: "_INPUT_",
            searchPlaceholder: "Search...",
        },
            ajax: {
            url: "api/stockadjustment/all",
            dataSrc: ""
        },
        columns: [
            {
                data: "StockAdjustmentId"
            },
            {
                data:
                    "CreatedBy"
            },
            {
                data:
                    "ApprovedBySupervisor"
            },
            {
                data:
                    "CreatedDateTime"
            },
            {
                data:
                    "StatusName"
            },
            {
                data:null,
            }
           
        ],

        "columnDefs": [

            {
                "targets": -1,

                "render": function (data, type, full, meta) {

                    return "<input type = 'button' id = 'testButton' value = 'View Detail'>"
                }
            }

        ],

        "autoWidth": true,
        select:"single",

        createdRow: function (row, data, dataIndex) {
            if (data.StatusName === "Approved") {
                $('td', row).eq(4).addClass('delivered');
            }
            if (data.StatusName === "Pending Approval") {
                $('td', row).eq(4).addClass('partially-delivered');
            }
            if (data.StatusName === "Rejected") {
                $('td', row).eq(4).addClass('awaiting-delivery');
            }
        },

    });

   $('#mySATable tbody').on("click", "#testButton", function () {

        //获取行

        var row = $("table#mySATable tr").index($(this).closest("tr"));

        //获取某列（从0列开始计数）的值

        var Id = $("table#mySATable").find("tr").eq(row).find("td").eq(0).text();

       var status = $("table#mySATable").find("tr").eq(row).find("td").eq(4).text();


       window.location.href = "/StockAdjustment/Details/" + Id;
        

       // alert(Id + "is" + status);

    });   
});