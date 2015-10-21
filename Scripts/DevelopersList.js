function initEdiTable() {
    $('.editfield').editable({
        send: 'never',
        inputclass: 'editableInputLength',
        validate: function (value) {
            var v = valib.String.isEmailLike(value)
            if (v == false) return 'Please insert valid mail';
        }
    });
}

function RemoveUserFromList(user) {
    $('[data-for="' + user + '"]').remove();
}

function addNewUser() {

    var newUserEmailIs = $("#newUserEmail").val();

    if ($("a:contains('" + newUserEmailIs + "')").length > 0) {
        alert("User " + newUserEmailIs + " was already added to list");
        return;
    }

    var newUerHtml = ' <div class="panel panel-default" data-for="' + newUserEmailIs + '">' +
    '<div class="panel-body">' +
    '<a href="#" class="editfield" data-type="text" data-original-title="developer Email">' + newUserEmailIs + '</a>' +
    '<button onclick="RemoveUserFromList(\'' + newUserEmailIs + '\')" type="button" class="btn btn-danger btn-xs" style="float:right">' +
    '<span class="glyphicon glyphicon-remove" aria-hidden="true"></span>' +
    '</button>' +
    '</div>' +
     '</div>';

    $("#usersList").prepend(newUerHtml);
    initEdiTable();
}

function saveChanges() {
    var users = { jsonUserList: [] };
    $.each($(".editfield"), function (i, v) {
        users.jsonUserList.push($(v).text());
    })

    $.ajax({
        url: "/Home/UpdateUserList",
        type: "POST",
        data: users,
        success: function (data) {
            if (data.result == "ok")
                toastr.success("Changes were saved :)");
            else if (data.result == "fail")
                toastr.error("Changes weren't saved :(", "ERROR!!!");
        },
        dataType: "json"
    });
}

initEdiTable();