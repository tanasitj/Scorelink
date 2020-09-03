function DocDetailModel(DocId, DocPageNo, FootnoteNo, PageType, PageTypeName, NoScan) {
    var self = this;
    self.DocId = ko.observable(DocId);
    self.DocPageNo = ko.observable(DocPageNo);
    self.FootnoteNo = ko.observable(FootnoteNo);
    self.PageType = ko.observable(PageType);
    self.PageTypeName = ko.observable(PageTypeName);
    self.CanEdit = ko.observable(NoScan > 0 ? false : true);
    self.CompletedFlag = ko.observable(NoScan > 0 ? false : true);
}
var ViewModel = function () {
    var self = this;
    self.DocDetail = ko.observableArray();
    self.DocId = ko.observable();
    self.DocPageNo = ko.observable();
    self.FootnoteNo = ko.observable();
    self.PageType = ko.observable();
    self.PageTypeName = ko.observable();
    self.CanEdit = ko.observable(false);
    self.CompletedFlag = ko.observable(false);

    GetDoclist();

    $(document).ready(function () {
        // Event click button select page
        $("#BtnSelectPage").click(function () {
            var docId = $("#hdId").val();
            var docPageNo = $("#pageNumber").val();
            var Footnotes = "Test Footnotes";
            var Kind_Of_Financial = $("input[name = 'Kind_of_Financial']:checked").val();
            var Type_Page;
            switch (Kind_Of_Financial) { case "Income Statement": Type_Page = '1';break;
                case "Balance Sheet": Type_Page = '2';break;
                case "Cash Flow": Type_Page = '3';break;
                case "Footnotes":Type_Page = '4';break;
                default:
            }
            var postData = {
                'docId': docId,
                'docPageno': docPageNo,
                'pageType': Type_Page
            };
            $.ajax({
                type: "POST",
                datatype: "json",
                contentType: "application/json; charset=utf-8",
                url: "/SelectPage/Get_SelectPage",
                data: ko.toJSON(postData),
                success: function (data) {
                    GetDoclist();
                },
                error: function (err) {
                    return Notification('Error', err.statusText, 'error');
                }
            });

        });

        $('#btnBack').click(function () {
            window.location.href = '/Upload/Index';
        });
    });
    //==================================================================================================
    //Event Button Click
    //show page for deleting
    self.ClickDelete = function (data, event) {
        self.DocId = data.DocId();
        self.PageType = data.PageType();
        var PageNo = data.DocPageNo(),
            xString = PageNo.split(',');
        array = [];
        array = array.concat(xString);
        $('#page_delete').empty();
        $.each(array, function (i, p) {
            $('#page_delete').append($('<option></option>').val(p).html(p));
        });
        $("#Modal_DeletePage").modal('show');
    }
    //delete page
    self.SubmitDelete = function () {
        var Type_Delete = $("input[name = 'TypeDelete']:checked").val();
        var page_no;
        switch (Type_Delete) {
            case "DeleteAllPages": page_no = '0'; break;
            case "Delete_specific_page": page_no = $("#page_delete").val(); break;
            default:
        }
        var filter = {
            'docId': self.DocId,
            'pageType': self.PageType,
            'docPageno': page_no
        }
        $.ajax({
            url: '/SelectPage/DeleteDocumentDetail',
            cache: false,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: ko.toJSON(filter),
            success: function (data) {
                $("#Modal_DeletePage").modal('hide');
                GetDoclist();
            }
        });
    }
    self.ClickScan = function (data, event) {
        var Type_Page = data.PageType();
        var DocId = data.DocId();
        var filter = {
            'docId': data.DocId(),
            'pageType': data.PageType()
        }
        blockUI();
        $.ajax({
            url: '/SelectPage/SelectScan',
            cache: false,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: ko.toJSON(filter),
            success: function (data) {
                //unblockUI();
                goSelectPattern(DocId, Type_Page);
            }
        });
    } 
    self.ClickScan_Edit = function (data, event) {
        $.redirect("/ScanResult/Index", {
            'docId': data.DocId(),
            'pageType': data.PageType(),
            'pagetypeName': data.PageTypeName()
        }, "POST");
    }
    $("#BtnExportAll").click(function () {
        var filter = {
            docId: $("#hdId").val()

        }
        $.ajax({
            url: "/ScanResult/ExportAllResult",
            cash: false,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: ko.toJSON(filter),
            success: function () {
                alert("Export data all result ready");
            }

        });
    });
    //================================================================================================
    // Functional using
    function ExportHTMLTableToExcel($table) {
        var tab_text = ""
        var final_text = "";
        var filename = $table.attr('export-excel-filename'); // attribute to be 
        // applied on Table tag
        filename = isNullOrUndefinedWithEmpty(filename) ? "AllResult" : filename;
        var index = $table.find("tbody tr").length;
        if (Number(index) > 0) {
            $.each($table, function (index, item) {
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
                    if (!$(this).hasClass("NoExport"))
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
                            var value = $(this).html();
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
                });

                // Create colum footer
                element.find("tfoot tr td").each(function () {
                    var colspan = $(this).attr("colspan");
                    var rowspan = $(this).attr("rowspan");

                    colspan = colspan == undefined ? 1 : colspan;
                    rowspan = rowspan == undefined ? 1 : rowspan;

                    if ($(this).hasClass("NoExport")) {
                        tab_text = tab_text + "";
                    }
                    else if ($(this).hasClass("ExportValueTD")) // Footer class that needs 
                    // to be no td that have input tags
                    {
                        $(this).find("input,select").each(function () {
                            var value = "";

                            if ($(this).prop("type") == 'select-one') {
                                value = $('option:selected', this).text();
                            } else {
                                value = $(this).val();
                            }

                            if (!$(this).closest("td").hasClass("NoExport") &&
                                !$(this).hasClass("NoExport")) {
                                tab_text = tab_text + "<td colspan=" + colspan + "rowspan = " + rowspan + " > " + value + "</th > ";
                            }
                        });
                    }
                    else
                        tab_text = tab_text + "<td colspan=" + colspan + "rowspan = " + rowspan + " > " + $(this).html() + "</td > ";
                });

                tab_text = tab_text + "<tr></tr></table>";

                if (index == 0) {
                    final_text = tab_text;
                }
                else {
                    final_text = final_text + tab_text;
                }
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
            else                 //other browser not tested on IE 11
            {
                //sa = window.open('data:application/vnd.ms-excel,' + 
                //         encodeURIComponent(final_text));
                var anchor = document.createElement('a');
                anchor.setAttribute('href', 'data:application/vnd.ms-excel,' +
                    encodeURIComponent(final_text));
                anchor.setAttribute('download', filename);
                anchor.style.display = 'none';
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
    function goSelectPattern(docId, pageType) {
        $.redirect("/SelectPattern/Index", {
            'docId': docId,
            'pageType': pageType
        }, "POST");
    }
    function GetDoclist() {
        var filter = {
            //filter docid,
            filterId: $("#hdId").val()
        }
        $.ajax({
            url: '/SelectPage/GetDocumentList',
            cache: false,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: ko.toJSON(filter),
            success: function (data) {
                self.DocDetail([]);
                ko.utils.arrayForEach(data, function (data) {
                    self.DocDetail.push(
                        new DocDetailModel(
                            data.DocId,
                            data.DocPageNo,
                            data.FootnoteNo,
                            data.PageType,
                            data.PageTypeName,
                            data.NoScan
                        )
                    );                   
                });
            }
        })
    }
}
var viewModel = new ViewModel();
ko.applyBindings(viewModel);