$(document).ready(function() {
  $('#DepartmentCode').on('change', function() {
    var selected = $('#DepartmentCode option:selected').val();
    $('#SupervisorEmail option').remove();
    $('#SupervisorEmail').append(
      $('<option      />')
        .val(0)
        .text('--- Please select supervisor ---'),
    );
    GetDropDownData(selected);
  });
});

function GetDropDownData(departmentCode) {
  $.ajax({
    type: 'GET',
    url: 'http://' + location.host + '/api/users/' + departmentCode,
    contentType: 'application/json; charset=utf-8',
    dataType: 'json',
    success: function(data) {
      $.each(data, function() {
        $('#SupervisorEmail').append(
          $('<option     />')
            .val(this.Email)
            .text(this.Name),
        );
      });
    },
    failure: function() {
      alert('Failed!');
    },
  });
}
