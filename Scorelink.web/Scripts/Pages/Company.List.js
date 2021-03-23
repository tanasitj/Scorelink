function CompanyModel(CompanyId, CompanyName, Domain, Address, Telephone, Status, CreateBy, CreateDate) {
    var self = this;
    self.CompanyId = ko.observable(CompanyId);
    self.CompanyName = ko.observable(CompanyName);
    self.Domain = ko.observable(Domain);
    self.Address = ko.observable(Address);
    self.Telephone = ko.observable(Telephone);
    self.Status = ko.observable(Status === 'N' ? false : true);
    self.CreateBy = ko.observable(CreateBy);
    self.CreateDate = ko.observable(CreateDate);
}

var ViewModel = function () {
    var self = this;
    self.CompanyList = ko.observableArray();
    self.CompanyId = ko.observable();
    self.CompanyName = ko.observable();
    self.Domain = ko.observable();
    self.Address = ko.observable();
    self.Telephone = ko.observable();
    self.Status = ko.observable(false);

    self.EntryCompanyId = ko.observable();
    self.EntryCompanyName = ko.observable();
    self.EntryDomain = ko.observable();
    self.EntryAddress = ko.observable();
    self.EntryTelephone = ko.observable();
    self.EntryStatus = ko.observable(false);

    self.Mode = ko.observable();

    GetCompanylist();

    self.ClickOpenAdd = function (data, event) {
        self.Mode('A');
        self.EntryCompanyId(0);
        self.EntryCompanyName('');
        self.EntryDomain('');
        self.EntryAddress('');
        self.EntryTelephone('');
        self.EntryStatus(false);
        $("#Modal_Page").modal('show');
    }

    self.ClickOpenEdit = function (data, event) {
        self.Mode('E');
        var filter = {
            'CompanyId': data.CompanyId()
        }
        $.ajax({
            url: '/Company/GetCompanyDetail',
            cache: false,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: ko.toJSON(filter),
            success: function (data) {
                self.EntryCompanyId(data.CompanyId);
                self.EntryCompanyName(data.CompanyName);
                self.EntryDomain(data.Domain);
                self.EntryAddress(data.Address);
                self.EntryTelephone(data.Telephone);
                self.EntryStatus(data.Status === 'N' ? false : true);
            }
        });
        
        $("#Modal_Page").modal('show');
    }

    self.ClickRefresh = function (data, event) {
        $('#tbCompany').DataTable().clear();
        $('#tbCompany').DataTable().destroy();
        GetCompanylist();
    }

    $("#BtnSaveCompany").click(function () {
        $('#tbCompany').DataTable().clear();
        $('#tbCompany').DataTable().destroy();
        $("#Modal_Page").modal('hide');
        SaveCompany();
    });

    function GetCompanylist() {

        blockUI();

        $.ajax({
            url: '/Company/GetCompanyList',
            cache: false,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: ko.toJSON(),
            success: function (data) {
                self.CompanyList([]);
                ko.utils.arrayForEach(data, function (data) {
                    self.CompanyList.push(
                        new CompanyModel(
                            data.CompanyId,
                            data.CompanyName,
                            data.Domain,
                            data.Address,
                            data.Telephone,
                            data.Status,
                            data.CreateBy,
                            data.CreateDate
                        )
                    );
                });

                unblockUI();
            }
        })
        .done(function () {
            var table = $('#tbCompany');
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

    function SaveCompany() {
        blockUI();

        var arg = {
            CompanyId: $("#CompanyId").val(),
            CompanyName: $("#CompanyName").val(),
            Domain: $("#Domain").val(),
            Address: $("#Address").val(),
            Telephone: $("#Telephone").val(),
            Status: $("#Status").prop('checked') ? 'Y' : 'N',
            CreateBy: $("#hdUserId").val(),
            UpdateBy: $("#hdUserId").val()
        }

        var data = {
            'mode': self.Mode,
            'item': arg
        };

        $.ajax({
            url: '/Company/SaveCompany',
            cache: false,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: ko.toJSON(data),
            success: function (data) {
                if (data == "OK") {
                    GetCompanylist();
                    //window.location.href = '/Company/Index';
                } else if (data == "CompanyDup") {
                    alert("Duplicate company name can't add your data.");
                } else if (data == "DomainDup") {
                    alert("Duplicate domain name can't add your data.");
                } else {
                    alert("System can't save your data.");
                }
                unblockUI();
            }
        })
    }
}

var viewModel = new ViewModel();
ko.applyBindings(viewModel);