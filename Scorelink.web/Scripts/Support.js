window.get = function (id) {
    return document.getElementById(id);
}

window.stackTrace = function () {
    var err = new Error();
    console.log(err.stack);
}

window.disableEventPropagation = function (event) {
    if (event.stopPropagation) {
        event.stopPropagation();
    }
    else if (window.event) {
        window.event.cancelBubble = true;
    }
}

$.fn.animateRotate = function (angle, duration, easing, complete) {
    var args = $.speed(duration, easing, complete);
    var step = args.step;
    return this.each(function (i, e) {
        args.complete = $.proxy(args.complete, e);
        args.step = function (now) {
            $.style(e, 'transform', 'rotate(' + now + 'deg)');
            if (step) return step.apply(e, arguments);
        };

        $({ deg: 0 }).animate({ deg: angle }, args);
    });
};

function getActivityTypeApproved(lastActivityType, isRejected) {
    if (lastActivityType == "admin-received") {
        return isRejected ? "admin-rejected" : "admin-approved";
    }
    else if (lastActivityType == "advisor-received") {
        return isRejected ? "advisor-rejected" : "advisor-approved";
    }
    else if (lastActivityType == "dean-received") {
        return isRejected ? "dean-rejected" : "dean-approved";
    }
}

function PNotification(title, text, type) {
    new PNotify({
        title: title,
        text: text,
        type: type,
        styling: 'bootstrap4'
    });
}

function parseJsonDate(jsonDate) {

    var fullDate = new Date(parseInt(jsonDate.substr(6)));
    var twoDigitMonth = (fullDate.getMonth() + 1) + ""; if (twoDigitMonth.length == 1) twoDigitMonth = "0" + twoDigitMonth;

    var twoDigitDate = fullDate.getDate() + ""; if (twoDigitDate.length == 1) twoDigitDate = "0" + twoDigitDate;
    var retDate = twoDigitDate + "/" + twoDigitMonth + "/" + parseInt(fullDate.getFullYear());

    return retDate;
}
function parseJsonDate2(jsonDate) {

    var fullDate = new Date(parseInt(jsonDate.substr(6)));
    var twoDigitMonth = (fullDate.getMonth() + 1) + ""; if (twoDigitMonth.length == 1) twoDigitMonth = "0" + twoDigitMonth;

    var twoDigitDate = fullDate.getDate() + ""; if (twoDigitDate.length == 1) twoDigitDate = "0" + twoDigitDate;
    var retDate = twoDigitDate + "-" + twoDigitMonth + "-" + parseInt(fullDate.getFullYear());

    return retDate;
}
function parseJsonDateYMD(jsonDate) {

    var fullDate = new Date(parseInt(jsonDate.substr(6)));
    var twoDigitMonth = (fullDate.getMonth() + 1) + ""; if (twoDigitMonth.length == 1) twoDigitMonth = "0" + twoDigitMonth;

    var twoDigitDate = fullDate.getDate() + ""; if (twoDigitDate.length == 1) twoDigitDate = "0" + twoDigitDate;
    var retDate = parseInt(fullDate.getFullYear()) + "-" + twoDigitMonth + "-" + twoDigitDate;

    return retDate;
}
function parseJsonDateTime(jsonDate) {

    var fullDate = new Date(parseInt(jsonDate.substr(6)));
    var twoDigitMonth = (fullDate.getMonth() + 1) + ""; if (twoDigitMonth.length == 1) twoDigitMonth = "0" + twoDigitMonth;

    var twoDigitDate = fullDate.getDate() + ""; if (twoDigitDate.length == 1) twoDigitDate = "0" + twoDigitDate;
    var twoDigitHour = fullDate.getHours() + ""; if (twoDigitHour.length == 1) twoDigitHour = "0" + twoDigitHour;
    var twoDigitMinute = fullDate.getMinutes() + ""; if (twoDigitMinute.length == 1) twoDigitMinute = "0" + twoDigitMinute;
    var retDate = twoDigitDate + "/" + twoDigitMonth + "/" + parseInt(fullDate.getFullYear()) + " " + twoDigitHour + ":" + twoDigitMinute;

    return retDate;
}

