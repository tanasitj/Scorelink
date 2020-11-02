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
    var sImgPath = $("#hdPagePath").val();
    $('img#view').attr("src", sImgPath);

    var area1 = {};
    var area2 = {};
    var area3 = {};
    if ($("#hdPatternNo").val() == "2" || $("#hdPatternNo").val() == "3") { var area4 = {}; }
    if ($("#hdPatternNo").val() == "3") { var area5 = {}; }

    setAreaValue($("#hdPatternNo").val());

    if ($("#hdPatternNo").val() == "2" || $("#hdPatternNo").val() == "3") {
        $('img#view').selectAreas({
            allowSelect: false,
            allowDelete: false,
            minSize: [10, 10],
            onChanged: debugQtyAreas,
            width: 600,
            areas: [area1, area2, area3, area4]
        });
    } else if ($("#hdPatternNo").val() == "3") {
        $('img#view').selectAreas({
            allowSelect: false,
            allowDelete: false,
            minSize: [10, 10],
            onChanged: debugQtyAreas,
            width: 600,
            areas: [area1, area2, area3, area4, area5]
        });
    } else {
        $('img#view').selectAreas({
            allowSelect: false,
            allowDelete: false,
            minSize: [10, 10],
            onChanged: debugQtyAreas,
            width: 600,
            areas: [area1, area2, area3]
        });
    }




    $(document).ready(function () {
        $('img#view').attr("src", sImgPath);

        $('#btnBack').click(function () {
            $.redirect("/SelectPage/SelectPage", {
                'id': $("#hdId").val()
            }, "POST");
        });

        //---- New ----//
        $('#btnSubmit').click(function () {
            blockUI();
            var a = 0;
            var aArea = new Array();
            var areas = $('img#view').selectAreas('relativeAreas');

            var docId = $("#hdId").val();
            var docDetId = $("#hdDocDetId").val();
            var pageType = $("#hdPageType").val();
            var pageFileName = $("#hdPageFileName").val();

            $.each(areas, function (id, area) {
                //Check Pattern
                if ($("#hdPatternNo").val() == "5") {
                    if (a < 2) {
                        aArea[a] = areaToSave(area);
                    }
                } else {
                    aArea[a] = areaToSave(area);
                }
                a = a + 1;
            });

            //Send Value to Server Side.
            var postData = {
                'values': aArea,
                'docId': docId,
                'docDetId': docDetId
            };

            $.ajax({
                type: "POST",
                datatype: "json",
                contentType: "application/json; charset=utf-8",
                url: "/SelectArea/SaveArea",
                data: JSON.stringify(postData),
                success: function (data) {
                    unblockUI();
                    if (data == "Home") {
                        window.location.href = '/Home/Index';
                    } else if (!data) {
                        $.redirect("/SelectPage/SelectPage", {
                            'id': $("#hdId").val()
                        }, "POST");
                    } else {
                        var arg = {
                            DocDetId: data.DocDetId,
                            DocId: data.DocId,
                            DocPageNo: data.DocPageNo,
                            PageType: data.PageType,
                            PageFileName: data.PageFileName,
                            PagePath: data.PagePath,
                            PatternNo: data.PatternNo
                        }

                        $.redirect("/SelectArea/Index", {
                            item: arg
                        }, "POST"); 
                    }
                },
                dataType: "json",
                traditional: true
            })
        });

        //------------//
        
    });

    function areaToSave(area) {
        return (typeof area.id === "undefined" ? "" : (area.id + "|")) + area.x + '|' + area.y + '|' + area.width + '|' + area.height
    };

    // Log the quantity of selections
    function debugQtyAreas(event, id, areas) {
        console.log(areas.length + " areas", arguments);
    };

    function setAreaValue(patternNo) {
        if (patternNo == "1") {
            $('img#pattern').attr("src", "../Content/img/Pattern/OCRArea_Pattern1n.png");
            area1 = { x: 78, y: 145, width: 122, height: 450, };
            area2 = { x: 223, y: 145, width: 22, height: 450, };
            area3 = { x: 270, y: 145, width: 60, height: 450, };
        } else if (patternNo == "2") {
            $('img#pattern').attr("src", "../Content/img/Pattern/OCRArea_Pattern2n.png");
            area1 = { x: 78, y: 145, width: 122, height: 450, };
            area2 = { x: 223, y: 145, width: 22, height: 450, };
            area3 = { x: 270, y: 145, width: 60, height: 450, };
            area4 = { x: 340, y: 145, width: 60, height: 450, };
        } else if (patternNo == "3") {
            $('img#pattern').attr("src", "../Content/img/Pattern/OCRArea_Pattern3n.png");
            area1 = { x: 78, y: 145, width: 122, height: 450, };
            area2 = { x: 223, y: 145, width: 22, height: 450, };
            area3 = { x: 270, y: 145, width: 60, height: 450, };
            area4 = { x: 340, y: 145, width: 60, height: 450, };
            area5 = { x: 410, y: 145, width: 60, height: 450, };
        }
    }
}

var viewModel = new ViewModel();
ko.applyBindings(viewModel);