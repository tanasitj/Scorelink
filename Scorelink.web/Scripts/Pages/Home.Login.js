var ViewModel = function () {
    var self = this;
    self.UserId = ko.observable();
    self.IPAddress = ko.observable();
    self.SessionId = ko.observable();
    self.MACAddress = ko.observable();
    self.CPUNO = ko.observable();

    //$("#btnLogIn").click(function () {
    //    window.location.href = "/UserProfile/Index";
    //});

    self.ClickLogin = function () {
        var sEmail = $("#txtUser").val();

        if (!isEmail(sEmail)) {
            alert("User is not correct format.");
        } else {
            Login();
        }
    }

    function Login() {
        blockUI();
        var arg = {
            IPAddress: $("#hdIPc").val(),
            SessionId: $("#hdSessionc").val(),
            MACAddress: "",
            CPUNO: $("#hdCPUc").val()
        }

        var filter = {
            'user': $("#txtUser").val(),
            'pass': $("#txtPass").val(),
            'online': arg
        }

        $.ajax({
            url: '/Home/Login',
            cache: false,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: ko.toJSON(filter),
            success: function (data) {
                if (data == "Ok") {
                    window.location.href = "/UserProfile/Index";
                } else if (data == "Time") {
                    unblockUI();
                    alert("Duplicate login please wait to login again.!!");
                } else {
                    unblockUI();
                    alert("Username or Password is not correct.!!");
                }
            }
        });
    }

    function isEmail(email) {
        var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        return regex.test(email);
    }

}

var viewModel = new ViewModel();
ko.applyBindings(viewModel);