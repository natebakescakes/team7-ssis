$(document).ready(function () {
    var $table = $('#itemTable');
    var datatb1 = $table.DataTable({
          ajax: {
              url: "api/saveinsession/items",
              dataSrc: ""
          },
                
                     
                  columns: [
                  
                    {data: "ItemCode", "autoWidth": true },                 
                    {data: "Description", "autoWidth": true },
                    {data: "Reason", "autoWidth": true },
                    {data: "UnitPrice", "autoWidth": true },
                    {data: null, "autoWidth": true }
                   
                     ]

                        });

       
    });





