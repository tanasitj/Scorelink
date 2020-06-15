function DocDetailModel(DocId, DocPageNo, FootnoteNo,PageType,UserId,date) {
    var self = this;
    self.DocId = ko.observable(DocId);
    self.DocPageNo = ko.observable(DocPageNo);
    self.FootnoteNo = ko.observable(FootnoteNo);
    self.PageType = ko.observable(PageType);
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
    GetDoclist();

    $(document).ready(function () {
<<<<<<< HEAD
        // Event click button select page
        $("#BtnSelectPage").click(function () {
            var docId = $("#hdId").val();
            var docPageNo = $("#pageNumber").val();
=======

        // Start when click button select page
        $("#BtnSelectPage").click(function () {
            //Getting value from to populate
            var docId = $("#hdId").val();
            var docPageNo = $("#pageNumber").val();
            //Getting value from to populate
>>>>>>> 50e9cee8fcccea4c621816df41e1484b6f448526
            var Footnotes = "Test Footnotes";
            var Kind_Of_Financial = $("input[name = 'Kind_of_Financial']:checked").val();
            var Type_Page;
            switch (Kind_Of_Financial) {
                case "Income Statement":
                    Type_Page = '1';
                    break;
                case "Balance Sheet":
                    Type_Page = '2';
                    break; 
                case "Cash Flow":
                    Type_Page = '3';
                    break; 
                case "Footnotes":
                    Type_Page = '4';
                    break;
<<<<<<< HEAD
                default:              
            }         
            var postData = {                
                'docId': docId,
                'docPageno': docPageNo,
                'pageType': Type_Page
=======
                default:
                    
            }
            
            //Send Value to Server Side.
            var postData = {
                
                'docId': docId,
                'docPageno': docPageNo,
                'pageType': Type_Page

>>>>>>> 50e9cee8fcccea4c621816df41e1484b6f448526
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
<<<<<<< HEAD
    //show page for deleting
    self.ClickDelete = function (data,event) {
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
        $("#exampleModalCenter").modal('show');
    }
    //delete page
    self.SubmitDelete = function () {
        var Type_Delete = $("input[name = 'TypeDelete']:checked").val();
        var page_no;
        switch (Type_Delete) {
            case "DeleteAllPages": page_no = '0';break;
            case "Delete_specific_page": page_no = $("#page_delete").val();break;
            default:
        }
        var Type_Page;
        switch (self.PageType) {case "Income Statement":Type_Page = '1';break;
            case "Balance Sheet": Type_Page = '2';break;
            case "Cash Flow": Type_Page = '3';break;
            case "Footnotes": Type_Page = '4';break;
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
                $("#exampleModalCenter").modal('hide');
                GetDoclist();
            }
        });
    }
    function GetDoclist() {
        var filter = {
            //filter docid,
            filterId: $("#hdId").val()
=======
    self.ClickDelete = function (data,event) {
        $.redirect("SelectPage/DeletePage", {
            'id': data.DocId(),
            'pagetype': data.PageType()
        }, "POST");
        //self.DocId(data.DocId());
        //self.PageType(data.PageType());
        ////alert(data.FilePath());
        //$("#exampleModalCenter").modal('show');

    }
    function GetDoclist() {
        //$('#table1').DataTable().clear();
        //$('#table1').DataTable().destroy();

        //blockUI();
        //---- Object for search ----
        var filter = {
            //filterId: self.FilterUserId,
            filterId: $("#hdId").val()
          // 'id': 14
>>>>>>> 50e9cee8fcccea4c621816df41e1484b6f448526
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
                            data.PageType
                        )
                    );
                });
<<<<<<< HEAD
            }
        })
=======

                unblockUI();
                //PNotification("Successful", "Insert data completed", "success");
            }
        })
        //.done(function () {
        //    var table = $('#table1');
        //    table.DataTable(
        //        {
        //            columnDefs: [
        //                { orderable: false, targets: 0 }
        //            ],
        //            bDestroy: true,
        //            pageLength: 10,
        //            "order": [[1, "asc"]]
        //        }
        //    );
        //})
        .fail(
            function (xhr, textStatus, err) {
                //PNotification("Error", err, "error");
                unblockUI();
            });

>>>>>>> 50e9cee8fcccea4c621816df41e1484b6f448526
    }
}
var viewModel = new ViewModel();
ko.applyBindings(viewModel);