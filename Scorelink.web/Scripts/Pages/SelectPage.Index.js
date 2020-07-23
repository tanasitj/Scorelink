function DocDetailModel(DocId, DocPageNo, FootnoteNo, PageType,PageTypeName,UserId, date) {
    var self = this;
    self.DocId = ko.observable(DocId);
    self.DocPageNo = ko.observable(DocPageNo);
    self.FootnoteNo = ko.observable(FootnoteNo);
    self.PageType = ko.observable(PageType);
    self.PageTypeName = ko.observable(PageTypeName);
    self.CreateBy = ko.observable(UserId);
    self.CreateDate = ko.observable(date);
}
var ViewModel = function () {
    var self = this;
    self.DocDetail = ko.observableArray();
    self.DocId = ko.observable();
    self.DocPageNo = ko.observable();
    self.FootnoteNo = ko.observable();
    self.PageType = ko.observable();
    self.PageTypeName = ko.observable();
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
    });
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
        var Type_Page;
        switch (self.PageType) {
            case "Income Statement": Type_Page = '1'; break;
            case "Balance Sheet": Type_Page = '2'; break;
            case "Cash Flow": Type_Page = '3'; break;
            case "Footnotes": Type_Page = '4'; break;
            default:
        }
        var filter = {
            'docId': self.DocId,
            'pageType': Type_Page,
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

    function goSelectPattern(docId,pageType) {
        $.redirect("/SelectPattern/Index", {
            'docId': docId,
            'pageType': pageType
        }, "POST");
    }
    
    self.ClickScan_Edit = function (data, event) {
        $.redirect("/ScanResult/Index", {
            'docId': data.DocId(),
            'pageType': data.PageType()
        }, "POST");
       
        //wait input parameter docId and page type ***********************
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
                            data.PageTypeName
                        )
                    );
                });
            }
        })
    }
}
var viewModel = new ViewModel();
ko.applyBindings(viewModel);