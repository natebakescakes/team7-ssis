$(document).ready(function() {
  GetDropDownData();
});

function GetDropDownData() {
  $.ajax({
    type: 'GET',
    url: 'http://' + location.host + '/api/notifications',
    contentType: 'application/json; charset=utf-8',
    dataType: 'json',
    success: function(data) {
      if (data.length > 0) {
        $('#notification-dropdown span').remove();
        $('#notification-dropdown')
          .append(' ')
          .append(
            $(
              '<span class="badge badge-pill badge-light">' +
                data.length +
                '</span>',
            ),
          );
      }
      $.each(data, function() {
        // if (data.NotificationType === 'Collection Ready') {
          $('#notification-dropdown-items').append(
            $(
              '<a class="dropdown-item" href="http://' +
                location.host +
                '/Requisition?=' +
                this.Contents +
                '">Requisition ' +
                this.Contents +
                ' is ready for collection.</a>',
            ),
          );
        // }
        if (data.NotificationType === 'Requisition Approval') {
          $('#notification-dropdown-items').append(
            $(
              '<a class="dropdown-item" href="http://' +
                location.host +
                '/Requisition?=' +
                this.Contents +
                '">Requisition ' +
                this.Contents +
                ' is awaiting your approval.</a>',
            ),
          );
        }
        if (data.NotificationType === 'Stock Adjustment Approval') {
          $('#notification-dropdown-items').append(
            $(
              '<a class="dropdown-item" href="http://' +
                location.host +
                '/StockAdjustment?=' +
                this.Contents +
                '">Stock Adjustment ' +
                this.Contents +
                ' is awaiting your approval.</a>',
            ),
          );
        }
      });
    },
    failure: function() {
      alert('Failed!');
    },
  });
}
