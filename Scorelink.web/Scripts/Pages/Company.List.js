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

    GetCompanylist();

    self.ClickOpenAdd = function (data, event) {
        $("#Modal_Page").modal('show');
    }

    $("#BtnAddCompany").click(function () {
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
            CompanyName: $("#CompanyName").val(),
            Domain: $("#Domain").val(),
            Address: $("#Address").val(),
            Telephone: $("#Telephone").val(),
            Status: $("#Status").prop('checked') ? 'Y' : 'N',
            CreateBy: $("#hdUserId").val(),
            UpdateBy: $("#hdUserId").val()
        }

        var data = {
            'item': arg
        };

        $.ajax({
            url: '/Company/AddCompany',
            cache: false,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: ko.toJSON(data),
            success: function (data) {
                if (data == "OK") {
                    window.location.href = '/Company/Index';
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