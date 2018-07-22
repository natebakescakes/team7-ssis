    $(document).ready(function () {

            var oTable = $('#myOutstandingTable').DataTable({

        "ajax": {

            "url": "outstandingitems",

                    "type": "get",

                    "datatype": "json"

                },

                "columns": [

                    {"data": "Item Code", "autoWidth": true},

                    {"data": "Description", "autoWidth": true },

                    {"data": "Outstanding Qty", "autoWidth": true },

                ]
            });

