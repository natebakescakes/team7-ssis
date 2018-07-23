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
      if (data.filter(x => x.Status === 'Unread').length > 0) {
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
        if (this.NotificationType === 'Collection Ready') {
          $('#notification-dropdown-items').append(
            $(
              '<form action="/Notification/Read" method="post" role="form"><input type="number" style="display: none;" id="NotificationId", name="NotificationId", value="' +
                this.NotificationId +
                '" /><input type="submit" class="dropdown-item" value="Requisition ' +
                this.Contents +
                ' is ready for collection. "/></form>',
            ),
          );
        }
        // }
        if (this.NotificationType === 'Requisition Approval') {
          $('#notification-dropdown-items').append(
            $(
              '<form action="/Notification/Read" method="post" role="form"><input type="number" style="display: none;" id="NotificationId", name="NotificationId", value="' +
                this.NotificationId +
                '" /><input type="submit" class="dropdown-item" value="Requisition ' +
                this.Contents +
                ' is awaiting your approval. "/></form>',
            ),
          );
        }
        if (this.NotificationType === 'Stock Adjustment Approval') {
          $('#notification-dropdown-items').append(
            $(
              '<form action="/Notification/Read" method="post" role="form"><input type="number" style="display: none;" id="NotificationId", name="NotificationId", value="' +
                this.NotificationId +
                '" /><input type="submit" class="dropdown-item" value="Stock Adjustment ' +
                this.Contents +
                ' is awaiting your approval. "/></form>',
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
