var ViewModel = function () {
    var self = this;
    self.ClickEdit = function () {
        window.location.href = '/Admin/EditUserProfile';
    }
    self.ClickSearch = function () {
        window.location.href = '/Admin/Index';
    }
}

var viewModel = new ViewModel();
ko.applyBindings(viewModel);

$(function () {
    $('#example2').DataTable({
        "paging": true,
        "lengthChange": false,
        "searching": true,
        "ordering": true,
        "info": true,
        "autoWidth": false,
        "columnDefs": [{ orderable: false, targets: [8,9,10] }],
        //"order": [[1,"asc"]],
    });
});