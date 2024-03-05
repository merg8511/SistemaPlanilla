let dataTable;

$(document).ready(function () {

    loadDataTable();
});

function loadDataTable() {
    $("#dataTable").LoadingOverlay("show");

    dataTable = $("#dataTable").DataTable({
        "ajax": {
            "url": "/BusinessManagement/OrganizationDepartment/GetAll",
        },
        "columns": [
            { "data": "name", "width": "20%" },
            { "data": "description", "width":"15%"},
            { "data": "createdBy", "width": "10%" },
            {
                "data": "createdAt",
                "render": function (data, type, row) {
                    return new Date(data).toLocaleDateString('es')
                },
                "width": "10%"
            },
            {
                "data": "updatedAt",
                "render": function (data, type, row) {
                    if (data) {
                        return new Date(data).toLocaleDateString('es')
                    } else {
                        return ""
                    }
                },
                "width": "10%"
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
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a href="/BusinessManagement/OrganizationDepartment/Upsert/${data}" class="btn btn-sm btn-info text-white" style="cursor:pointer" title="Update Organization Department"> 
                                <i class="bi bi-pencil-square" style="font-size:0.8rem;"></i>
                            </a>
                            <a onclick=Delete("/BusinessManagement/OrganizationDepartment/Delete/${data}") class="btn btn-sm btn-danger text-white" style="cursor:pointer" title="Delete Organization Department"> 
                                <i class="bi bi-trash3-fill" style="font-size:0.8rem;"></i>
                            </a>
                        </div>
                    `
                },
                "width": "10%"
            }
        ],
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

function Delete(url) {
    swal({
        title: "Are you sure you want to delete this event?",
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