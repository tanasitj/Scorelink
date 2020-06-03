var ViewModel = function () {
    var self = this;
    self.DocId = ko.observable();
    self.DocPageNo = ko.observable();
    self.PageFileName = ko.observable();
    self.PagePath = ko.observable();
    self.PatternNo = ko.observable();
    self.chkAccTitleAlphabet = ko.observable();
    self.chkAccTitleNumber = ko.observable();
    self.chkAccTitleSymbol = ko.observable();
    self.chkFootnoteAlphabet = ko.observable();
    self.chkFootnoteNumber = ko.observable();
    self.chkFootnoteSymbol = ko.observable();
    self.opNumberOfDec = ko.observable();
    self.rdNoiseReduce = ko.observable();


    $(document).ready(function () {

        $('#btnSubmit').click(function () {
            $("#hdPatternNo").val($("#rdPattern:checked").val());
            $("#chkAccTitleAlphabet").is(":checked") ? $("#hdAccTitleAlphabet").val("Y") : $("#hdAccTitleAlphabet").val("");
            $("#chkAccTitleNumber").is(":checked") ? $("#hdAccTitleNumber").val("Y") : $("#hdAccTitleNumber").val("");
            $("#chkAccTitleSymbol").is(":checked") ? $("#hdAccTitleSymbol").val("Y") : $("#hdAccTitleSymbol").val("");
            $("#chkFootnoteAlphabet").is(":checked") ? $("#hdFootnoteAlphabet").val("Y") : $("#hdFootnoteAlphabet").val("");
            $("#chkFootnoteNumber").is(":checked") ? $("#hdFootnoteNumber").val("Y") : $("#hdFootnoteNumber").val("");
            $("#chkFootnoteSymbol").is(":checked") ? $("#hdFootnoteSymbol").val("Y") : $("#hdFootnoteSymbol").val("");
            $("#hdNumberOfDec").val($("#opNumberOfDec").val());
            $("#hdNoiseReduce").val($("#rdNoiseReduce:checked").val());

            var arg = {
                DocDetId: $("#hdDocDetId").val(),
                DocId: $("#hdId").val(),
                DocPageNo: $("#hdDocPageNo").val(),
                PageFileName: $("#hdPageFileName").val(),
                PagePath: $("#hdPagePath").val(),
                //PatternNo: $("#rdPattern:checked").val(),
                AccTitleAlphabet: $("#hdAccTitleAlphabet").val(),
                AccTitleNumber: $("#hdAccTitleNumber").val(),
                AccTitleSymbol: $("#hdAccTitleSymbol").val(),
                FootnoteAlphabet: $("#hdFootnoteAlphabet").val(),
                FootnoteNumber: $("#hdFootnoteNumber").val(),
                FootnoteSymbol: $("#hdFootnoteSymbol").val(),
                NumberOfDec: $("#hdNumberOfDec").val(),
                NoiseReduce: $("#hdNoiseReduce").val()
            }

            var data = {
                'item': arg,
                'PatternNo': $("#rdPattern:checked").val()
            };

            $.ajax({
                url: '/SelectPattern/SavePattern',
                cache: false,
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: ko.toJSON(data),
                success: function (data) {
                    goSelectArea();
                }
            })
        });

        function goSelectArea() {
            var arg = {
                DocDetId: $("#hdDocDetId").val(),
                DocId: $("#hdId").val(),
                DocPageNo: $("#hdDocPageNo").val(),
                PageFileName: $("#hdPageFileName").val(),
                PagePath: $("#hdPagePath").val(),
                PatternNo: $("#rdPattern:checked").val()
            }

            $.redirect("/SelectArea/SelectArea", {
                item: arg
            }, "POST"); 
        };
    });
}


var viewModel = new ViewModel();
ko.applyBindings(viewModel);