$(document).ready(function () {
    var $table = $('#mySATable');
    var datatb1 = $table.DataTable({

        sAjaxSource: "/api/stockadjustment/restoreitems",
        sAjaxDataProp: "",
                                     
                  columns: [
                  
                    {data: "ItemCode", "autoWidth": true },                 
                    {data: "Description", "autoWidth": true },
                    {data: null, "autoWidth": true },
                    {data: "UnitPrice", "autoWidth": true },
                    {data: null, "autoWidth": true }
                   
                     ]

                        });

       
    });





