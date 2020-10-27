
﻿function DocInfoModel(id, fileUID, fileName, filePath, fileUrl, Language, UserId, date) {
    var self = this;
    self.DocId = ko.observable(id);
    self.FileUID = ko.observable(fileUID);
    self.FileName = ko.observable(fileName);
    self.FilePath = ko.observable(filePath);
    self.FileUrl = ko.observable(fileUrl);
    self.Language = ko.observable(Language);
    self.CreateBy = ko.observable(UserId);
    self.CreateDate = ko.observable(date);
}

var ViewModel = function () {
    var self = this;
    self.AttachFile1 = ko.observable(null);
    self.DocumentInfo = ko.observableArray();
    self.DocId = ko.observable();
    self.FilePath = ko.observable();
    self.FileUrl = ko.observable();

    GetDoclist();

    self.Upload = function () {
        readURL(document.getElementById("fileUpload1"));
        GetDoclist();
        //PNotification("Successful", "Upload completed", "success");
    }

    self.ClickScan = function (data, event) {
        $.redirect("/SelectPage/SelectPage", {
            'id': data.DocId()
        }, "POST");
    }

    self.ClickDelete = function (data, event) {
        self.DocId(data.DocId());
        self.FilePath(data.FilePath());
        //alert(data.FilePath());
        $("#modal-default").modal('show');
    }

    self.SubmitDeleteData = function () {
        var filter = {
            'id': self.DocId()
        }

        $.ajax({
            url: '/Upload/DeleteDocumentInfo',
            cache: false,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: ko.toJSON(filter),
            success: function (data) {
                //if (!data) return PNotification("Failed", "Deleted failed", "error");

                //PNotification("Successful", "Deleted completed", "success");
                $("#modal-default").modal('hide');
                GetDoclist();
                //window.location.href = '/Upload/Index';
            }
        });
    }

    function GetDoclist() {
        $('#table1').DataTable().clear();
        $('#table1').DataTable().destroy();

        blockUI();

        //---- Object for search ----
        var filter = {
            //filterId: self.FilterUserId,
            filterId: $("#hdUserId").val()
        }

        $.ajax({
            url: '/Upload/GetDocumentList',
            cache: false,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: ko.toJSON(filter),
            success: function (data) {
                self.DocumentInfo([]);
                ko.utils.arrayForEach(data, function (data) {
                    self.DocumentInfo.push(
                        new DocInfoModel(
                            data.DocId,
                            data.FileUID,
                            data.FileName,
                            data.FilePath,
                            data.FileUrl,
                            data.Language,
                            data.CreateBy,
                            data.CreateDate
                        )
                    );
                });
                
                unblockUI();
                //PNotification("Successful", "Upload completed", "success");
            }
        })
        .done(function () {
            var table = $('#table1');
            table.DataTable(
                {
                    columnDefs: [
                        { orderable: false, targets: 0 }
                    ],
                    bDestroy: true,
                    pageLength: 10,
                    "order": [[1, "asc"]]
                }
            );
        })
        .fail(
            function (xhr, textStatus, err) {
                //PNotification("Error", err, "error");
                unblockUI();
        });

    }

    function readURL(input) {
        if (input.files && input.files[0]) {

            var reader = new FileReader();

            reader.onload = function (e) {
                if (input === document.getElementById('fileUpload1')) {
                    self.AttachFile1(input.files[0].name);
                    //$('#img1').attr('src', e.target.result);
                }
            };

            reader.readAsDataURL(input.files[0]);
        }

        if (self.AttachFile1()) {
            var fileUpload = $("#fileUpload1").get(0);
            var files = fileUpload.files;

            // Create FormData object  
            var fileData = new FormData();

            // Looping over all files and add it to FormData object  
            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);
            }

            fileData.append("userId", $("#hdUserId").val());
            fileData.append("language", $("#txtLanguage").val());

            $.ajax({
                url: '/Upload/UploadFiles',
                type: "POST",
                contentType: false, // Not to set any content header  
                processData: false, // Not to process data  
                data: fileData,
                success: function (result) {
                    GetDoclist();
                },
                error: function (err) {
                    return Notification('Error', err.statusText, 'error');
                }
            });
        }
        
    }

}

var viewModel = new ViewModel();
ko.applyBindings(viewModel);