let dataTable;

$(function () {
    loadDataTable();
})

function loadDataTable() {
    $("#dataTable").LoadingOverlay("show");

    dataTable = $("#dataTable").DataTable({
        "ajax": {
            "url": "/Payroll/Payroll/GetAll",
        },
        "columns": [
            {
                "data": "employer.name",
                "width": "25%"
            },
            {
                "data": "startDate",
                "render": function (data, type, row) {
                    var d = new Date(data);
                    return d.toLocaleDateString('es-SV', {
                        day: '2-digit',
                        month: '2-digit',
                        year: 'numeric'
                    });
                },
                "width": "12%"
            },
            {
                "data": "endDate",
                "render": function (data, type, row) {
                    var d = new Date(data);
                    return d.toLocaleDateString('es-SV', {
                        day: '2-digit',
                        month: '2-digit',
                        year: 'numeric'
                    });
                },
                "width": "12%"
            },
            {
                "data": "totalAmount",
                "className": "text-end",
                "render": function (data, type, row) {
                    return `$ ${parseFloat(data).toFixed(2)}`
                },
                "width": "13%"
            },
            {
                "data": "createdBy",

                "width": "15%"
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
                "width": "10%"
            },
            {
                "data": "status",
                "className": "text-center",
                "render": function (data) {

                    if (data == 1) {
                        return '<span class="badge bg-primary" style="font-size:0.9rem";><i class="bi bi-cash-coin"></i> Paid</span>';
                    } else {
                        return '<span class="badge bg-secondary"  style="font-size:0.9rem";><i class="bi bi-cash-coin"></i></i> Pending</span>';
                    }
                },
                "width": "10%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a href="/Payroll/Payroll/Details/${data}" class="btn btn-sm btn-info text-white mb-3" style="cursor:pointer" title="Payroll details"> 
                               <i class="bi bi-card-checklist" style="font-size:0.8rem;"></i>
                            </a>
                             <a onclick=UpdatePayrollStatus("/Payroll/Payroll/UpdatePayrollStatus/${data}") class="btn btn-sm btn-primary text-white mb-3" style="cursor:pointer" title="Payroll activate"> 
                               <i class="bi bi-arrow-repeat" style="font-size:0.8rem;"></i> 
                            </a>                           
                            <a href="/Payroll/Payroll/PrintPayroll/${data}" class="btn btn-sm btn-secondary text-white mb-3" style="cursor:pointer" title="Print payroll"> 
                              <i class="bi bi-printer" style="font-size:0.8rem;"></i>
                            </a>
                            <a onclick=Delete("/Payroll/Payroll/Delete/${data}") class="btn btn-sm btn-danger text-white mb-3" style="cursor:pointer" title="Delete payroll"> 
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
        title: "Are you sure you want to delete this payroll?",
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

function UpdatePayrollStatus(url) {
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