function parseJsonDateTimeYYYYMMDD(jsonDate) {

    var fullDate = new Date(parseInt(jsonDate.substr(6)));
    var twoDigitMonth = (fullDate.getMonth() + 1) + ""; if (twoDigitMonth.length == 1) twoDigitMonth = "0" + twoDigitMonth;

    var twoDigitDate = fullDate.getDate() + ""; if (twoDigitDate.length == 1) twoDigitDate = "0" + twoDigitDate;
    var twoDigitHour = fullDate.getHours() + ""; if (twoDigitHour.length == 1) twoDigitHour = "0" + twoDigitHour;
    var twoDigitMinute = fullDate.getMinutes() + ""; if (twoDigitMinute.length == 1) twoDigitMinute = "0" + twoDigitMinute;
    var retDate = parseInt(fullDate.getFullYear()) + "-" + twoDigitMonth + "-" + twoDigitDate + " " + twoDigitHour + ":" + twoDigitMinute;

    return retDate;
}
function parseDateTimeForCompare(jsonDate) {

    var fullDate = new Date(parseInt(jsonDate.substr(6)));
    var twoDigitMonth = (fullDate.getMonth() + 1) + ""; if (twoDigitMonth.length == 1) twoDigitMonth = "0" + twoDigitMonth;

    var twoDigitDate = fullDate.getDate() + ""; if (twoDigitDate.length == 1) twoDigitDate = "0" + twoDigitDate;
    var twoDigitHour = fullDate.getHours() + ""; if (twoDigitHour.length == 1) twoDigitHour = "0" + twoDigitHour;
    var twoDigitMinute = fullDate.getMinutes() + ""; if (twoDigitMinute.length == 1) twoDigitMinute = "0" + twoDigitMinute;
    var retDate = twoDigitDate + "/" + twoDigitMonth + "/" + parseInt(fullDate.getFullYear()) + " " + twoDigitHour + ":" + twoDigitMinute;

    return retDate;
}

function getCurrentYear() {
    var today = new Date();
    var yyyy = today.getFullYear();

    return yyyy
}

function getCurrentMonth() {
    var today = new Date();
    var mm = today.getMonth() + 1; if (mm < 10) mm = '0' + mm;

    return mm
}

function getCurrentDate() {
    var today = new Date();
    var dd = today.getDate(); if (dd < 10) dd = '0' + dd;
    var mm = today.getMonth() + 1; if (mm < 10) mm = '0' + mm;
    var yyyy = today.getFullYear();

    today = dd + '/' + mm + '/' + yyyy;
    return today
}

function setDateFormatYMD(dateString) {
    var dd = dateString.substring(0, 2);
    var mm = dateString.substring(3, 5);
    var yyyy = dateString.substring(6, 10);

    return yyyy + mm + dd;
}

function setDateFormatYMD2(dateString) {
    var dd = dateString.substring(0, 2);
    var mm = dateString.substring(3, 5);
    var yyyy = dateString.substring(6, 10);

    return yyyy + '-' + mm + '-' + dd;
}

function setDateFormatYYYYMMDD(dateString) {
    var dd = dateString.substring(0, 2);
    var mm = dateString.substring(3, 5);
    var yyyy = dateString.substring(6, 10);

    return yyyy + mm + dd;
}

function setDateFormatYYYYMMDDTH(dateString) {
    var dd = dateString.substring(0, 2);
    var mm = dateString.substring(3, 5);
    var yyyy = parseInt(dateString.substring(6, 10)) + 543;

    return yyyy + mm + dd;
}

function setDateFormatThai(dateString) {
    var dd = dateString.substring(0, 2);
    var mm = dateString.substring(3, 5);
    var yyyy = dateString.substring(6, 10);
    var mmm = "";
    switch (mm) {
        case "01": mmm = "มกราคม"; break;
        case "02": mmm = "กุมภาพันธ์"; break;
        case "03": mmm = "มีนาคม"; break;
        case "04": mmm = "เมษายน"; break;
        case "05": mmm = "พฤษภาคม"; break;
        case "06": mmm = "มิถุนายน"; break;
        case "07": mmm = "กรกฎาคม"; break;
        case "08": mmm = "สิงหาคม"; break;
        case "09": mmm = "กันยายน"; break;
        case "10": mmm = "ตุลาคม"; break;
        case "11": mmm = "พฤศจิกายน"; break;
        case "12": mmm = "ธันวาคม"; break;
    }

    return dd + '-' + mmm + '-' + yyyy;
}

