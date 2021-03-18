function UserModel(UserId, UserName, Name, Surname, FullName, Email, CompanyId, CompanyName, Address, Telephone, Status, Admin, RegisterDate, ExpireDate) {
    var self = this;
    self.UserId = ko.observable(UserId);
    self.UserName = ko.observable(UserName);
    self.Name = ko.observable(Name);
    self.Surname = ko.observable(Surname);
    self.FullName = ko.observable(FullName);
    self.Email = ko.observable(Email);
    self.Company = ko.observable(CompanyId);
    self.CompanyName = ko.observable(CompanyName);
    self.Address = ko.observable(Address);
    self.Telephone = ko.observable(Telephone);
    self.Status = ko.observable(Status === 'N' ? false : true);
    self.Admin = ko.observable(Admin === 'N' ? false : true);
    self.RegisterDate = ko.observable(RegisterDate);
    self.ExpireDate = ko.observable(ExpireDate);
}

var ViewModel = function () {
    var self = this;

    self.UserList = ko.observableArray();
    self.CompanyList = ko.observableArray();

    self.UserId = ko.observable();
    self.UserName = ko.observable();
    self.Name = ko.observable();
    self.Surname = ko.observable();
    self.FullName = ko.observable();
    self.Password = ko.observable();
    self.Email = ko.observable();
    self.Company = ko.observable();
    self.CompanyName = ko.observable();
    self.Address = ko.observable();
    self.Telephone = ko.observable();
    self.Status = ko.observable(false);
    self.Admin = ko.observable(false);
    self.RegisterDate = ko.observable();
    self.ExpireDate = ko.observable();

    self.EntryUserId = ko.observable();
    self.EntryUserName = ko.observable();
    self.EntryName = ko.observable();
    self.EntrySurname = ko.observable();
    self.EntryPassword = ko.observable();
    self.EntryRePassword = ko.observable();
    self.EntryEmail = ko.observable();
    self.EntryCompanyId = ko.observable();
    self.EntryCompanyName = ko.observable();
    self.EntryAddress = ko.observable();
    self.EntryTelephone = ko.observable();
    self.EntryStatus = ko.observable(false);
    self.EntryAdmin = ko.observable(false);
    self.EntryRegisterDate = ko.observable();
    self.EntryExpireDate = ko.observable();

    GetUserlist();
    GetCompanyDD();

    self.ClickOpenAdd = function (data, event) {
        init();
        $("#Modal_Page").modal('show');
    }

    self.ClickOpenEdit = function (data, event) {
        var filter = {
            'UserId': data.UserId()
        }
        $.ajax({
            url: '/User/GetUserDetail',
            cache: false,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: ko.toJSON(filter),
            success: function (data) {
                self.EntryUserId(data.UserId);
                self.EntryUserName(data.UserName);
                self.EntryName(data.Name);
                self.EntrySurname(data.Surname);
                //self.EntryPassword(data.Password);
                //self.EntryRePassword(data.Password);
                self.EntryEmail(data.Email);
                self.EntryCompanyId(data.Company);
                self.EntryAddress(data.Address);
                self.EntryTelephone(data.Telephone);
                self.EntryStatus(data.Status === 'N' ? false : true);
                self.EntryAdmin(data.Admin === 'N' ? false : true);
                self.EntryRegisterDate(data.RegisterDate);
                self.EntryExpireDate(data.ExpireDate);
            }
        });

        $("#Modal_Page").modal('show');
    }

    function GetCompanyDD() {
        $.ajax({
            url: '/User/GetCompanyDD',
            cache: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                self.CompanyList([]);
                self.CompanyList(data);
            }
        });
    }

    function GetUserlist() {

        blockUI();

        $.ajax({
            url: '/User/GetUserList',
            cache: false,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: ko.toJSON(),
            success: function (data) {
                self.UserList([]);
                ko.utils.arrayForEach(data, function (data) {
                    self.UserList.push(
                        new UserModel(
                            data.UserId,
                            data.UserName,
                            data.Name,
                            data.Surname,
                            data.FullName,
                            data.Email,
                            data.Company,
                            data.CompanyName,
                            data.Address,
                            data.Telephone,
                            data.Status,
                            data.Admin,
                            data.RegisterDate,
                            data.ExpireDate
                        )
                    );
                });

                unblockUI();
            }
        })
            .done(function () {
                var table = $('#tbUser');
                table.DataTable(
                    {
                        bDestroy: true,
                        pageLength: 10,
                        "order": [[0, "asc"]]
                    }
                );
            })
            .fail(
                function (xhr, textStatus, err) {
                    //PNotification("Error", err, "error");
                    unblockUI();
                });
    }

    function init() {
        self.EntryUserId(0);
        self.EntryUserName('');
        self.EntryName('');
        self.EntrySurname('');
        self.EntryPassword('');
        self.EntryRePassword('');
        self.EntryEmail('');
        self.EntryCompanyId('');
        self.EntryAddress('');
        self.EntryTelephone('');
        self.EntryStatus(false);
        self.EntryAdmin(false);
        self.EntryRegisterDate('');
        self.EntryExpireDate('');
    }

    $("#BtnSaveUser").click(function () {
        $('#tbUser').DataTable().clear();
        $('#tbUser').DataTable().destroy();
        $("#Modal_Page").modal('hide');
        SaveUser();
    });

    function SaveUser() {
        blockUI();

        var arg = {
            UserId: $("#UserId").val(),
            UserName: $("#UserName").val(),
            Name: $("#Name").val(),
            Surname: $("#Surname").val(),
            Address: $("#Address").val(),
            Email: $("#Email").val(),
            Password: $("#Password").val(),
            Company: $("#Company").val(),
            Telephone: $("#Telephone").val(),
            RegisterDate: $("#RegisterDate").val(),
            ExpireDate: $("#ExpireDate").val(),
            Status: $("#Status").prop('checked') ? 'Y' : 'N',
            Admin: $("#Admin").prop('checked') ? 'Y' : 'N',
            UpdateBy: $("#hdUserId").val()
        }

        var data = {
            'item': arg
        };

        $.ajax({
            url: '/User/SaveUser',
            cache: false,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: ko.toJSON(data),
            success: function (data) {
                if (data == "OK") {
                    
                } else if (data == "UserDup") {
                    alert("Duplicate user name can't add your data.");
                } else {
                    alert("System can't save your data.");
                }
                GetUserlist();
                unblockUI();
            }
        })
    }

    self.ClickRefresh = function (data, event) {
        $('#tbUser').DataTable().clear();
        $('#tbUser').DataTable().destroy();
        GetUserlist();
    }
}

var viewModel = new ViewModel();
ko.applyBindings(viewModel);