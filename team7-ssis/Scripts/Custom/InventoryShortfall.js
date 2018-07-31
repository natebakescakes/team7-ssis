$(document).ready(function () {

    
    var shortfallTbl = $('#shortfallTable').DataTable(
        {
            
            ajax: {

                url: "/api/inventory/shortfall",
                dataSrc: ""
            },

            columns: [

                {
                    data: "ItemCode",
                    defaultContent: "<i>Not available</i>"
                },
                {
                    data: "Description",
                    defaultContent: "<i>Not available</i>"
                },
                {
                    data: "Quantity",
                    defaultContent: "<i>Not available</i>"
                },
                {
                    data: "ReorderLevel"
                },
                {
                    data: "ReorderQuantity"
                },
                {
                    data: "Uom"
                },
                {
                    data: "AmountToReorder"
                }

            ],
            autowidth: true,
            select: "multiple",
            dom:"tp"

        });


    $("#generatePOforselected").on("click", function () {

        //$("#abc").submit();
        //alert($("#abc").attr('method'));
        //alert(JSON.stringify($("#abc").find(":input")));
        var data = $('#shortfallTable').DataTable().rows({ selected: true }).data().toArray();
       
       
        poNumbers = [];

            for (var i = 0; i < data.length; i++) {
                poNumbers.push(data[i].ItemCode);
        }
       // alert(poNumbers);
        
        generateUrl = $('#generateUrl').val();
        //alert(generateUrl);
        var form = document.createElement("form");
        var element1 = document.createElement("input");
        form.method = "POST";
        form.action = generateUrl;

        element1.value = poNumbers;
        element1.name = "poNums";
        element1.type = "hidden";
        form.appendChild(element1);

        document.body.appendChild(form);
  
        form.submit();
        
    });


    $(document).on("click", "#generatePOforall", function () {
        poNumbers = [];

        shortfallTbl.rows().every(function (rowIdx, tableLoop, rowLoop) {
            var data = $('#shortfallTable').DataTable().rows().data();
            poNumbers.push(data[rowIdx].ItemCode);

        });
        

        generateUrl = $('#generateUrl').val();
        
        var form = document.createElement("form");
        var element1 = document.createElement("input");
        form.method = "POST";
        form.action = generateUrl;

        element1.value = poNumbers;
        element1.name = "poNums";
        element1.type = "hidden";
        form.appendChild(element1);

        document.body.appendChild(form);

        form.submit();

    });




});