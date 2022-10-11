function PersianDateTimePicker(value) {
    $(value).MdPersianDateTimePicker({
        Placement: 'bottom',
        Trigger: 'click',
        EnableTimePicker: false,
        TargetSelector: value,
        Format: 'yyyy/MM/dd',
        IsGregorian: false,
        EnglishNumber: true,
        

    });
}
function PersianDateTimePickerTop(value) {
    $(value).MdPersianDateTimePicker({
        Placement: 'top',
        Trigger: 'click',
        EnableTimePicker: false,
        TargetSelector: value,
        Format: 'yyyy/MM/dd',
        IsGregorian: false,
        EnglishNumber: true,


    });
}
function PersianDateTimePickerwithtime(value) {
    $(value).MdPersianDateTimePicker({
        Placement: 'bottom',
        Trigger: 'focus',
        EnableTimePicker: true,
        timeFormat: "12",
        TargetSelector: value,
        Format: 'yyyy/MM/dd hh:mm:ss tt',
        IsGregorian: false,
        EnglishNumber: true,
    });
}
function ToShamsiNumber(sNum) {
    sResult = "";
    sChar = "";
    for (n = 0; n <= (sNum.length) - 1; n++) {
        sChar = sNum.substr(n, 1);

        if (sChar == null || sChar == "") {
            sResult = "";
        } else {
            sResult += FormatToShamsiNumber(sChar);
        }
    }
    return sResult;
}
function FormatToShamsiNumber(sNum) {
    sResult = "";
    switch (sNum) {
        case "0":
            sResult = "۰";
            break;
        case "1":
            sResult = "۱";
            break;
        case "2":
            sResult = "۲";
            break;
        case "3":
            sResult = "۳";
            break;
        case "4":
            sResult = "۴";
            break;
        case "5":
            sResult = "۵";
            break;
        case "6":
            sResult = "۶";
            break;
        case "7":
            sResult = "۷";
            break;
        case "8":
            sResult = "۸";
            break;
        case "9":
            sResult = "۹";
            break;
        default:
            sResult = sNum;
            break;
    }
    return sResult;
}

function ToEnglishNumber(sNum, id) {

    sResult = "";
    sChar = "";
    for (n = 0; n <= (sNum.length) - 1; n++) {
        sChar = sNum.substr(n, 1);
        if (sChar == null || sChar == "") {
            sResult = "";
        } else {
            sResult += FormatToEnglishNumber(sChar);


        }

    }

    $("#" + id).val('').val(sResult);
    $("#" + id).focus();
    $("#" + id).blur();

    return sResult;
}
function ToEnglishNumber(sNum) {
    sResult = "";
    sChar = "";
    for (n = 0; n <= (sNum.length) - 1; n++) {
        sChar = sNum.substr(n, 1);

        if (sChar == null || sChar == "") {
            sResult = "";
        } else {
            sResult += FormatToEnglishNumber(sChar);
        }
    }
    return sResult;
}
function FormatToEnglishNumber(sNum) {
    sResult = "";
    switch (sNum) {
        case "۰":
            sResult = "0";
            break;
        case "۱":
            sResult = "1";
            break;
        case "۲":
            sResult = "2";
            break;
        case "۳":
            sResult = "3";
            break;
        case "۴":
            sResult = "4";
            break;
        case "۵":
            sResult = "5";
            break;
        case "۶":
            sResult = "6";
            break;
        case "۷":
            sResult = "7";
            break;
        case "۸":
            sResult = "8";
            break;
        case "۹":
            sResult = "9";
            break;
        default:
            sResult = sNum;
            break;
    }
    return sResult;
}






function LoadAjaxDatatable(url,Parameter,TableID,NoCulumn) {
        $.ajax({
            url: url,
            type: "POST",
            contentType: "application/json",
            data: {Parameter},
            datatype: "JSON",
            success: function (result) {
                var tr = "";
                $.each(result, function (i, state) {

                   
                
                    tr += "<tr>";
                    tr += "<td>";
                    tr += state.
                    tr += "</td>";
                    tr += "</tr>";


                })

                $(TableID+">tbody").html('').html();

            },
            error: function (error) {
                alert("Error Ouccer");
            },
            complete: function (data) {

            }
        });
    }




function loadData(URL,tableName, successTarget, errorTarget) {
    var el = $(tableName);
    $.ajax({
        url: URL,
        type: "POST",
        beforeSend: function () {
            App.blockUI({
                target: el,
                animate: !0,
                overlayColor: 'none'
            });
        },
        success: function (result) {
            setTimeout(function () {
                App.unblockUI(el);
            }, 1000);
            $(successTarget).html(result);
        },
        complete: function () {
       
                MakeDataTable(tableName);
          
          
        },
        error: function (error) {
            $(errorTarget).html("").html("error loading data..." + error.responseText);
            setTimeout(function () {
                App.unblockUI(el);
            }, 1000);
        },
    });
}

