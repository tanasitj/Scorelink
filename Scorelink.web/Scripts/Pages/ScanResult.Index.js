function ScanEditModel(Footnote_No, Divisions, Digitized_Account_Title, Recovered, Amount, Modified, CLCTCD) {
    var self = this;
    self.Footnote_No = ko.observable(Footnote_No);
    self.Divisions = ko.observable(Divisions);
    self.Digitized_Account_Title = ko.observable(Digitized_Account_Title);
    self.Recovered = ko.observable(Recovered);
    self.Amount = ko.observable(Amount);
    self.Modified = ko.observable(Modified);
    self.CLCTCD = ko.observable(CLCTCD);
}
var ViewModel = function () {
    var self = this;
    self.ScanEdit = ko.observableArray();
    self.Footnote_No = ko.observable();
    self.Divisions = ko.observable();
    self.Digitized_Account_Title = ko.observable();
    self.Recovered = ko.observable();
    self.Amount = ko.observable();
    self.Modified = ko.observable();
    self.CLCTCD = ko.observable();
    LoadData();
    var pickedup;
    var tr;
    var row_index;
    var footnote;
    var footnote2;
    var digitize_account;
    $(document).ready(function () {
        $("#tbResult").chromatable({
            width: "700px",
            height: "630px",
            scrolling: "yes"
        });
        $("#tbResult tbody tr").on("click", function (event) {          
            tr = $(this).closest("tr").find("td").addClass("selected");
            footnote = $(this).closest("tr").find('td:eq(0)').text();
            digitize_account = $(this).closest("tr").find('td:eq(2)').text();
            amount = $(this).closest("tr").find('td:eq(4)').text();
            modified = $(this).closest("tr").find('td:eq(5)').text();           
        });
        $('td').click(function () {
            row_index = $(this).parent().index();
            var col_index = $(this).index();
        });
        // Insert Row
        $("#BtnInsert").click(function () {
            $('#tbResult').append('<tr><td>text</td><td>text</td><td>text</td><td>text</td><td>text</td><td>text</td><td>text</td></tr>');
            $('#Table tbody:last').append(newRow);
            $("#BtnCheck").attr("disabled", true);
            $("#BtnMerge").attr("disabled", true);
            $("#BtnExport").attr("disabled", true);
            $("#BtnDelete").attr("disabled", true);
            $("#Commit").attr("disabled", true);
        });
        $("#BtnMerge").click(function () {
            var footnote2 = $('table > tbody > tr').eq(row_index).find('td:eq(0)').text() + footnote;
            var digitize_account2 = $('table > tbody > tr').eq(row_index).find('td:eq(2)').text() + " " + digitize_account;
            var amount2 = $('table > tbody > tr').eq(row_index).find('td:eq(4)').text() + amount;
            tr.remove();
            document.getElementById("tbResult").deleteRow(row_index);
            var html =
                "<tr><td>" + footnote2 + "</td><td></td><td>" + digitize_account2 + "</td><td></td><td>"
                + amount2 + "</td><td>*</td><td></td></tr>";
            pickedup = $(this);
            $('table > tbody > tr').eq(row_index).after(html);
            $("#tbResult").find("td").removeClass("selected");
        });
        $("#BtnDelete").click(function () {
            tr.remove(); 
        });
        //Check --edit data
        $("#BtnCheck").click(function () {
            $.ajax({
                url: "/ScanResult/AssignGridCheck",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                datatype: "json",
                data: "",
                success: OnSuccessCheck,
                failure: function (response) {
                    alert(response.d);
                },
                error: function (response) {
                    alert(response.d);
                }
            });
            $("#BtnCheck").attr("disabled", true);
            $("#BtnMerge").attr("disabled", true);
            $("#BtnInsert").attr("disabled", false);
            $("#BtnDelete").attr("disabled", false);
        });
        $("#BtnCancel").click(function () {
            $("#tbResult").find("td").removeClass("selected");
            $.ajax({
                url: "/ScanResult/AssignGridMerge",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                datatype: "json",
                data: "",
                success: OnSuccessCancel,
                failure: function (response) {
                    alert(response.d);
                },
                error: function (response) {
                    alert(response.d);
                }
            });
            //$("#BtnCheck").removeattr("disabled");
            $("#BtnCheck").attr("disabled", false);
            $("#BtnMerge").attr("disabled", false);
            $("#BtnInsert").attr("disabled", true);
            $("#BtnDelete").attr("disabled", true);

        });

        $("#BtnExport").click(function () {
            ResultsToTable();
        });
    });
    function ResultsToTable() {
        $grid = $("#tbResult").clone();
        $grid.find("input").remove();
        $grid.table2excel({
            exclude: ".noExl",
            name: "Results"
        });
    }
    function OnSuccessCheck(response) {
        var model = response;
        $(".Grid td:nth-child(2),th:nth-child(2)").toggle();
        $(".Grid td:nth-child(4),th:nth-child(4)").toggle();
        $(".Grid td:nth-child(7),th:nth-child(7)").toggle();
        var row = $("#tbResult tbody tr:last-child").clone(true);
        $("#tbResult tbody tr").remove();
        $.each(model, function () {
            var financials = this;
            $("td", row).eq(0).html(financials.Footnote_No);
            $("td", row).eq(1).html(financials.Divisions);
            $("td", row).eq(2).html(financials.Digitized_Account_Title);
            $("td", row).eq(3).html(financials.Recovered);
            $("td", row).eq(4).html(financials.Amount);
            $("td", row).eq(5).html(financials.Modified);
            $("td", row).eq(6).html(financials.CLCTCD);
            $("#tbResult").append(row);
            row = $("#tbResult tbody tr:last-child").clone(true);
        });
    };
    function OnSuccessCancel(response) {
        var model = response;
        $(".Grid td:nth-child(2),th:nth-child(2)").hide();
        $(".Grid td:nth-child(4),th:nth-child(4)").hide();
        $(".Grid td:nth-child(7),th:nth-child(7)").hide();
        var row = $("#WebGrid tbody tr:last-child").clone(true);
        $("#tbResult tbody tr").remove();
        $.each(model, function () {
            var financials = this;
            $("td", row).eq(0).html(financials.Footnote_No);
            $("td", row).eq(2).html(financials.Digitized_Account_Title);
            $("td", row).eq(4).html(financials.Amount);
            $("td", row).eq(5).html(financials.Modified);
            $("#tbResult").append(row);
            row = $("#tbResult tbody tr:last-child").clone(true);
        });
    };
    function LoadData() {

        var filter = {
            docId: $("#hddocId").val(),
            pageType: $("#hdPageType").val()
        }
        $("#BtnInsert").attr("disabled", true);
        $("#BtnDelete").attr("disabled", true);
        $.ajax({
            url: '/ScanResult/AssignGridMerge',
            cache: false,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: ko.toJSON(filter),
            success: function (data) {
                self.ScanEdit([]);
                ko.utils.arrayForEach(data, function (data) {
                    self.ScanEdit.push(
                        new ScanEditModel(
                            data.Footnote_No,
                            data.Divisions,
                            data.Digitized_Account_Title,
                            data.Recovered,
                            data.Amount,
                            data.Modified,
                            data.CLCTCD
                        )
                    );
                });
            }
        })
    }
}
var viewModel = new ViewModel();
ko.applyBindings(viewModel);
