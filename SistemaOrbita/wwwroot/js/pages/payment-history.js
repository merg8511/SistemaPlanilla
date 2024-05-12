const { type } = require("jquery");

$(function () {
    initialize();
})

function initialize() {
    selectType();
    initialSelect2();
    setupSearch();
}

function selectType() {
    $("#select-type").on("change", function () {
        const selected = $(this).val();
        toggleVisibility(selected);
    })
}

function toggleVisibility(selected) {
    const dataMappings = {
        '1': ["#employee-data"],
        '2': ["#payroll-data"],
        '3': ["#department-data"]
    };

    $("#employee-data, #payroll-data, #department-data").hide();
    if (dataMappings[selected]) {
        $(dataMappings[selected].join(", ")).show();
        $("#div-search").show();
    } else {
        $("#div-search").hide();
    }
}

function initialSelect2() {
    setupSelect2("#employee-select");
    setupSelect2("#payroll-select");
}

function setupSelect2(selector) {
    const select = $(selector).select2({
        width: '100%',
        theme: 'bootstrap-5',
        placeholder: 'Select'
    });

    select.on('select2:open', () => {
        document.querySelector('.select2-search__field').focus();
    });
}

function setupSearch() {
    $('#btn-search').on('click', function () {
        const selected = $("#select-type").val();
        searchResult(selected);
    })
}

function searchResult(selected) {
    if (selected == '1') {
        const employeeId = $("#employee-select").val();
        const dates = getDates('#employee-start-date', '#employee-end-date');
        sendEmployeeData(employeeId, dates.start, dates.end);

    } else if (selected == '2') {
        const payrollId = $("#payroll-select").val();
        sendPayrollData(payrollId);

    } else if (selected == '3') {
        const departmentId = $("#department-select").val();
        const dates = getDates('#department-start-date', '#department-end-date');
        sendDepartmentData(departmentId, dates.start, dates.end);
    }
}

function getDates(startDateSelector, endDateSelector) {
    return {
        start: $(startDateSelector).val(),
        end: $(endDateSelector).val()
    };
}

function sendEmployeeData(employeeId, startDate, endDate) {
    $.ajax({
        url: '/Payroll/PaymentHistory/GetEmployeePayment',
        method: 'POST',
        data: {
            employeeId: employeeId,
            startDate: startDate,
            endDate: endDate
        },
        success: function (response) {
            loadDataTable(response);
        },
        error: function (error) {
            console.error('Error sending data', error);
        }
    });
}

function sendPayrollData(payrollId) {
    $.ajax({
        url: 'your-endpoint-url',
        method: 'POST',
        data: { payrollId: payrollId },
        success: function (response) {
            // Carga aquí el datatable
        },
        error: function (error) {
            console.error('Error sending data', error);
        }
    });
}

function sendDepartmentData(departmentId, startDate, endDate) {
    $.ajax({
        url: 'your-endpoint-url',
        method: 'POST',
        data: {
            departmentId: departmentId,
            startDate: startDate,
            endDate: endDate
        },
        success: function (response) {
            // Carga aquí el datatable
        },
        error: function (error) {
            console.error('Error sending data', error);
        }
    });
}


function loadDataTable(data) {
    $("#dataTable").LoadingOverlay("show");

    dataTable = $("#dataTable").DataTable({
        "destroy": true,
        "source": data,
        "columns": [
            { "data": "payroll.createdAt", "width": "20%" },
            {
                "data": "employee",
                "render": function (data) {
                    return `${data.firtsName}`
                },
                "width": "25%"
            },
            {
                "data": "paymentDate",
                "className": "align-middle text-center",
                "render": function (data, type, row) {
                    return new Date(data).toLocaleDateString('es')
                },
                "width": "10%"
            },
            { "data": "amount", "width": "20%" },
            {
                "data": "status",
                "className": "align-middle text-center",
                "render": function (data) {

                    if (data == 1) {
                        return '<span class="badge bg-success" style="font-size:0.9rem";><i class="bi bi-check-circle me-1"></i> Active</span>';
                    } else {
                        return '<span class="badge bg-danger"  style="font-size:0.9rem";><i class="bi bi-exclamation-octagon me-1"></i> Inactive</span>';
                    }
                },
                "width": "10%"
            },
        ],
        "responsive": "true",
        "dom": 'l<"d-flex float-right"f><"border-1 border-white"B>rtip', // l = length changing input, B = buttons, f = filtering input, r = processing display element, t = table, i = information summary, p = pagination control

        "buttons": [
            {
                extend: 'copy',
                exportOptions: {
                    columns: ':visible:not(:last-child)'
                }
            },
            {
                extend: 'excel',
                exportOptions: {
                    columns: ':visible:not(:last-child)'
                }
            },
            {
                extend: 'pdf',
                exportOptions: {
                    columns: ':visible:not(:last-child)'
                }
            },
            {
                extend: 'csv',
                exportOptions: {
                    columns: ':visible:not(:last-child)'
                }
            },
            {
                extend: 'print',
                exportOptions: {
                    columns: ':visible:not(:last-child)'
                }
            }
        ],

        "initComplete": function (settings, json) {
            $("#dataTable").LoadingOverlay("hide");
        }
    })
}