function loadDataMakeModal(URL, tableName, successTarget, errorTarget,ModalName) {
    var el = $(tableName);
    $.ajax({
        url: URL,
        type: "POST",
        beforeSend: function () {
            App.blockUI({
                target: el,
                animate: !0,
                overlayColor: 'none'
            });
        },
        success: function (result) {
            setTimeout(function () {
                App.unblockUI(el);
            }, 1000);
            $(successTarget).html(result);
        },
        complete: function () {
            $(ModalName).modal("show");
            MakeDataTable(tableName);
        


        },
        error: function (error) {
            $(errorTarget).html("").html("error loading data..." + error.responseText);
            setTimeout(function () {
                App.unblockUI(el);
            }, 1000);
        },
    });
}
function MakeDataTable(value) {

    var table = $(value).DataTable({
        stateSave: true,
        responsive:false,
        "language": {
            "emptyTable": "No Record Found",
            "info": "Showing <b>( _START_  )</b> to <b>( _END_  )</b> of <b>( _TOTAL_ )</b> entries "
             ,
            "infoEmpty": "From ( 0 ) To ( 0 )  Total Record ( 0 ) show",
            "infoFiltered": "(Found <b> ( _MAX_ ) </b> Records)",
            "infoPostFix": "",
            "lengthMenu": "Show _MENU_ Record",
            "search": "Search <span class='glyphicon glyphicon-search'></span>",
            "searchPlaceholder": "Search",
            "zeroRecords": "No Record Found  ",
            "paginate": {
                "first": "First",
                "previous": "Previous ",
                "next": "Next",
                "last": "Last"
            },
        },
        buttons: [{ extend: "excel", className: "btn red moveTop ", text: "<i class='fa fa-file-excel-o' aria-hidden='true'>&nbsp;<b>Excel</b>" },
        { extend: "colvis", className: "btn dark moveTop", text: "<b>Column Show/Hide</b>&nbsp;<i class='fa fa-arrow-circle-down' aria-hidden='true'></i>" }],
        lengthMenu: [[5, 10, 25, 100, -1], [5, 10, 25, 100, "All"]],
        dom: "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
    });
    $('table tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected intro')) {
            $(this).removeClass('selected intro');
        }
        else {
            table.$('tr.selected').removeClass('selected intro');
            $(this).addClass('selected intro');
        }
    });
}
function MakeDataTableMini(value) {
    var table = $(value).DataTable({
        responsive: false,
        "language": {
            "emptyTable": "په جدول کې معلومات شتون نه لري",
            "info": "",
            "infoEmpty": "",
            "infoFiltered": "(All پیدا شوي  <b> ( _MAX_ ) </b> ریکارډونه)",
            "infoPostFix": "",
            "lengthMenu": "",
            "search": "Search <span class='glyphicon glyphicon-search'></span>",
            "searchPlaceholder": "Search",
            "zeroRecords": "پدی نوم هیڅ ریکارډ پیدا نشو ",
            "paginate": {
                "first": "لومړۍ پاڼه",
                "previous": "مخکې",
                "next": "وروسته",
                "last": "پای"
            },
        },
        bStateSave: !0, pagingType: "bootstrap_extended", lengthMenu: [[5, 15, 20, -1], [5, 15, 20, "All"]], pageLength: 10, columnDefs: [{
            orderable: !1, targets: [0]
        }
             , {
                 searchable: !1, targets: [0]
             }
        ], order: [[1, "asc"]]
    });
}


function toastorOptions() {
    toastr.options.positionClass = "toast-top-center";
    toastr.options.closeButton = true;
    toastr.options.timeOut = 2500;
    toastr.options.extendedTimeOut = 0;
    toastr.options.showMethod = "fadeIn";
    toastr.options.type = "success";
    toastr.options.text = "ریکارد ذخیره شو";
}
function loadRelatedDocs(URL, el, target) {
    $.ajax({
        url: URL,
        type: "POST",
        beforeSend: function () {
            App.blockUI({
                target: el,
                animate: !0,
                overlayColor: 'none'
            });
        },
        success: function (result) {
            setTimeout(function () {
                App.unblockUI(el);
            }, 1000);
            $(target).html(result);
        },
        error: function (error) {
            $(target).html("").html("error loading data..." + error.responseText);
            setTimeout(function () {
                App.unblockUI(el);
            }, 1000);
        },
    });
}


function fnDelete(RecordID, TableName) {         
    $('#myModalDelete').modal('show');
    $('#RecordID').val('').val(RecordID);
    $('#TableName').val('').val(TableName);

}