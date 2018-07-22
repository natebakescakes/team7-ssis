$(document).ready(function(){
	

        
		var $table=$('#poTable');
	
		
		var datatbl = $table.DataTable(
            {
                "createdRow": function (row, data, dataIndex) {
                    if(data[3] == "Delivered") {
                        $(row).addClass('red');
                    }
                },
                
                language: {
                    search: "_INPUT_",
                    searchPlaceholder: "Search...",
                },
                ajax: {

                    url: "api/purchaseOrder/all",
                    dataSrc: ""    
                },
                
                columns: [
                    { defaultContent: '<input type="checkbox" class="checkbox" />'},
                    {
                        data: "PNo",
                        defaultContent: "<i>Not available</i>" },
                    {
                        data: "SupplierName",
                        defaultContent: "<i>Not available</i>"},
                    {
                        data: "CreatedDate",
                        defaultContent: "<i>Not available</i>"},
                    {
                        data: "Status",
                        defaultContent: "<i>Not available</i>"
                    },
                    {
                        //defaultContent: "<button><i class='fa fa-info-circle'></i></button>"
                        defaultContent: '<select name="action"><option>Action</option><option value="0">View Purchase Order details</option> <option value=1>View Related Delivery Orders</option><option value=2>Receive Goods</option></select>'
                    }
                    
                ],
                autowidth: true,
                select: "single"
					
				});
		
	

});
