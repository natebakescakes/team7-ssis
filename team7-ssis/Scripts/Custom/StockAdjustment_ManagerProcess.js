$(document).ready(function () {
    var $table = $('#mySATable');

    var datatb1 = $table.DataTable({

        ajax: {
            url: "/api/stockadjustment/allExceptDraft",
            dataSrc: ""
        },

        "columnDefs": [

            //{

            //    "targets": 5,

            //    "render": function (data, type, full, meta) {

            //        return "<button class='btn btn-safe' id='viewBtn' ><i class='fa fa-view'>View</i></button>";

            //    }

            //},
            {

                "targets": 5,

                "render": function (data, type, full, meta) {
                    if (data.StatusName=="Pending Approval")

                        return "<button class='btn btn-primary' id='processBtn' ><i class='fa fa-delete'>Process</i></button>";
                    else (data.StatusName == "Rejected" || data.StatusName == "Approved")
                    return "<button class='btn btn-primary' id='viewBtn' ><i class='fa fa-view'>View</i></button>";

                }
            }

        ],
        columns: [
            { data: "StockAdjustmentId" },
            { data: "CreatedBy" },
            { data: "ApprovedBySupervisor" },
            { data: "CreatedDateTime" },
            { data: "StatusName" },
            //{ data: null },
            { data: null }
        ],
        autowidth: true,
        select: "single",
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
            if (data.StatusName === "Draft") {
                $('td', row).eq(4).addClass('draft');
            }
            if (data.StatusName === "Cancelled") {
                $('td', row).eq(4).addClass('cancel');
            }
        }

    });


    $('#mySATable tbody').on("click", "#viewBtn", function () {

        var row = $("table#mySATable tr").index($(this).closest("tr"));
        var Id = $("table#mySATable").find("tr").eq(row).find("td").eq(0).text();
        window.location.href = "/StockAdjustment/Details/" + Id;

    });

    $('#mySATable tbody').on("click", "#processBtn", function () {

        var row = $("table#mySATable tr").index($(this).closest("tr"));

        var Id = $("table#mySATable").find("tr").eq(row).find("td").eq(0).text();

        window.location.href = "/StockAdjustment/Process/" + Id;


    })

});










