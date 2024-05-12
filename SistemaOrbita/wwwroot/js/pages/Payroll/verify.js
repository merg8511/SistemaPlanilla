$(function () {
    loadDataTable();
    setPeriod();
    btnAdd();
});

function loadDataTable() {
    let department;
    let employeeName;
    let employeeId;
    let period = JSON.parse(localStorage.getItem("selectedDates"));
    $("#dataTable").LoadingOverlay("show");

    dataTable = $("#dataTable").DataTable({
        "ajax": {
            "url": `/Payroll/PayrollConfig/GetAllOverTimes?startDate=${period.start}&endDate=${period.end}`,
        },
        "columns": [
            {
                "data": "employee",
                "render": function (data) {
                    return `${data.firstName} ${data.lastName}`
                },
                "width": "30%"
            },
            {
                "data": "employee.position.organizationDepartment.name",
                "width": "30%"
            },
            {
                "data": "dayOvertime",
                "className": "align-middle text-center",
                "width": "10%"
            },
            {
                "data": "nightOvertime",
                "className": "align-middle text-center",
                "width": "10%"
            },
        ],
        "responsive": "true",
        "initComplete": function (settings, json) {
            $("#dataTable").LoadingOverlay("hide");
        }
    })
}

function setPeriod() {
    let period = JSON.parse(localStorage.getItem("selectedDates"));
    let startDate = new Date(period.start + 'T00:00:00');
    let endDate = new Date(period.end + 'T00:00:00');

    let dayStart = startDate.getDate();
    let dayEnd = endDate.getDate();
    let month = startDate.toLocaleString('es-ES', { month: 'long' }); 
    let year = startDate.getFullYear();

    $("#txtStart").text(("0" + dayStart).slice(-2)); 
    $("#txtEnd").text(("0" + dayEnd).slice(-2))
    $("#txtMonth").text(month);
    $("#txtYear").text(year);

}

function btnAdd() {
    let period = JSON.parse(localStorage.getItem("selectedDates"));
    $('#btn-save-').on('click', function (event) {
        event.preventDefault();
        // Construir la URL con los parámetros y redirigir
        window.location.href = '/Payroll/Payroll/Upsert?start=' + period.start + '&end=' + period.end;
    });
}