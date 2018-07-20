//var editor;
$(document).ready(function(){
	

        var u = window.contextRoot + '/PurchaseOrderApi/loaddata';
		var $table=$('#myTable');
	
		
		//var simple_checkbox=function(data,type,full, meta){
		//	var checked= (data==true)? "checked" : "";
		//	return '<input type="checkbox"  disabled="true" class="checkbox td-button" ' + checked + '/>';
		//}
		
		
		var datatbl = $table.DataTable(
				{
					ajax: {
                        url: 'api/PurchaseOrderApi/loaddata',
                        type: 'get',
                        dataType: 'json'
					},
					columns: [
						{
							data: 'userid'
						},
						{
							data: 'username'
						},
						{
							data: 'firstName'
						},
						{
							data: 'lastName'
						},
						{
							data: 'email'
						},
						{
							data: 'address'
						},
						{
							data: 'phone'
						},
						{
							data: 'dob'
						},
						{
							data: 'enabled',
							render: simple_checkbox	
						},
						{
							data: 'admin',
							render: simple_checkbox
						},
						{
							defaultContent: "<button class='td-button btn-edit btn btn-table btn-primary'>Edit</button>"
						}
						
					]
					
				});
		
		
		

	     //$('#myTable tbody').on('click', '.btn-edit', function (e) {
	     //    var data = datatbl.row( $(this).parents('tr') ).data();
	    	// var counter = 0;
	     //    $($(this).parents('tr')).find("td").each(function(){
      //  		 counter++;
	     //   	 if(counter < 3){
	     //   		 return;
	     //   	 }

	     //   	 if ($(this).children().hasClass("checkbox"))
	     //   	 {
	     //   		( $(this).find("input")).prop("disabled",false);
	        		 
	     //   	 }
	     //   	 if (!$(this).children().hasClass("td-button"))
	     //   	    {
	     //   	        var text = $(this).text();
	     //   	        $(this).html ('<input type="text" size=10 value="' +  text + '">');
	     //   	    } 
	     //   	 if ($(this).children().hasClass("btn-edit"))
	     //   		 {
	     //   	       $(this).html ('<button class="td-button btn-save btn btn-table btn-primary">Submit</button>');
	     //   		 }
	     //   	   })
	      
	     // } );
	     
	     //$('#myTable tbody').on('click', '.btn-save', function (e) {
    		// var parenttr = $(this).parents('tr');
	    	// var counter = 0;
	    	// $($(this).parents('tr')).find("td").each(function(){
    		
      //  		 counter++;
	     //   	 if(counter < 3){
	     //   		 return;
	     //   	 }
	        	 
	     //   	 if ($(this).children().hasClass("checkbox"))
	     //   	 {
		    //    		( $(this).find("input")).prop("disabled",true);
		        		
		    //    		if ( ($(this).find("input")).prop('checked')==true)
		    //    		{
				  //      	 var cell = datatbl.cell( $(this) );
				        	 
				  //      	 cell.data(1).draw();
		   	//        	}
		        		
		   	//        	 else
		   	//        	 {
			   //	        	 var cell = datatbl.cell( $(this) );
			   //	        	 cell.data(0).draw();
		   	//        	 }
	        		
	     //   	 }
	        	 
	        	 
	     //   	 if (!$(this).children().hasClass("td-button"))
	     //   	    {
	     //   		 	var text = $(this).find("input").val();
	     //   		 	$(this).text(text);
	     //   		 	var cell = datatbl.cell( $(this) );
	     //   		 	cell.data( text ).draw();
	        	   
	     //   	    } 
	
		    	 
	     //   	 if ($(this).children().hasClass("btn-save"))
	     //   		 {
	     //   	        $(this).html ('<button class="td-button btn-edit btn btn-table btn-primary">Edit</button>');
	     //   		 } 
	        	 
	    	// }) ;
	        
	    	 
	     //    var memberdata = datatbl.row( parenttr ).data();
	       

	         
	     //    $.ajax({
	     //        url: window.contextRoot + "/admin/members/update",
	     //        type: "POST",
	     //        data: JSON.stringify(memberdata),
	     //        contentType: "application/json",
	     //        cache: true,
	     //        success: function (result) {
	            	
	     //        }
	     //      });
	      	    
	     // } );    
	    

});
