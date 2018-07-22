$(document).ready(function(){
	
    var action_dropbox = function (data, type, row, meta) {
        if (data == "Delivered") {
            return '<select  class="action form-control form-control-sm"><option value="-1">Action</option><option value="0">View Purchase Order details</option> <option value=1>View Related Delivery Orders</option></select>';

        }
        else if (data == "Partially Delivered") {
            return '<select  class=" action form-control form-control-sm"><option value="-1">Action</option><option value="0">View Purchase Order details</option> <option value=1>View Related Delivery Orders</option><option value=2 > Receive Goods</ option></select>';
        }
        else {
            return '<select class="action form-control form-control-sm"><option value="-1">Action</option><option value="0">View Purchase Order details</option> <option value=2 > Receive Goods</ option></select>';
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

                initComplete: function (){ // After DataTable initialized
                    
                    this.api().columns([4]).every(function () {
                        var column = this;
                        var select = $('<select multiple id="sel1" title="All Statuses" data-width="auto" data-style="btn-sm" class=" selectpicker  " ></select>')
                            .prependTo($('.dataTables_filter')) 
                        var download = $('<a class=" btn  btn-primary pull-left mr-3 btn-sm btn" href="#"><i class="fa fa-download" ></i>  Download Selected</a>').prependTo($('#poTable_length'))
                        var select = $('#sel1').on('change', function () {
                           
                            var val = $(this).val() + '';
               

                            column.search(val != '' ? '^' + val.split(',').join('$|^') + '$' : '', true, false).draw();
                            });

                        column.data().unique().sort().each(function (d, j) {
                            select.append('<option value="' + d + '">' + d + '</option>')
                        });
                        $('.selectpicker').selectpicker();  
                    }); // this.api function


                } //initComplete function 

               
        });

    $('#poTable tbody').on('change', '.action', function (e) {

        var poNum = JSON.stringify(datatbl.row($(this).parents('tr')).data().PNo);
        //alert(poNum);
        var value = Number($(this).val());
        //if (value == 0) { var u = 'PurchaseOrder/'; }
        //else if (value == 1) { alert(value); }
        //else if (value == 2) { alert(value); }
        //else { alert("HIIIIIII"); }
        $.ajax({
            type: 'POST',
            data: { poNum: poNum, val: value },
            url: 'PurchaseOrder/details',
            success: function (result) {
                alert("HIIIII" + result);
            }

        });
    });
                       
   
});
