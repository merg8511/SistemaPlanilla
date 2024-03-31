let dataTable;

$(function () {
    loadDataTable();
    initialSelect2();
    initializeEventSelect();
    handleDateInputs();
    updateRecurringValue();
});

function loadDataTable() {
    $("#dataTable").LoadingOverlay("show");

    dataTable = $("#dataTable").DataTable({
        "ajax": {
            "url": "/Payroll/EventLog/GetAll",
        },
        "columns": [
            {
                "data": "employee",
                "render": function (data, type, row) {
                    return `${data.firstName} ${data.lastName}`
                },
                "width": "10%"
            },
            {
                "data": "event",
                "render": function (data, type, row) {
                    return `${data.name}`
                },
                "width": "10%"
            },
            {
                "data": "createdAt",
                "render": function (data, type, row) {
                    var d = new Date(data);
                    return d.toLocaleDateString('es-SV', {
                        day: '2-digit',
                        month: '2-digit',
                        year: 'numeric'
                    });
                },
                "width": "5%"
            },
            {
                "data": "amount",
                "className": "text-end",
                "render": function (data, type, row) {
                    return `$ ${parseFloat(data).toFixed(2)}`
                },
                "width": "5%"
            },
            {
                "data": "employee",
                "render": function (data, type, row) {
                    return `${data.firstName} ${data.lastName}`
                },
                "width": "10%"
            },
            {
                "data": "event.isDeduction",
                "className": "text-center",
                "render": function (data) {

                    if (data == 1) {
                        return '<span class="badge bg-danger"  style="font-size:0.9rem";> <i class="bi bi-dash-circle"></i> Deduction</span>';

                    } else {
                        return '<span class="badge bg-success" style="font-size:0.9rem";><i class="bi bi-plus-circle"></i> Bonus</span>';
                    }
                },
                "width": "5%"
            },
            {
                "data": "isActive",
                "className": "text-center",
                "render": function (data) {

                    if (data == 1) {
                        return '<span class="badge bg-success" style="font-size:0.9rem";><i class="bi bi-check-circle me-1"></i> Active</span>';
                    } else {
                        return '<span class="badge bg-danger"  style="font-size:0.9rem";><i class="bi bi-exclamation-octagon me-1"></i> Inactive</span>';
                    }
                },
                "width": "8%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a onclick=Details("/Payroll/EventLog/Details/${data}") class="btn btn-sm btn-secondary text-white" style="cursor:pointer" title="Project details">
                                <i class="bi bi-card-checklist"></i>
                            </a>
                            <a href="/Payroll/EventLog/Upsert/${data}" class="btn btn-sm btn-info text-white" style="cursor:pointer" title="Update position"> 
                                <i class="bi bi-pencil-square" style="font-size:0.8rem;"></i>
                            </a>
                            <a onclick=Delete("/Payroll/EventLog/Delete/${data}") class="btn btn-sm btn-danger text-white" style="cursor:pointer" title="Delete position"> 
                                <i class="bi bi-trash3-fill" style="font-size:0.8rem;"></i>
                            </a>
                        </div>
                    `
                },
                "width": "10%"
            }
        ],
        "responsive": "true",
        "dom": 'l<"d-flex float-right"f>Brtip', // l = length changing input, B = buttons, f = filtering input, r = processing display element, t = table, i = information summary, p = pagination control

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

function Delete(url) {
    swal({
        title: "Are you sure you want to delete this event log?",
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

function Details(url) {

    $.ajax({
        url: url,
        method: 'GET',
        success: function (response) {
            var data = response.data;
            console.log(data)
            $('#detailName').text(`Event Log: ${data.employee.firstName}`);
            $('#txtEmployeeName').text(`${data.employee.firstName} ${data.employee.lastName}`);
            $('#txtNotes').text(data.notes);
            $('#txtAuthorized').text(`${data.authorizedByNavigation.firstName} ${data.authorizedByNavigation.lastName}`);
            $('#txtAmount').text(`$ ${parseFloat(data.amount).toFixed(2)}`);
            $('#txtEvent').text(data.event.name);
            $('#txtStatus').text(data.isActive ? 'Active' : 'Inactive');
            $('#txtCreatedAt').text(new Date(data.createdAt).toLocaleDateString('es-ES', {
                year: 'numeric',
                month: '2-digit',
                day: '2-digit'
            }));

            $('#txtRecurring').text(data.recurring == 0 ? 'No' : 'Si');

            $('#txtStartDate').text(data.startDate ? new Date(data.startDate).toLocaleDateString() : 'N/A');
            $('#txtEndDate').text(data.endDate ? new Date(data.endDate).toLocaleDateString() : 'N/A');
            $('#txtFrequency').text(
                data.frequency == null ? 'N/A' :
                    data.frequency == 0 ? 'Quincenal' :
                        data.frequency == 1 ? 'Mensual' : ''
            );
        },
        error: function (xhr, status, error) {
            console.error("Error fetching project details:", status, error);
        }
    });
    $('#detailsModal').modal('show');
}

function initializeEventSelect() {
    var eventSelect = $("#eventSelect");
    var discountDiv = document.getElementById('discountDiv');
    var discountLabel = document.getElementById('discountLabel');

    eventSelect.on('select2:select', function (e) {
        if (e.params && e.params.data && e.params.data.element) {
            var selectedOptionDataType = $(e.params.data.element).attr('data-type');

            if (e.params.data.id === "") {
                discountDiv.style.display = 'none';
            } else {
                discountDiv.style.display = 'block';
                discountDiv.style.backgroundColor = selectedOptionDataType === "0" ? 'green' : 'red';
                discountDiv.style.color = 'white';
                discountLabel.innerText = selectedOptionDataType === "0" ? 'BONO' : 'DISCOUNT';
            }
        }
    });

    eventSelect.on('select2:open', () => {
        document.querySelector('.select2-search__field').focus();
    });
    eventSelect.trigger('select2:select');
}

function handleDateInputs() {
    var startDate = document.getElementById('startDate');
    var endDate = document.getElementById('endDate');

    startDate.addEventListener('change', function () {
        endDate.min = startDate.value;
    });
}

function updateRecurringValue() {
    var recurringSwitch = document.getElementById('recurringSwitch');
    var containersToShowOrHide = document.querySelectorAll('.container-recurring');

    containersToShowOrHide.forEach(function (container) {
        container.style.display = recurringSwitch.checked ? '' : 'none';
        if (!recurringSwitch.checked) {
            document.getElementById('startDate').value = '';
            document.getElementById('endDate').value = '';
        }
    });
}

function initialSelect2() {
    var eventSelect = $("#eventSelect").select2({
        width: '100%',
        theme: 'bootstrap-5',
        placeholder: 'Select event'
    });
    var employeeSelect = $("#employeeSelect").select2({
        theme: 'bootstrap-5',
        placeholder: 'Select employee'
    });

    eventSelect.on('select2:open', () => {
        document.querySelector('.select2-search__field').focus();
    });

    employeeSelect.on('select2:open', () => {
        document.querySelector('.select2-search__field').focus();
    });

}