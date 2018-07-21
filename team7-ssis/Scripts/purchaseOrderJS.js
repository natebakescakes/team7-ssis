$(document).ready(function(){
	

        
		var $table=$('#poTable');
	
		
		var datatbl = $table.DataTable(
				{
                ajax: {

                    url: "api/purchaseOrder/all",
                    dataSrc: ""    
                },
                
                columns: [
                    { data: "PNo" },
                    { data: "SupplierName"},
                    { data: "CreatedDate"},
                    { data: "Status" }
                ],
                autowidth:"true",
                select: "single"
					
				});
		
	

});
