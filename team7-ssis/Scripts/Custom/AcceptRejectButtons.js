// Click "Accept" button
$(document).on("click", "#approveBtn", function (e) {
    e.preventDefault();
    console.log("Approved");
    var data = myTable
        .row($(this).parents('tr'))
        .data();
    post("/Requisition/Approve", { rid: data.Requisition, email: "", remarks: "" });
});

// Click "Reject" button
$(document).on("click", "#rejectBtn", function (e) {
    e.preventDefault();
    console.log("Rejected");
    var data = myTable
        .row($(this).parents('tr'))
        .data();
    post("/Requisition/Reject", { rid: data.Requisition, email: "", remarks: "" });
});