function getCurrentDateTime() {
    var today = new Date();
    var dd = today.getDate(); if (dd < 10) dd = '0' + dd;
    var mm = today.getMonth() + 1; if (mm < 10) mm = '0' + mm;
    var yyyy = today.getFullYear();
    var twoDigitHour = today.getHours() + ""; if (twoDigitHour.length == 1) twoDigitHour = "0" + twoDigitHour;
    var twoDigitMinute = today.getMinutes() + ""; if (twoDigitMinute.length == 1) twoDigitMinute = "0" + twoDigitMinute;

    today = dd + '/' + mm + '/' + yyyy + ' ' + twoDigitHour + ':' + twoDigitMinute;
    return today
}
function setDateFormatYMDHHmm(dateString) {
    var dd = dateString.substring(0, 2);
    var mm = dateString.substring(3, 5);
    var yyyy = dateString.substring(6, 10);
    var hour = dateString.substring(11, 13);
    var min = dateString.substring(14, 16);

    return yyyy + mm + dd + hour + min;

    //var d = moment(dateString).format("YYYYMMDDHHmm");

    //return d;
}

//function setDateFormatYMD(dateString) {
//    //var dd = dateString.substring(0, 2);
//    //var mm = dateString.substring(3, 5);
//    //var yyyy = dateString.substring(6, 10);
//    //var hour = dateString.substring(11, 13);
//    //var min = dateString.substring(14, 16);

//    //return yyyy + mm + dd + hour + min;

//    return moment(dateString, "YYYYMMDD");
//}

function getThaiDate(date) {
    var d = date.substring(0, 2) + "/" + date.substring(3, 5) + "/" + (parseInt(date.substring(6, 10)) + 543);
    return d;
}

function SetDateFormatYMD(date) {
    var dd = date.getDate(); if (dd < 10) dd = '0' + dd;
    var mm = date.getMonth() + 1; if (mm < 10) mm = '0' + mm;
    var yyyy = date.getFullYear();

    var ret = dd + '/' + mm + '/' + yyyy;
    return ret
}

function confirm(title, message, successFunc, failFunc, okName, height) {

    if (get('dialog-confirm')) {
        //get('ui-id-4').innerHTML = title;
        get('dialog-confirm').parentElement.children[0].children[0].innerHTML = title;
        get('dialog-confirm').children[0].innerHTML = message;
    } else {
        $(document.body).append("<div id='dialog-confirm' class='WizardDialog' title='" + title + "' style='height:40px'><p>" + message + "</p></div>");
    }

    $("#dialog-confirm").dialog({
        resizable: false,
        draggable: false,
        height: height || 220,
        modal: true,
        show: 'clip',
        hide: 'clip',
        dialogClass: 'no-close',
        buttons: [
            {
                text: okName || 'OK',
                "class": 'btnWizBlue',
                click: function () {
                    successFunc()
                    $(this).dialog("close");
                }
            },
            {
                text: "Cancel",
                "class": 'btnWizOrng',
                click: function () {
                    failFunc();
                    $(this).dialog("close");
                }
            }
        ],
    });
}

//function getCookie(cname, index) {
//    var name = cname + "=";
//    var ca = document.cookie.split(',');
//    return ca[index];
//}
function getHours() {
    var hours = [];
    
    for (var i = 0; i < 24; i++) {
        if (i < 10) i = "0" + i;
        hours.push({
            'id': i,
            'name': i
        });
    }

    return hours;
}
function getMinutes() {
    var minutes = [];

    for (var i = 0; i < 60; i++) {
        if (i < 10) i = "0" + i;
        minutes.push({
            'id': i,
            'name': i
        });
    }

    return minutes;
}

function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split('=');
    if (!ca) return false;

    var cValue = null;
    for (var i = 0; i < ca.length; i++) {
        if (i == 1) {
            cValue = ca[i];
        }
    }

    var data = cValue.split(',');

    return data;
}

