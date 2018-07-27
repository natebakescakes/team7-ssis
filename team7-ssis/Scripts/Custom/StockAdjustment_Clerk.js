$(document).ready(function () {
    var $table = $('#ClerkTable');

    var datatb1=$table.DataTable({

        ajax: {
            url: "/api/stockadjustment/all",
            dataSrc: ""
        },

        "columnDefs": [

            {

                "targets": 5,

                "render": function (data, type, full, meta) {
                    if (data.StatusName == "Pending Approval" || data.StatusName == "Draft")

                        return "<button class='btn btn-warning' id='processBtn' ><i class='fa fa-delete'>Process</i></button>";

                    else (data.StatusName == "Approved" || data.StatusName == "Rejected")
                        return "<button class='btn btn-primary' id='viewBtn' ><i class='fa fa-view'>View</i></button>";


                }
            }


        ],
        columns: [
            { data: "StockAdjustmentId"},
            { data: "CreatedBy" },
            { data: "ApprovedBySupervisor" },
            { data: "CreatedDateTime"},
            { data: "StatusName" },
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

   
    $('#ClerkTable tbody').on("click", "#viewBtn", function () {

      var row = $("table#ClerkTable tr").index($(this).closest("tr"));
        var Id = $("table#ClerkTable").find("tr").eq(row).find("td").eq(0).text();
        window.location.href = "/StockAdjustment/DetailsNoEdit/" + Id;

    });

    $('#ClerkTable tbody').on("click", "#processBtn", function () {

        var row = $("table#ClerkTable tr").index($(this).closest("tr"));

        var Id = $("table#ClerkTable").find("tr").eq(row).find("td").eq(0).text();
        window.location.href = "/StockAdjustment/DetailsEdit/" + Id;

        //$.ajax({
        //    type: "GET",
        //    contentType: 'application/json',
        //    url: '/api/stockadjustment/delete/?id=' + Id,

        //    success: function (responseJSON) {
        //        window.location.replace("/StockAdjustment");
        //    }
        //});

    });

});










