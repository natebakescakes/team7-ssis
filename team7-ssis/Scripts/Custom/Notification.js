$(document).ready(function () {
    GetDropDownData();
});

function GetDropDownData() {
    $.ajax({
        type: 'GET',
        url: 'http://' + location.host + '/api/Notification/GetCurrentUser',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (data) {
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
            $.each(data, function () {
                if (this.NotificationType === 'Collection Ready') {
                    $('#notification-dropdown-items').append(
                        '<li class="list-group-item list-group-item-action small" data-value="' + this.NotificationId + '">' + this.Contents + '</li>'
                    );
                }
                if (this.NotificationType === 'Requisition Approval') {
                    $('#notification-dropdown-items').append(
                        '<li class="list-group-item list-group-item-action small" data-value="' + this.NotificationId + '">' + this.Contents + '</li>'
                    );
                }
                if (this.NotificationType === 'Stock Adjustment Approval') {
                    $('#notification-dropdown-items').append(
                        '<li class="list-group-item list-group-item-action small" data-value="' + this.NotificationId + '">' + this.Contents + '</li>'
                    );
                }
            });
        },
        failure: function () {
            alert('Failed!');
        },
        error: function (xhr, ajaxOptions, thrownError) {
            if (xhr.status == 404) {
                $('#notification-dropdown-items').append(
                    $('<li class="list-group-item">No notifications.</li>'),
                );
            }
        },
    });
}

$(document).on('click', '.list-group-item', function () {
    var notificationId = $(this).data("value");
    console.log(notificationId);
    post("/Notification/Read", { NotificationId: notificationId });
})
