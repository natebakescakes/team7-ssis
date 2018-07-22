$(document).ready(function(){
	
    var action_dropbox = function (data, type, row, meta) {
        if (data == "Delivered") {
            return '<select name="action" class="action"><option>Action</option><option value="0">View Purchase Order details</option> <option value=1>View Related Delivery Orders</option></select>';

        }
        else if (data == "Partially Delivered") {
            return '<select name="action" class="action"><option>Action</option><option value="0">View Purchase Order details</option> <option value=1>View Related Delivery Orders</option><option value=2 > Receive Goods</ option></select>';
        }
        else {
            return '<select name="action" class="action"><option>Action</option><option value="0">View Purchase Order details</option> <option value=2 > Receive Goods</ option></select>';
        }
        }
       
        
		var $table=$('#poTable');
	
		
		var datatbl = $table.DataTable(
            {
               
                language: {
                    search: "_INPUT_",
                    searchPlaceholder: "Search...",
                },
                ajax: {

                    url: "api/purchaseOrder/all",
                    dataSrc: ""    
                },
                
                columns: [
                    { defaultContent: '<input type="checkbox" class="checkbox" />' },
                    {
                        data: "PNo",
                        defaultContent: "<i>Not available</i>"
                    },
                    {
                        data: "SupplierName",
                        defaultContent: "<i>Not available</i>"
                    },
                    {
                        data: "CreatedDate",
                        defaultContent: "<i>Not available</i>"
                    },
                    {
                        data: "Status"  
                    },
                    {
                        data: "Status",
                        render: action_dropbox
                    }
                    
                ],
                autowidth: true,
                select: "single",

                createdRow: function (row, data, dataIndex) {
                    if (data.Status == "Delivered") {
                        $('td', row).eq(4).addClass('delivered');
                    }
                    if (data.Status == "Partially Delivered") {
                        $('td', row).eq(4).addClass('partially-delivered');
                    }
                    if (data.Status == "Awaiting Delivery") {
                        $('td', row).eq(4).addClass('awaiting-delivery');
                    }
                },

                initComplete: function () { // After DataTable initialized
                    this.api().columns([4]).every(function () {
                        var column = this;
                        //var select = $('<select id="testSelect1" ><option value="">Filter</option></select>')
                        //    .appendTo($('#poTable .dropdown .fifth').empty()) /* for multiples use .appendTo( $(column.header()).empty() ) or .appendTo( $(column.footer()).empty() ) */
                          var select=$('#sel1') .on('change', function () {
                                var val = $.fn.dataTable.util.escapeRegex(
                                    $(this).val());
                                column.search(val ? '^' + val + '$' : '', true, false).draw();
                            });

                        column.data().unique().sort().each(function (d, j) {
                            select.append('<option value="' + d + '">' + d + '</option>')
                        });
                    }); // this.api function
                } //initComplete function 

               
        });

       
            
		
    //$('#testSelect1').multiselect({
    //    nonSelectedText: 'Select Framework',
    //    enableFiltering: true,
    //    enableCaseInsensitiveFiltering: true,
    //    buttonWidth: '400px'
    //});

});
