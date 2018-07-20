//var editor;
$(document).ready(function(){
	

        
		var $table=$('#poTable');
	
		
		//var simple_checkbox=function(data,type,full, meta){
		//	var checked= (data==true)? "checked" : "";
		//	return '<input type="checkbox"  disabled="true" class="checkbox td-button" ' + checked + '/>';
		//}
		
		
		var datatbl = $table.DataTable(
				{
					ajax: {
                        sAjaxSource: 'api/purchaseOrders/all',
                        sAjaxDataProp: ''
                },
                select: 'single',
                columns: [
                        //{
                        //defaultContent: '<input type="checkbox"  class="checkbox" />'
                        //},
						{
							data: 'PNo'
						},
						{
							data: 'SupplierName'
						},
						{
							data: 'CreatedDate'
						},
						{
							data: 'Status'
						}
					]
					
				});
		
		
		
  
	    

});
