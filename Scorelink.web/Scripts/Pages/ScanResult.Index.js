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
    var lastSelected;
    $(document).ready(function () {
        $("#tbResult").chromatable({
            width: "Auto",
            height: "630px",
            scrolling: "yes"
        });
        // get row number
        $('#tbResult tbody tr').each(function (row_num) {
            $(this).children("td:eq(0)").html(row_num + 1);
        });

        $("#tbResult tbody tr").on("click", function (event) { 
            
            tr = $(this).closest("tr").find("td").addClass("selected");
            footnote = $(this).closest("tr").find('td:eq(1)').text();
            digitize_account = $(this).closest("tr").find('td:eq(3)').text();
            amount = $(this).closest("tr").find('td:eq(5)').text();
            modified = $(this).closest("tr").find('td:eq(6)').text();   

            // get back to where it was before if it was selected :
            if (pickedup != null) {
                pickedup.css("background-color", "#ffffff");
            }
            $(this).css("background-color", "skyblue");

            pickedup = $(this);
        });
        $('td').click(function () {
            row_index = $(this).parent().index();
            var col_index = $(this).index();
        });
        //$('#tbResult tr').live('click', function (event) {

        //    var tableRow = $(this).closest("tr").prevAll("tr").length + 1;
        //    if ($(this).hasClass('row_selected')) {
        //        $(this).removeClass('row_selected');
        //    }
        //    else {
        //        $(this).addClass('row_selected');
        //    }

        //    if (event.shiftKey) {
        //        var table = $('#tbResult');


        //        var start = Math.min(tableRow, lastSelected);
        //        var end = Math.max(tableRow, lastSelected);

        //        for (var i = start; i < end; i++) {
        //            //$(this).parent().parent().addClass('row_selected'); 
        //            table.find('tr:gt(' + (start - 1) + '):lt(' + (end) + ')').addClass('row_selected');
        //        }

        //    } else {

        //        lastSelected = $(this).closest("tr").prevAll("tr").length + 1;
        //    }
        //});
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

            //var id_row1 = $('table > tbody > tr').eq(row_index).find('td:eq(0)').text();
            //var id_row2 = $('table > tbody > tr').eq(row_index).find('td:eq(0)').text();
            var footnote2 = $('table > tbody > tr').eq(row_index).find('td:eq(1)').text();
            if ((footnote2 != "") && (footnote != "")) {
                footnote2 = $('table > tbody > tr').eq(row_index).find('td:eq(1)').text() + "," + footnote;
            }
            else {
                footnote2 = $('table > tbody > tr').eq(row_index).find('td:eq(1)').text() + footnote;
            }
            var digitize_account2 = $('table > tbody > tr').eq(row_index).find('td:eq(3)').text() + " " + digitize_account;
            var amount2 = $('table > tbody > tr').eq(row_index).find('td:eq(5)').text();
            if ((amount2 != "") && (amount != "")) {
                alert("Cannot merge row please check data");
                //$("#tbResult").find("tr").removeClass("selected");
               // $(this).css("background-color", "white");
                $('table > tbody > tr').removeClass("rowOnCheck");
                return false;
            }
            else {
                amount2 = $('table > tbody > tr').eq(row_index).find('td:eq(5)').text() + amount;
            }
            tr.remove();
            document.getElementById("tbResult").deleteRow(row_index);
            var html =
                "<tr><td></td><td>" + footnote2 + "</td><td></td><td>" + digitize_account2 + "</td><td></td><td>"
                + amount2 + "</td><td>*</td><td></td></tr>";
            
            $('table > tbody > tr').eq(row_index).after(html);
           // $("#tbResult").find("td").removeClass("selected");
        });
        $("#BtnDelete").click(function () {
            tr.remove(); 
        });
        //Check --edit data
        $("#BtnCheck").click(function () {
            var filter = {
                docId: $("#hddocId").val(),
                pageType: $("#hdPageType").val()
            }
            $.ajax({
                url: "/ScanResult/AssignGridMerge",
                cash: false,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: ko.toJSON(filter),
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
            var filter = {
                docId: $("#hddocId").val(),
                pageType: $("#hdPageType").val()
            }
            //$("#tbResult").find("td").removeClass("selected");
            $.ajax({
                url: "/ScanResult/AssignGridMerge",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: ko.toJSON(filter),
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
        $(".Grid td:nth-child(3),th:nth-child(3)").toggle();
        $(".Grid td:nth-child(5),th:nth-child(5)").toggle();
        $(".Grid td:nth-child(4),th:nth-child(4)").width(160);
        $(".Grid td:nth-child(8),th:nth-child(8)").toggle();
        var row = $("#tbResult tbody tr:last-child").clone(true);
        $("#tbResult tbody tr").remove();
        $.each(model, function () {
            var financials = this;
            $("td", row).eq(1).html(financials.Footnote_No);
            $("td", row).eq(2).html('<select class="form-control input-sm">\n\
                <option value = "Low">Major0</option>\n\
                <option value="Normal">Major1</option>\n\
                <option value="High">Major2</option></select>');
            $("td", row).eq(3).html(financials.Digitized_Account_Title);
            $("td", row).eq(3).css("background-color", "#ffd800");
            $("td", row).eq(4).html('<select class="form-control input-sm">\n\
                <option value = "Low">direct costs</option>\n\
                <option value="Normal">gain on investment</option>\n\
                 <option value="Normal">increase in other receivables</option>\n\
                 <option value="Normal">state subsidy</option>\n\
                 <option value="Normal">indent sales</option>\n\
                <option value="High">other direct costs</option></select>');
            $("td", row).eq(4).css("background-color", "#ffd800");
            $("td", row).eq(5).html(financials.Amount);
            $("td", row).eq(6).html(financials.Modified);
            $("td", row).eq(7).html(financials.CLCTCD);
            $("#tbResult").append(row);
            row = $("#tbResult tbody tr:last-child").clone(true);
        });
    };
    function OnSuccessCancel(response) {
        var model = response;
        $(".Grid td:nth-child(3),th:nth-child(3)").hide();
        $(".Grid td:nth-child(5),th:nth-child(5)").hide();
        $(".Grid td:nth-child(8),th:nth-child(8)").hide();
        var row = $("#tbResult tbody tr:last-child").clone(true);
        $("#tbResult tbody tr").remove();
        $.each(model, function () {
            var financials = this;
            $("td", row).eq(1).html(financials.Footnote_No);
            $("td", row).eq(2).html(financials.Divisions);
            $("td", row).eq(3).html(financials.Digitized_Account_Title);
            $("td", row).eq(4).html(financials.Recovered);
            $("td", row).eq(5).html(financials.Amount);
            $("td", row).eq(6).html(financials.Modified);
            $("td", row).eq(7).html(financials.CLCTCD);
            $("#tbResult").append(row);
            row = $("#tbResult tbody tr:last-child").clone(true);
        });
        $('#tbResult tbody tr').each(function (idx) {
            $(this).children("td:eq(0)").html(idx + 1);
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
