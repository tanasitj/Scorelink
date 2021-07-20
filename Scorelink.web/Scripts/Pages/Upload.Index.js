﻿function DocInfoModel(id, fileUID, fileName, filePath, fileUrl, Language, UserId, date, Commited) {
    var self = this;
    self.DocId = ko.observable(id);
    self.FileUID = ko.observable(fileUID);
    self.FileName = ko.observable(fileName);
    self.FilePath = ko.observable(filePath);
    self.FileUrl = ko.observable(fileUrl);
    self.Language = ko.observable(Language);
    self.CreateBy = ko.observable(UserId);
    self.CreateDate = ko.observable(date);
    self.CanExport = ko.observable(Commited === 'Y' ? true : false);
}

var ViewModel = function () {
    var self = this;
    var docIds = [];
    self.AttachFile1 = ko.observable(null);
    self.DocumentInfo = ko.observableArray();
    self.DocId = ko.observable();
    self.FilePath = ko.observable();
    self.FileName = ko.observable();
    self.Language = ko.observable();
    self.CreateBy = ko.observable();
    self.CreateDate = ko.observable();
    self.FileUrl = ko.observable();
    self.Commited = ko.observable();

    GetDoclist();

    //-- Progress Bar --//
    var progress = document.getElementById("progress");
    var progress_wrapper = document.getElementById("progress_wrapper");
    var progress_status = document.getElementById("progress_status");

    var upload_btn = document.getElementById("upload_btn");
    var loading_btn = document.getElementById("loading_btn");
    var cancel_btn = document.getElementById("cancel_btn");

    var alert_wrapper = document.getElementById("alert_wrapper");

    var input = document.getElementById("fileUpload1");
    var file_input_label = document.getElementById("file_input_label");

    self.ClickUpload = function (event) {
        $('#table1').DataTable().clear();
        $('#table1').DataTable().destroy();
        readURL(document.getElementById("fileUpload1"), event);
    }

    self.ClickRefresh = function () {
        window.location = '/Upload/Index';
    }

    $("#select_all").change(function () {
        //alert('Test');
        //$("input[type=checkbox]").prop('checked', $(this).prop('checked'));
        $(".checkbox").prop('checked', $(this).prop("checked"));
    });

    $('.checkbox').change(function () {
        //uncheck "select all", if one of the listed checkbox item is unchecked
        if (false == $(this).prop("checked")) { //if this item is unchecked
            $("#select_all").prop('checked', false); //change "select all" checked status to false
        }
        //check "select all" if all checkbox items are checked
        if ($('.checkbox:checked').length == $('.checkbox').length) {
            $("#select_all").prop('checked', true);
        }
    });

    self.ClickDeleteAll = function () {
        /*$('#chkDocId:checked').each(function () {
            docIds.push(this.value);
        });*/

        $("#Modal-DeleteAll").modal('show');
    }

    self.SubmitDeleteAll = function () {

        $('#chkDocId:checked').each(function () {
            docIds.push(this.value);
        });

        //alert(docIds);
        var filter = {
            'id': docIds
        }

        $.ajax({
            url: '/Upload/DeleteAllDocumentInfo',
            cache: false,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: ko.toJSON(filter),
            success: function (data) {
                //if (!data) return PNotification("Failed", "Deleted failed", "error");

                //$('#table1').DataTable().clear();
                //$('#table1').DataTable().destroy();
                //PNotification("Successful", "Deleted completed", "success");
                //$("#Modal-DeleteAll").modal('hide');
                //GetDoclist();
                //$("#select_all").prop('checked', false);
                //window.location.href = '/Upload/Index';

                window.location = '/Upload/Index';
            },
        });
    }

    self.ClickScan = function (data, event) {
        $.redirect("/SelectPage/SelectPage", {
            'id': data.DocId()
        }, "POST");
    }

    self.ClickDelete = function (data, event) {
        self.DocId(data.DocId());
        self.FileName(data.FileName());
        self.Language(data.Language());
        self.CreateDate(data.CreateDate());
        self.FilePath(data.FilePath());
        //alert(data.FilePath());
        $("#Modal_DeletePage").modal('show');
    }

    self.ClickEdit = function (data, event) {
        self.DocId(data.DocId());
        self.FileName(data.FileName());
        self.Language(data.Language());
        self.CreateDate(data.CreateDate());
        self.FilePath(data.FilePath());
        //alert(data.FilePath());
        $("#Modal_EditPage").modal('show');
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
                $("#Modal_DeletePage").modal('hide');
                GetDoclist();
                //window.location.href = '/Upload/Index';
            }
        });
    }

    self.ClickExport = function (data, event) {
        var filter = {
            'docId': data.DocId()
        }

        $("#hdId").val(data.DocId());
        //alert(data.DocId());
        blockUI();
        $.ajax({
            url: "/Upload/ExportAllResult",
            cash: false,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: ko.toJSON(filter),
            error: function (xhr, status, error) {
                unblockUI();
            },
            success: function (data) {
                //get the file name for download
                window.location = '/Upload/Download?file=' + data + '&docId=' + $("#hdId").val();
                // alert("Export data all result already");
                window.setTimeout(unblockUI(), 5000);
            }
            
        })

        .fail(
        function (xhr, textStatus, err) {
            //PNotification("Error", err, "error");
            unblockUI();
        });
    };

    self.SubmitEditData = function () {
        var arg = {
            DocId: self.DocId,
            FileName: self.FileName,
            Language: self.Language,
            CreateBy: $("#hdUserId").val()
        }

        var filter = {
            'item': arg
        }

        $.ajax({
            url: '/Upload/EditDocumentInfo',
            cache: false,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: ko.toJSON(filter),
            success: function (data) {
                $("#Modal_EditPage").modal('hide');
                GetDoclist();
            }
        });
    }

    function GetDoclist() {


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
                            data.CreateDate,
                            data.Commited
                        )
                    );
                });
                $("#fileUpload1").val(null);
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
                    paging: true,
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

    function readURL(input, event) {
        
        if (!input.value) {
            show_alert("No file selected", "warning");
            return;
        }

        alert_wrapper.innerHTML = "";
        input.disabled = true;
        upload_btn.classList.add("d-none");
        loading_btn.classList.remove("d-none");
        //cancel_btn.classList.remove("d-none");
        progress_wrapper.classList.remove("d-none");

        //var loaded = event.loaded;
        //var total = event.total;
        //var percentage_complete = (loaded / total) * 100;

        //progress.setAttribute("style", 'width: ${Math.floor(percentage_complete)}%');
        //progress.style.width = Math.floor(percentage_complete);
        //progress_status.innerText = Math.floor(percentage_complete) + '% uploaded';

        if (input.files && input.files[0]) {
            var fileUpload = $("#fileUpload1").get(0);
            var files = fileUpload.files;

            // Create FormData object  
            var fileData = new FormData();

            fileData.append("userId", $("#hdUserId").val());
            fileData.append("language", $("#txtLanguage").val());

            // Looping over all files and add it to FormData object  
            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);
            }

            $.ajax({
                url: '/Upload/UploadFiles',
                type: "POST",
                xhr: function () {
                    var xhr = new window.XMLHttpRequest();

                    //Upload progress
                    xhr.upload.addEventListener("progress", function (evt) {
                        if (evt.lengthComputable) {
                            var percentComplete = (evt.loaded / evt.total) * 100;

                            //$("#ctlProgress").val(percentComplete * 100);
                            progress.setAttribute("style", 'width: ' + Math.floor(percentComplete) + '%');
                            progress.style.width = Math.floor(percentComplete);
                            progress_status.innerText = Math.floor(percentComplete) + '% uploaded';
                        }

                    }, false);

                    //Download progress
                    xhr.addEventListener("progress", function (evt) {
                        if (evt.lengthComputable) {
                            var percentComplete = evt.loaded / evt.total;
                            //Do something with download progress
                            console.log(percentComplete);
                        }

                    }, false);
                    return xhr;
                },
                contentType: false, // Not to set any content header  
                processData: false, // Not to process data  
                data: fileData,
                success: function (result) {
                    input.value = null;
                    input.disabled = false;
                    loading_btn.classList.add("d-none");
                    //cancel_btn.classList.add("d-none");
                    upload_btn.classList.remove("d-none");
                    GetDoclist();
                },
                done: function () {
                    GetDoclist();
                }
            });
        }
        //}
    }

    function show_alert(message, alert) {
        alert_wrapper.innerHTML = "<div class='alert alert-" + alert + " alert-dismissible fade show' role='alert' ><span>" + message + "</span><button type='button' class='btn-close' data-bs-dismiss='alert' aria-label='Close' ></button ></div >";
    };

    function input_filename() {
        file_input_label.innerText = input.files[0].name;
    }

    function reset() {
        input.value = null;
        input.disabled = false;
        cancel_btn.classList.add("d-none");
        loading_btn.classList.add("d-none");
        upload_btn.classList.remove("d-none");
        progress_wrapper.classList.add("d-none");
        pregress.setAttribute("style", "width: 0%");
        file_input_label.innerText = "Select file";
    }

    function abort() {
        reset();
        show_alert('Upload cancelled', "primary");
    }

    self.Uploadxx = function upload(url) {

        

        var data = new FormData();
        var request = new XMLHttpRequest();
        request.responseType = "json";
        

        var file = input.files[0];
        var filename = file.name;
        var filesize = file.size;

        document.cookie = 'filesize=$(filesize)';
        data.append("file", file);

        /*request.upload.addEventListener("progress", function (e) {
            var loaded = e.loaded;
            var total = e.total;
            var percentage_complete = (loaded / total) * 100;
            progress.setAttribute("style", 'width: ${Math.floor(percentage_complete)}%');
            progress_status.innerText = '${Math.floor(percentage_complete)}% uploaded';
        })*/

        request.addEventListener("load", function (e) {
            if (request.status == 200) {
                show_alert('File Uploaded', "success");
            }
            else {
                show_alert('Error uploading file', "danger");
            }
            reset();
        })

        request.addEventListener("error", function (e) {
            reset();
            show_alert('Error uploading file', "danger");
        })

        

        request.open("post", url);
        request.send(data);

        cancel_btn.addEventListener("click", function () {
            request.abort();
        })

        

    }

}

var viewModel = new ViewModel();
ko.applyBindings(viewModel);