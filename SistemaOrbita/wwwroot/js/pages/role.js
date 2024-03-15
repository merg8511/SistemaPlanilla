let dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    $("#dataTable").LoadingOverlay("show");

    dataTable = $("#dataTable").DataTable({
        "ajax": {
            "url": "/Admin/Role/GetAll",
        },
        "columns": [
            {
                "data": "name",
                "className": "align-middle text-center w-50",
            },
            {
                "data": "id",
                "className": "align-middle text-center w-50",
                "render": function (data) {
                    return `
                      <div class="text-center">
                            <a href="/Admin/Role/Upsert/${data}" class="btn btn-sm btn-info text-white" style="cursor:pointer" title="Update role">
                                <i class="bi bi-pencil-square" style="font-size:0.8rem;"></i>
                            </a>
                            <a onclick=Delete("/Admin/Role/Delete/@role.Id") class="btn btn-sm btn-danger text-white" style="cursor:pointer" title="Delete employee">
                                <i class="bi bi-trash3-fill" style="font-size:0.8rem;"></i>
                            </a>
                            <a href="/Admin/Permission/Index/${data}" class="btn btn-sm btn-primary text-white" style="cursor:pointer" title="Manage role">
                                <i class="bi bi-tools"></i> Manage Permissions
                            </a>
                       </div>
                    `
                },
                "width": "10%"
            }
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

function Delete(url) {
    swal({
        title: "Are you sure you want to delete this role?",
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