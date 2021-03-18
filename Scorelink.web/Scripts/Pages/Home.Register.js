

var ViewModel = function () {
    var self = this;
    self.UserName = ko.observable();
    self.Name = ko.observable();
    self.Surname = ko.observable();
    self.Password = ko.observable();
    self.Email = ko.observable();
    self.Company = ko.observable();
    self.Telephone = ko.observable();
    self.Status = ko.observable();
    self.Admin = ko.observable();
    self.RegisterDate = ko.observable();
    self.ExpireDate = ko.observable();
    self.UpdateBy = ko.observable();
    self.UpdateDate = ko.observable();

    $(document).ready(function () {

        $("#btnRegister").click(function () {
            var sEmail = $("#Email").val();
            var sPass = $("#Password").val();
            var sRePass = $("#RePassword").val();

            if (!isEmail(sEmail)) {
                alert("Email is not correct format.");
            } else if (sPass != sRePass) {
                alert("Please retype password again.");
            } else {
                SaveRegister();
            }
        });

        

        function SaveRegister() {
            blockUI();
            var arg = {
                UserName: $("#Email").val(),
                Name: $("#Name").val(),
                Surname: $("#Surname").val(),
                Password: $("#Password").val(),
                Email: $("#Email").val(),
                //Company: $("#Company").val(),
                Address: $("#Address").val(),
                Telephone: $("#Telephone").val()
            }

            var data = {
                'item': arg
            };

            $.ajax({
                url: '/Home/SaveRegister',
                cache: false,
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: ko.toJSON(data),
                success: function (data) {
                    if (data == "OK") {
                        window.location.href = '/Home/Index';
                    } else if (data == "Dup") {
                        alert("Duplicate email can't register your information.");
                    } else {
                        alert("System can't register your information.");
                    }
                    unblockUI();
                }
            })
        }

        function isEmail(email) {
            var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            return regex.test(email);
        }
    });

    self.ClickAddCompany = function (data, event) {
        /*self.DocId = data.DocId();
        self.PageType = data.PageType();
        var PageNo = data.DocPageNo(),
            xString = PageNo.split(',');
        array = [];
        array = array.concat(xString);
        $('#page_delete').empty();
        $.each(array, function (i, p) {
            $('#page_delete').append($('<option></option>').val(p).html(p));
        });*/
        $("#Modal_DeletePage").modal('show');
    }
}

var viewModel = new ViewModel();
ko.applyBindings(viewModel);