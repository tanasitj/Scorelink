function DocDetailModel(DocDetId, DocId, DocPageNo, FootnoteNo, PageType, ScanStatus, PageFileName, PagePath, Selected, PatternNo, CreateBy, CreateDate, UpdateDate) {
    var self = this;
    self.DocDetId = ko.observable(DocDetId);
    self.DocId = ko.observable(DocId);
    self.DocPageNo = ko.observable(DocPageNo);
    self.FootnoteNo = ko.observable(FootnoteNo);
    self.PageType = ko.observable(PageType);
    self.ScanStatus = ko.observable(ScanStatus);
    self.PageFileName = ko.observable(PageFileName);
    self.PagePath = ko.observable(PagePath);
    self.Selected = ko.observable(Selected);
    self.PatternNo = ko.observable(PatternNo);
    self.CreateBy = ko.observable(CreateBy);
    self.CreateDate = ko.observable(CreateDate);
    self.UpdateDate = ko.observable(UpdateDate);
}

var ViewModel = function () {
    var self = this;
    self.DocumentDetail = ko.observableArray();

    $('img#view').attr("src", $("#hdPagePath").val());
    $('img#view').selectAreas({
        minSize: [10, 10],
        onChanged: debugQtyAreas,
        width: 600
    });

    function areaToSave(area) {
        return (typeof area.id === "undefined" ? "" : (area.id + "|")) + area.x + '|' + area.y + '|' + area.width + '|' + area.height
    }

    // Log the quantity of selections
    function debugQtyAreas(event, id, areas) {
        console.log(areas.length + " areas", arguments);
    }

    function SetNextPage() {
        $.ajax({
            url: '/SelectArea/',
            cache: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                self.Departments([]);
                self.Departments(data);
            }
        });
    }

    $(document).ready(function () {
        $('img#view').attr("src", $("#hdPagePath").val());
        //---- New ----//
        $('#btnSubmit').click(function () {
            $('img#view').attr("src", $("#hdPagePath").val());
            var a = 0;
            var aArea = new Array();
            var areas = $('img#view').selectAreas('relativeAreas');

            var docId = $("#hdId").val();
            var docDetId = $("#hdDocDetId").val();
            var pageFileName = $("#hdPageFileName").val();

            $.each(areas, function (id, area) {
                aArea[a] = areaToSave(area);
                a = a + 1;
            });

            //Send Value to Server Side.
            var postData = {
                'values': aArea,
                'docId': docId,
                'docDetId': docDetId,
                'pageFileName': pageFileName
            };

            $.ajax({
                type: "POST",
                datatype: "json",
                contentType: "application/json; charset=utf-8",
                url: "/SelectArea/SaveArea",
                //JSON.stringify(postData);
                data: JSON.stringify(postData),
                success: function (data) {
                    $("#hdId").val(data.DocId);
                    $("#hdDocDetId").val(data.DocDetId);
                    $("#hdDocPageNo").val(data.DocPageNo);
                    $("#hdPageFileName").val(data.PageFileName);
                    $("#hdPagePath").val(data.PagePath);

                    $('img#view').attr("src", data.PagePath);
                    $('img#view').selectAreas('reset');
                    //alert("Save completed");
                },
                dataType: "json",
                traditional: true
            })
        });

        //------------//

    });
}

var viewModel = new ViewModel();
ko.applyBindings(viewModel);