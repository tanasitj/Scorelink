var ViewModel = function () {
    var self = this;
    self.ClickBack = function () {
        window.location.href = '/Admin/Index';
    }
    self.ClickSave = function () {
        window.location.href = '/Admin/Index';
    }
}
var viewModel = new ViewModel();
ko.applyBindings(viewModel);

$("#btnLogIn").click(function () {
    window.location.href = "/UserProfile/Index";
});