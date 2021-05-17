/*$(document).ready(function () {
    getMenuList($("#hdUserId").val());

    $("#alogout").click(function () {
        var data = {
            'userid': $("#hdUserId").val()
        };

        $.ajax({
            url: '/Home/Logout',
            cache: false,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: ko.toJSON(data),
            success: function (data) {
                window.location.href = '/Home/Index';
            }
        })

    });

});*/

$(document).ready(function () {
    getMenuList($("#hdUserId").val());

    $("a[href = '#logout']").click(function () {
        logout();
    });
});

function logout() {
    var data = {
        'userid': $("#hdUserId").val()
    };

    $.ajax({
        url: '/Home/Logout',
        cache: false,
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: ko.toJSON(data),
        success: function (data) {
            window.location.href = '/Home/Index';
        }
    });
}

function getMenuList(userid) {
    var filter = {
        'userId': userid
    }
    $.ajax({
        url: '/Home/GetMenuList',
        cache: false,
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: ko.toJSON(filter),
        success: function (data) {
            $("#menulist").html(data);
        }
    });
}