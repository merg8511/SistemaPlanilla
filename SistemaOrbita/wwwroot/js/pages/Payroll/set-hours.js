$(function () {
    let dataTable;
    initializeEmployeeSelect();
    saveSelected();
    loadDataTable();
    updateOvertime();
    showPeriodDate();
})

function initializeEmployeeSelect() {
    $("#employee-select").select2({
        width: '100%',
        closeOnSelect: false,
        multiple: true,
        theme: "bootstrap-5",
        placeholder: "Selecione empleados",
    });
}

function saveSelected() {
    $("#btn-add").off('click').on('click', function () {
        
        let selectedValues = $("#employee-select").val()
        let dOvertime = $("#dOvertime").val()
        let nOvertime = $("#nOvertime").val()
        let period = JSON.parse(localStorage.getItem("selectedDates"));
        console.log("selectedValues: " + selectedValues);
        console.log("dOvertime: " + dOvertime);
        console.log("nOvertime: " + nOvertime);
        console.log("period: " + period);

        if (!selectedValues || selectedValues.length === 0) {
            toastr.error("You must select at least one employee", 'Error');
            return false;
        }

        if (dOvertime < 0 || nOvertime < 0) {
            toastr.error("Overtime can’t be negative numbers", 'Error');
            return false;
        }

        $.ajax({
            url: '/Payroll/PayrollConfig/AddOverTime',
            type: 'POST',
            data: {
                employeeIds: selectedValues.toString(),
                dayOvertime: dOvertime,
                nightOvertime: nOvertime,
                startDate: period.start,
                endDate: period.end
            },
            success: function (response) {
                if (response.success) {
                    toastr.success('Overtime successfuly added.');
                    dataTable.ajax.reload();
                    clearInputs();
                    removeOptions(selectedValues);
                }
            },
            error: function (error) {
                toastr.error('An error ocurred while added overtime');
            }
        })

    })
}

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
                    employeeId = data.id;
                    employeeName = `${data.firstName} ${data.lastName}`
                    return `${data.firstName} ${data.lastName}`
                },
                "width": "30%"
            },
            {
                "data": "employee.position.organizationDepartment.name",
                "render": function (data) {
                    department = data;
                    return data
                },
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
            {
                "data": "id",
                "className": "align-middle text-center",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a onclick=Details("/Payroll/PayrollConfig/OvertimeDetails/${data}") class="btn btn-sm btn-secondary text-white" style="cursor:pointer" title="overtime details">
                                <i class="bi bi-card-checklist"></i>
                            </a>
                            <a onclick=Update("/Payroll/PayrollConfig/OvertimeUpdate/${data}") class="btn btn-sm btn-info text-white" style="cursor:pointer" title="Update record"> 
                                <i class="bi bi-pencil-square" style="font-size:0.8rem;"></i>
                            </a>
                            <a onclick="Delete('/Payroll/PayrollConfig/DeleteOvertime/${data}', '${employeeId}', '${employeeName} - ${department}')" class="btn btn-sm btn-danger text-white" style="cursor:pointer" title="Delete record">
                                <i class="bi bi-trash3-fill" style="font-size:0.8rem;"></i>
                            </a>

                        </div>
                    `
                },
                "width": "20%"
            }
        ],
        "responsive": "true",
        "initComplete": function (settings, json) {
            $("#dataTable").LoadingOverlay("hide");
            UpdateEmployeeSelectBasedDataTable()

        }
    })
}

function removeOptions(selectedValues) {
    selectedValues.forEach(function (value) {
        $("#employee-select option[value='" + value + "']").remove();
    });
    $("#employee-select").trigger('change');
}

function UpdateEmployeeSelectBasedDataTable() {
    let existingEmployeeIds = [];
    dataTable.rows().data().each(function (val, index) {
        if (val.employeeId) {
            existingEmployeeIds.push(val.employeeId);
        }
    });

    existingEmployeeIds.forEach(function (id) {
        $("#employee-select option[value='" + id + "']").remove();
    });
    $("#employee-select").trigger('change');
}

function clearInputs() {
    $("#dOvertime").val(0);
    $("#nOvertime").val(0);
}

function Details(url) {

    $.ajax({
        url: url,
        method: 'GET',
        success: function (response) {
            var data = response.data;
            $('#employee').text(`${data.employee.firstName} ${data.employee.lastName}`);
            $('#position').text(data.employee.position.title);
            $('#department').text(data.employee.position.organizationDepartment.name);
            $('#dayOvertime').text(data.dayOvertime);
            $('#nightOvertime').text(data.nightOvertime);
            $('#startPeriod').text(new Date(data.periodStart).toLocaleDateString('es-ES', {
                year: 'numeric',
                month: '2-digit',
                day: '2-digit'
            }));
            $('#endPeriod').text(new Date(data.periodEnd).toLocaleDateString('es-ES', {
                year: 'numeric',
                month: '2-digit',
                day: '2-digit'
            }));

            $('#createdBy').text(data.createdBy);
            $('#createdAt').text(new Date(data.createdAt).toLocaleDateString('es-ES', {
                year: 'numeric',
                month: '2-digit',
                day: '2-digit'
            }));
        },
    });

    $('#detailsModal').modal('show');

}

function Update(url) {

    $.ajax({
        url: url,
        method: 'GET',
        success: function (response) {
            var data = response.data;
            console.log(data)
            $('#txtotid').val(data.id);
            $('#txtemployee').val(`${data.employee.firstName} ${data.employee.lastName}`);
            $('#txtdayOvertime').val(data.dayOvertime);
            $('#txtnightOvertime').val(data.nightOvertime);
        },
    });

    $('#updateModal').modal('show');

}

function updateOvertime() {
    $("#btn-update").on('click', function (event) {
        event.preventDefault();
        var formData = $("#overtime-form").serialize();

        $.ajax({
            url: "/Payroll/PayrollConfig/UpdateOvertime",
            type: "POST",
            data: formData,
            success: function (data) {
                if (data.success) {
                    toastr.success(data.message, "Success")
                    $('#updateModal').modal('hide');
                    dataTable.ajax.reload();
                }
                else {
                    toastr.error(data.message, "Error")
                    $('#updateModal').modal('hide');
                    dataTable.ajax.reload();
                }
            },
            error: function (error) {
                console.log(error)
            }

        });
    })
}

function Delete(url, employeeId, employeeText) {
    swal({
        title: "Are you sure you want to delete this overtime?",
        text: "This record cannot be retrieved",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((del) => {
        if (del) {
            $("#dataTable tbody").LoadingOverlay("show");
            $.ajax({
                type: "POST",
                url: url,
                success: function (data) {
                    $("#dataTable tbody").LoadingOverlay("hide");
                    if (data.success) {
                        toastr.success(data.message, 'Success');
                        dataTable.ajax.reload();
                        UpdateEmployeeSelectBasedDataTable();
                        var newOption = new Option(employeeText, employeeId, false, false);
                        $('#employee-select').append(newOption).trigger('change');

                    }
                    else {
                        $("#dataTable tbody").LoadingOverlay("hide");
                        toastr.error(data.message, 'Error');
                    }
                }
            });
        }
    });
}

function showPeriodDate() {
    let period = JSON.parse(localStorage.getItem("selectedDates"));
    let startDate = new Date(period.start + 'T00:00:00');
    let endDate = new Date(period.end + 'T00:00:00');

    let dayStart = startDate.getDate();
    let dayEnd = endDate.getDate();
    let month = startDate.toLocaleString('es-ES', { month: 'long' });
    let year = startDate.getFullYear();

    $("#start-date").text(("0" + dayStart).slice(-2)); // Asegura dos dígitos para el día
    $("#end-date").text(("0" + dayEnd).slice(-2))
    $("#month").text(month);
    $("#year").text(year);
}





