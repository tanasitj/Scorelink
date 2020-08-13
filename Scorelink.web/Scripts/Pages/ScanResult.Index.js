function ScanEditModel(Footnote_No, Divisions, Digitized_Account_Title, Recovered,Standard_Title, Amount, Modified, CLCTCD) {
    var self = this;
    self.Footnote_No = ko.observable(Footnote_No);
    self.Divisions = ko.observable(Divisions);
    self.Digitized_Account_Title = ko.observable(Digitized_Account_Title);
    self.Recovered = ko.observable(Recovered);
    self.Standard_Title = ko.observable(Standard_Title);
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
    self.Standard_Title = ko.observable();
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
            tr = $(this).closest("tr").find("td").toggleClass("row_selected");
            //tr = $(this).closest("tr").find("td").addClass("selected");
            footnote = $(this).closest("tr").find('td:eq(1)').text();
            digitize_account = $(this).closest("tr").find('td:eq(3)').text();
            amount = $(this).closest("tr").find('td:eq(6)').text();
            modified = $(this).closest("tr").find('td:eq(7)').text();   
        });
        $('td').click(function () {
            row_index = $(this).parent().index();
            var col_index = $(this).index();
        });
       
        // Insert Row
        $("#BtnInsert").click(function () {
            $('#tbResult').append('<tr><td>text</td><td>text</td><td>text</td><td>text</td><td>text</td><td>text</td><td>text</td><td>text</td></tr>');
            $('#Table tbody:last').append(newRow);
            $("#BtnCheck").attr("disabled", true);
            $("#BtnMerge").attr("disabled", true);
            $("#BtnExport").attr("disabled", true);
            $("#BtnDelete").attr("disabled", true);
            $("#Commit").attr("disabled", true);
        });
        $("#BtnMerge").click(function () {

            var footnote2 = $('table > tbody > tr').eq(row_index).find('td:eq(1)').text();
            if ((footnote2 != "") && (footnote != "")) {
                footnote2 = $('table > tbody > tr').eq(row_index).find('td:eq(1)').text() + "," + footnote;
            }
            else {
                footnote2 = $('table > tbody > tr').eq(row_index).find('td:eq(1)').text() + footnote;
            }
            var digitize_account2 = $('table > tbody > tr').eq(row_index).find('td:eq(3)').text() + " " + digitize_account;
            var amount2 = $('table > tbody > tr').eq(row_index).find('td:eq(6)').text();
            if ((amount2 != "") && (amount != "")) {
                alert("Cannot merge row please check data");
                $('table > tbody > tr').removeClass("rowOnCheck");
                return false;
            }
            else {
                amount2 = $('table > tbody > tr').eq(row_index).find('td:eq(6)').text() + amount;
            }
            tr.remove();
            document.getElementById("tbResult").deleteRow(row_index);
            var html =
                "<tr><td></td><td>" + footnote2 + "</td><td></td><td>" + digitize_account2 + "</td><td></td><td></td><td>"
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
                url: "/ScanResult/AssignGrid",
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
            $("#tbResult").find("td").removeClass("selected");
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
        $("#BtnBack").click(function () {
            $.redirect("/SelectPage/SelectPage", {
                'id': $("#hddocId").val()
            }, "POST");
        });
        $("#BtnCommit").click(function () {

            var $table = $("#tbResult");
            var filename = "Tmp" + "00" + $("#hdPageType").val() + ".csv";
            var sheetname = $("#hdPageTypeName").val();
            var csv_data = exportTableToCSV($table, filename);
            var temp_data = {
                docId: $("#hddocId").val(),
                csv_file: csv_data,
                filenames: filename
            }
            //console.log(csv_file);
            $.ajax({
                url: "/ScanResult/Commit_file",
                cash: false,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: ko.toJSON(temp_data),
                success: function () {
                    alert("Commit All Readdy");
                    $.redirect("/SelectPage/SelectPage", {
                        'id': $("#hddocId").val()
                    }, "POST");
                }
            });
       });
        $("#BtnExport").click(function () {
            var table = $("#tbResult");
            var filename = "Result";
            var sheetname = $("#hdPageTypeName").val();
            ExportHTMLTableToExcel(table, sheetname, filename);
        });
    });
    function exportTableToCSV($table, filename) {
        var tab_text = "";
        var final_text = "";
        filename = isNullOrUndefinedWithEmpty(filename) ? "Result_data" : filename;
        var index = $table.find("tbody tr").length;
        if (Number(index) > 0) {
            $.each($table, function (index, item) {
                var element = $(item);
                var headertext = $("#" + element[0].id).closest
                    (":has(label.HeaderLabel)").find('label').text().trim();
                if (headertext == "") {
                    tab_text = "";
                }
                else {
                    tab_text = headertext;
                }
                // Create column header
                element.find("thead tr th").each(function () {
                    if (!$(this).hasClass("NoExport"))
                        //tab_text = tab_text +
                        //    $(this)[0].innerHTML + "</td>";
                    var txt = $(this).text();
                    if (txt.indexOf(',') == 0 || txt.indexOf('\"') == 0 || txt.indexOf('\n') == 0) {
                        txt = "\"" + txt.replace(/\"/g, "\"\"") + "\"";
                    }
                    tab_text += txt + ",";
                });
                tab_text += '\n';
                // Create body column
                element.find(" tbody tr").each(function () {
                    
                    $(this).find("td").each(function () {
                        
                        if ($(this).hasClass("text-center")) {
                           // tab_text += ",";
                            var txt = $(this).text();
                            if (txt.indexOf(',') >= 0 || txt.indexOf('\"') >= 0 || txt.indexOf('\n') >= 0) {
                                txt = "\"" + txt.replace(/\"/g, "\"\"") + "\"";
                            }
                            tab_text += txt + ",";
                        }
                        else if ($(this).hasClass("dropdown")) {
                            //tab_text += ",";
                            $(this).find("select").each(function () {
                                var txt = "";
                                if ($(this).prop("type") == 'select-one') {
                                    txt = $('option:selected', this).text();
                                    if (txt.indexOf(',') >= 0 || txt.indexOf('\"') >= 0 || txt.indexOf('\n') >= 0) {
                                        txt = "\"" + txt.replace(/\"/g, "\"\"") + "\"";
                                    }
                                } else {
                                    txt = $(this).val();
                                    if (txt.indexOf(',') >= 0 || txt.indexOf('\"') >= 0 || txt.indexOf('\n') >= 0) {
                                        txt = "\"" + txt.replace(/\"/g, "\"\"") + "\"";
                                    }
                                }
                                tab_text += txt + ",";
                            });
                        }
                        else {}
                    });
                    tab_text += '\n';
                     
                });
            });
             
        } // end if
        return tab_text;
    }

    function ExportHTMLTableToExcel(table,sheetName,filename) {
        var tab_text = ""
        var final_text = "";
        filename = isNullOrUndefinedWithEmpty(filename) ? "Result_data" : filename;
        var index = table.find("tbody tr").length;
        if (Number(index) > 0) {
            $.each(table, function (index, item) {
                var element = $(item);
                var headertext = $("#" + element[0].id).closest
                    (":has(label.HeaderLabel)").find('label').text().trim();
                if (headertext == "") {
                    tab_text = "<table border='2px'><tr>";
                }
                else {
                    tab_text = "<table border='2px'><tr> " + headertext + "</tr><tr>";
                }
                // Create column header
                element.find("thead tr th").each(function () {
                    if (!$(this).hasClass("text"))
                        tab_text = tab_text + "<td bgcolor='#87AFC6'>" +
                            $(this)[0].innerHTML + "</td>";
                });
                //Close column header
                tab_text = tab_text + "</tr>";
                // Create body column
                element.find(" tbody tr").each(function () {
                    tab_text = tab_text + "<tr>";
                    $(this).find("td").each(function () {
                        if ($(this).hasClass("text-center")) {
                            var value = $(this).text();                     
                            tab_text = tab_text + "<th>" + value + "</th>";                      
                        }
                        else {
                            $(this).find("select").each(function () {
                                var value = "";
                                if ($(this).prop("type") == 'select-one') {
                                    value = $('option:selected', this).text();
                                } else {
                                   value = $(this).val();
                                }
                                tab_text = tab_text + "<th>" + value + "</th>";
                            });
                        }
                    });
                    tab_text = tab_text + "</tr>";
                    if (index == 0) {
                        final_text = tab_text;
                    }
                    else {
                        final_text = final_text + tab_text;
                    }
                });
            });          
            var ua = window.navigator.userAgent;
            var msie = ua.indexOf("MSIE ");
            if (msie > 0 || !!navigator.userAgent.match
                (/Trident.*rv\:11\./))      // If Internet Explorer
            {
                txtArea1 = window.open();
                txtArea1.document.open("txt/html", "replace");
                txtArea1.document.write(final_text);
                txtArea1.document.close();
                txtArea1.focus();
                sa = txtArea1.document.execCommand("SaveAs", true, filename + ".xlsx");
                return (sa);
            }
            else  //other browser not tested on IE 11
            {
                //----------------------------------------------------------------
                //if (!table.nodeType)
                //    table = document.getElementById(table);
                var ctx = { worksheet: sheetName || 'Worksheet', table: table.innerHTML };
                var anchor = document.createElement('a');
                //var base64file = "base64," + $.base64.encode(final_text);
                anchor.setAttribute('href', 'data:application/vnd.ms-excel,' +  encodeURIComponent(final_text));
                //anchor.setAttribute('href', 'data:application/vnd.ms-excel,' + 'filename =' + $filename.excel, + base64file)
                  
                //-----------------------------------------------------------------
                anchor.setAttribute('download', filename);
                //anchor.style.display = 'none';
                document.body.appendChild(anchor);
                anchor.click();
                document.body.removeChild(anchor);
            }
        }
    }

    function isNullOrUndefinedWithEmpty(text) {
        if (text == undefined)
            return true;
        else if (text == null)
            return true;
        else if (text == null)
            return true;
        else
            false;
    }
    function OnSuccessCheck(response) {
        var model = response;
        $(".Grid td:nth-child(3),th:nth-child(3)").toggle();
        $(".Grid td:nth-child(5),th:nth-child(5)").toggle();
        $(".Grid td:nth-child(9),th:nth-child(9)").toggle();
        var row = $("#tbResult tbody tr:last-child").clone(true);
        $("#tbResult tbody tr").remove();
        $.each(model, function () {
            var financials = this;
            $("td", row).eq(1).html(financials.Footnote_No);
            $("td", row).eq(2).html('<select name = "Combo" id = "Combo" class="form-control input-sm">\n\
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
            $("td", row).eq(5).html(financials.Standard_Title);
            $("td", row).eq(6).html(financials.Amount);
            $("td", row).eq(7).html(financials.Modified);
            $("td", row).eq(8).html(financials.CLCTCD);
            $("#tbResult").append(row);
            row = $("#tbResult tbody tr:last-child").clone(true);
        });
        $('#tbResult tbody tr').each(function (row_num) {
            $(this).children("td:eq(0)").html(row_num + 1);
        });
    };
    function OnSuccessCancel(response) {
        var model = response;
        $(".Grid td:nth-child(2),th:nth-child(2)").hide();
        $(".Grid td:nth-child(4),th:nth-child(4)").hide();
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
            $("td", row).eq(5).html(financials.Standard_Title);
            $("td", row).eq(6).html(financials.Amount);
            $("td", row).eq(7).html(financials.Modified);
            $("td", row).eq(8).html(financials.CLCTCD);
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
            url: '/ScanResult/AssignGrid',
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
                            data.Standard_Title,
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