function isStudent(role) {
    return role.toLowerCase() == "student" ? true : false;
}

function isAdmin(role) {
    return role.toLowerCase() == "admin" ? true : false;
}

function isAdvisor(role) {
    return role.toLowerCase() == "advisor" ? true : false;
}

function isDean(role) {
    return role.toLowerCase() == "dean" ? true : false;
}

function getActivityTypes() {
    var data = [
        { ActivityType: "open", ActivityDescription: "นิสิตกรอกข้อมูลคำร้อง" },
        { ActivityType: "submit", ActivityDescription: "นิสิตส่งเอกสารแล้วรอเจ้าหน้าที่ตรวจสอบ" },
        { ActivityType: "admin-received", ActivityDescription: "เจ้าหน้าที่กำลังตรวจสอบคำร้อง" },
        { ActivityType: "admin-approved", ActivityDescription: "เจ้าหน้าที่ตรวจสอบแล้วรออาจาย์ที่ปรึกษาอนุมัติคำร้อง" },
        { ActivityType: "admin-rejected", ActivityDescription: "เอกสารส่งกลับนิสิตเพื่อแก้ไขตามที่เจ้าหน้าแนะนำ" },
        { ActivityType: "advisor-received", ActivityDescription: "อาจารย์ที่ปรึกษากำลังพิจารณาอนุมัติคำร้อง" },
        { ActivityType: "advisor-approved", ActivityDescription: "อาจารย์ที่ปรึกษาอนุมัติแล้วรอคณบดีอนุมัติคำร้อง" },
        { ActivityType: "advisor-rejected", ActivityDescription: "อาจารย์ที่ปรึกษาไม่อนุมัติคำร้อง" },
        { ActivityType: "dean-received", ActivityDescription: "คณบดีกำลังพิจารณาอนุมัติคำร้อง" },
        { ActivityType: "dean-approved", ActivityDescription: "คณบดีอนุมัติคำร้อง" },
        { ActivityType: "dean-rejected", ActivityDescription: "คณบดีไม่อนุมัติคำร้อง" },
        { ActivityType: "completed", ActivityDescription: "คำร้องสมบูรณ์" },
        { ActivityType: "cancel", ActivityDescription: "ยกเลิกคำร้อง" }
    ];

    return data;
}

function getActivityTypesForCompleted() {
    var data = [
        { ActivityType: "dean-approved", ActivityDescription: "คณบดีอนุมัติคำร้อง" },
        { ActivityType: "completed", ActivityDescription: "คำร้องสมบูรณ์" }
    ];

    return data;
}

function getContendApprovedConfirm(bApproved, name, year, major) {
    var contend = null;
    if (bApproved) {
        contend = '<b>ยืนยันการอนุมัติคำร้อง</b> <br/>นิสิตชื่อ: ';
    } else {
        contend = '<b>ยืนยันไม่อนุมัติคำร้อง</b> <br/>นิสิตชื่อ: ';
    }
    contend += name + '<br/>ชั้นปีที่: ' + year + '<br/>สาขาวิชา: ' + major;

    return contend;
}

function blockUI() {
    $.blockUI({
        css: {
            border: 'none',
            padding: '15px',
            backgroundColor: '#000',
            '-webkit-border-radius': '10px',
            '-moz-border-radius': '10px',
            opacity: .5,
            color: '#fff'
        }
    });
}

function unblockUI() {
    setTimeout($.unblockUI, 500);
}

function getSmSEMList() {
    var data = [
        { id: "1", name: "ภาคต้น" },
        { id: "2", name: "ภาคปลาย" },
        { id: "0", name: "ภาคฤดูร้อน" }
    ];

    return data;
}

function getDayOfWeekList() {
    var data = [
        { id: "7", name: "วันอาทิตย์" },
        { id: "1", name: "วันจันทร์" },
        { id: "2", name: "วันอังคาร" },
        { id: "3", name: "วันพุธ" },
        { id: "4", name: "วันพฤหัสบดี" },
        { id: "5", name: "วันศุกร์" },
        { id: "6", name: "วันเสาร์" }
    ];

    return data;
}

