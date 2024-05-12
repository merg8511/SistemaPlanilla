$(function () {
    loadDataTable();
})

function loadDataTable() {
    new DataTable('#dataTable', {
        fixedColumns: {
            start: 2
        },
        paging: false,
        scrollCollapse: true,
        scrollX: true,
        scrollY: 350,
        "initComplete": function (settings, json) {
            loadTotal();
        }
    });
}

function loadTotal() {
    var totalToPay = 0;
    var txtDO = 0;
    var txtNO = 0;
    var txtSO = 0;
    var txtBO = 0;
    var txtDI = 0;
    var txtIS = 0;
    var txtAF = 0;
    var txtIR = 0;
    var txtEI = 0;
    var txtEA = 0;
    var txtTD = 0;

    $('input[name$="ToPay"]').each(function () {
        totalToPay += parseFloat($(this).val()) || 0;
    });

    $('input[name$="DayOvertime"]').each(function () {
        txtDO += parseFloat($(this).val()) || 0;
    })

    $('input[name$="NightOvertime"]').each(function () {
        txtNO += parseFloat($(this).val()) || 0;
    })

    $('input[name$="SalaryOvertime"]').each(function () {
        txtSO += parseFloat($(this).val()) || 0;
    })

    $('input[name$="Bonus"]').each(function () {
        txtBO += parseFloat($(this).val()) || 0;
    })

    $('input[name$="Discount"]').filter(function () {
        return !this.name.includes("TotalDiscount");
    }).each(function () {
        txtDI += parseFloat($(this).val()) || 0;
    });

    $('input[name$="Isss"]').each(function () {
        txtIS += parseFloat($(this).val()) || 0;
    })

    $('input[name$="Afp"]').each(function () {
        txtAF += parseFloat($(this).val()) || 0;
    })

    $('input[name$="Isr"]').each(function () {
        txtIR += parseFloat($(this).val()) || 0;
    })

    $('input[name$="EmployeerIsss"]').each(function () {
        txtEI += parseFloat($(this).val()) || 0;
    })

    $('input[name$="EmployeerAfp"]').each(function () {
        txtEA += parseFloat($(this).val()) || 0;
    })

    $('input[name$="TotalDiscount"]').each(function () {
        txtTD += parseFloat($(this).val()) || 0;
    })



    $("#txtTotalAmount").val(totalToPay.toFixed(2));
    $("#txtTP").text(`${totalToPay.toFixed(2)}`);

    $("#txtDO").text(`${txtDO.toFixed(2)}`);
    $("#txtNO").text(`${txtNO.toFixed(2)}`);
    $("#txtSO").text(`${txtSO.toFixed(2)}`);
    $("#txtBO").text(`${txtBO.toFixed(2)}`);
    $("#txtDI").text(`${txtDI.toFixed(2)}`);
    $("#txtIS").text(`${txtIS.toFixed(2)}`);
    $("#txtAF").text(`${txtAF.toFixed(2)}`);
    $("#txtIR").text(`${txtIR.toFixed(2)}`);
    $("#txtEI").text(`${txtEI.toFixed(2)}`);
    $("#txtEA").text(`${txtEA.toFixed(2)}`);
    $("#txtTD").text(`${txtTD.toFixed(2)}`);
}

