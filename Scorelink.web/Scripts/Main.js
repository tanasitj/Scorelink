$(document).ready(function () {
    $('a[href="#logout"]').click(function () {
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
});