function getSmName(id) {
    var smName = "";
    if (id === "0") {
        smName = "ภาคฤดูร้อน"
    }
    if (id === "1") {
        smName = "ภาคต้น"
    }
    if (id === "2") {
        smName = "ภาคปลาย"
    }
    return smName;
}

function partUrl() {
    return '/wlb';
    //return '';
}

function autocomplete(inp, arr) {
    /*the autocomplete function takes two arguments,
    the text field element and an array of possible autocompleted values:*/
    var currentFocus;
    /*execute a function when someone writes in the text field:*/
    inp.addEventListener("input", function (e) {
        var a, b, i, val = this.value;
        /*close any already open lists of autocompleted values*/
        closeAllLists();
        if (!val) { return false; }
        currentFocus = -1;
        /*create a DIV element that will contain the items (values):*/
        a = document.createElement("DIV");
        a.setAttribute("id", this.id + "autocomplete-list");
        a.setAttribute("class", "autocomplete-items");
        /*append the DIV element as a child of the autocomplete container:*/
        this.parentNode.appendChild(a);
        /*for each item in the array...*/
        for (i = 0; i < arr.length; i++) {
            /*check if the item starts with the same letters as the text field value:*/
            if (arr[i].substr(0, val.length).toUpperCase() == val.toUpperCase()) {
                /*create a DIV element for each matching element:*/
                b = document.createElement("DIV");
                /*make the matching letters bold:*/
                b.innerHTML = "<strong>" + arr[i].substr(0, val.length) + "</strong>";
                b.innerHTML += arr[i].substr(val.length);
                /*insert a input field that will hold the current array item's value:*/
                b.innerHTML += "<input type='hidden' value='" + arr[i] + "'>";
                /*execute a function when someone clicks on the item value (DIV element):*/
                b.addEventListener("click", function (e) {
                    /*insert the value for the autocomplete text field:*/
                    inp.value = this.getElementsByTagName("input")[0].value;
                    /*close the list of autocompleted values,
                    (or any other open lists of autocompleted values:*/
                    closeAllLists();
                });
                a.appendChild(b);
            }
        }
    });
    /*execute a function presses a key on the keyboard:*/
    inp.addEventListener("keydown", function (e) {
        var x = document.getElementById(this.id + "autocomplete-list");
        if (x) x = x.getElementsByTagName("div");
        if (e.keyCode == 40) {
            /*If the arrow DOWN key is pressed,
            increase the currentFocus variable:*/
            currentFocus++;
            /*and and make the current item more visible:*/
            addActive(x);
        } else if (e.keyCode == 38) { //up
            /*If the arrow UP key is pressed,
            decrease the currentFocus variable:*/
            currentFocus--;
            /*and and make the current item more visible:*/
            addActive(x);
        } else if (e.keyCode == 13) {
            /*If the ENTER key is pressed, prevent the form from being submitted,*/
            e.preventDefault();
            if (currentFocus > -1) {
                /*and simulate a click on the "active" item:*/
                if (x) x[currentFocus].click();
            }
        }
    });
    function addActive(x) {
        /*a function to classify an item as "active":*/
        if (!x) return false;
        /*start by removing the "active" class on all items:*/
        removeActive(x);
        if (currentFocus >= x.length) currentFocus = 0;
        if (currentFocus < 0) currentFocus = (x.length - 1);
        /*add class "autocomplete-active":*/
        x[currentFocus].classList.add("autocomplete-active");
    }
    function removeActive(x) {
        /*a function to remove the "active" class from all autocomplete items:*/
        for (var i = 0; i < x.length; i++) {
            x[i].classList.remove("autocomplete-active");
        }
    }
    function closeAllLists(elmnt) {
        /*close all autocomplete lists in the document,
        except the one passed as an argument:*/
        var x = document.getElementsByClassName("autocomplete-items");
        for (var i = 0; i < x.length; i++) {
            if (elmnt != x[i] && elmnt != inp) {
                x[i].parentNode.removeChild(x[i]);
            }
        }
    }
    /*execute a function when someone clicks in the document:*/
    document.addEventListener("click", function (e) {
        closeAllLists(e.target);
    });
}


window.keys = [];
window.onkeyup = function (e) { window.keys[e.keyCode] = false; }
window.onkeydown = function (e) { window.keys[e.keyCode] = true; }