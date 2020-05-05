
var ViewModel = function () {
    var self = this;
    self.AttachFile1 = ko.observable(null);

    self.Upload = function () {
        readURL(document.getElementById("fileUpload1"));
    }

    function readURL(input) {
        if (input.files && input.files[0]) {
            //if (!isImage(input.files[0])) return alert('Allow only IMAGE file!');//PNotification('Error', 'Allow only IMAGE file!', 'error');
            //if (input.files[0].size > 2097152) return alert('Maximun file size 2 Mb!');//PNotification('Error', 'Maximun file size 2 Mb!', 'error');

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

            $.ajax({
                url: '/Upload/UploadFiles',
                type: "POST",
                contentType: false, // Not to set any content header  
                processData: false, // Not to process data  
                data: fileData,
                success: function (result) {
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