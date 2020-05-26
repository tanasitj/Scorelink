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

    GetDoclist();

    function GetDoclist() {
        $('#tab1').DataTable().clear();
        $('#tab1').DataTable().destroy();

        blockUI();

        //---- Object for search ----
        var filter = {
            //filterId: self.FilterUserId,
            filterId: "48"
        }

        $.ajax({
            url: '/SelectArea/GetDocumentList',
            cache: false,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: ko.toJSON(filter),
            success: function (data) {
                self.DocumentDetail([]);
                ko.utils.arrayForEach(data, function (data) {
                    self.DocumentDetail.push(
                        new DocDetailModel(
                            data.DocDetId,
                            data.DocId,
                            data.DocPageNo,
                            data.FootnoteNo,
                            data.PageType,
                            data.ScanStatus,
                            data.PageFileName,
                            data.PagePath,
                            data.Selected,
                            data.PatternNo,
                            data.CreateBy,
                            data.CreateDate,
                            data.UpdateDate
                        )
                    );
                });

                unblockUI();
                //PNotification("Successful", "Upload completed", "success");
            }
        })
        .done(function () {
            var table = $('#tab1');
            table.DataTable(
                {
                    columnDefs: [
                        { orderable: false, targets: 0 }
                    ],
                    bDestroy: true,
                    pageLength: 1,
                    "order": [[1, "asc"]]
                }
            );
        })
        .fail(function (xhr, textStatus, err) {
            //PNotification("Error", err, "error");
            unblockUI();
        });

    }

    $(document).ready(function () {

        $('img#view').selectAreas({
            minSize: [10, 10],
            onChanged: debugQtyAreas,
            width: 600,
            //areas: [
            //    {
            //        x: 10,
            //        y: 20,
            //        width: 60,
            //        height: 100,
            //    }
            //]
        });

        //---- New ----//
        $('#btnSubmit').click(function () {
            var i = $("#hfdId").val() * 1;
            //alert(i);
            var sImg = "../Temp/Copy_00" + i + ".jpg";
            var sFileName = "Copy_00" + i + ".jpg";
            //output("")

            var a = 0;
            var aArea = new Array();
            var areas = $('img#view').selectAreas('relativeAreas');

            $.each(areas, function (id, area) {
                aArea[a] = areaToSave(area);
                a = a + 1;
            });

            //Send Value to Server Side.
            var postData = {
                'values': aArea,
                'fileName': sFileName
            };

            $.ajax({
                type: "POST",
                datatype: "json",
                contentType: "application/json; charset=utf-8",
                url: "/Digitize/SaveArea",
                //JSON.stringify(postData);
                data: JSON.stringify(postData),
                success: function (data) {
                    i = i + 1;
                    $('#txtPage').text("Page " + i);
                    $("#hfdId").val(i);
                    sImg = "../Temp/Copy_00" + i + ".jpg";
                    $('img#view').attr("src", sImg);
                    $('img#view').selectAreas('reset');
                    alert("Save completed");
                },
                dataType: "json",
                traditional: true
            })
        });

        function areaToSave(area) {
            return (typeof area.id === "undefined" ? "" : (area.id + "|")) + area.x + '|' + area.y + '|' + area.width + '|' + area.height
        }

        //------------//


        $('#btnView').click(function () {
            var areas = $('img#view').selectAreas('areas');
            displayAreas(areas);
        });
        $('#btnViewRel').click(function () {
            var areas = $('img#view').selectAreas('relativeAreas');
            displayAreas(areas);
        });
        $('#btnReset').click(function () {
            output("reset")
            $('img#view').selectAreas('reset');
        });
        $('#btnDestroy').click(function () {
            $('img#view').selectAreas('destroy');

            output("destroyed")
            $('.actionOn').attr("disabled", "disabled");
            $('.actionOff').removeAttr("disabled")
        });
        $('#btnCreate').attr("disabled", "disabled").click(function () {
            $('img#view').selectAreas({
                minSize: [10, 10],
                onChanged: debugQtyAreas,
                width: 500,
            });

            output("created")
            $('.actionOff').attr("disabled", "disabled");
            $('.actionOn').removeAttr("disabled")
        });
        $('#btnNew').click(function () {
            var areaOptions = {
                x: Math.floor((Math.random() * 200)),
                y: Math.floor((Math.random() * 200)),
                width: Math.floor((Math.random() * 100)) + 50,
                height: Math.floor((Math.random() * 100)) + 20,
            };
            output("Add a new area: " + areaToString(areaOptions))
            $('img#view').selectAreas('add', areaOptions);
        });
        $('#btnNews').click(function () {
            var areaOption1 = {
                x: Math.floor((Math.random() * 200)),
                y: Math.floor((Math.random() * 200)),
                width: Math.floor((Math.random() * 100)) + 50,
                height: Math.floor((Math.random() * 100)) + 20,
            }, areaOption2 = {
                x: areaOption1.x + areaOption1.width + 10,
                y: areaOption1.y + areaOption1.height - 20,
                width: 50,
                height: 20,
            };
            output("Add a new area: " + areaToString(areaOption1) + " and " + areaToString(areaOption2))
            $('img#view').selectAreas('add', [areaOption1, areaOption2]);
        });
    });
}

var viewModel = new ViewModel();
ko.applyBindings(viewModel);