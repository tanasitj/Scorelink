var ViewModel = function () {
    var self = this;
    self.AttachFile1 = ko.observable(null);

    self.Upload = function () {
        $("#fileuploader").uploadFile({
            url: "/Upload/UploadFiles",
            fileName: "myfile"
        });
    }
}

var viewModel = new ViewModel();
ko.applyBindings(viewModel);

$(document).ready(function () {
    $("#fileuploader").uploadFile({
        url: "/Upload/UploadFiles",
        fileName: "myfile"
    });
});