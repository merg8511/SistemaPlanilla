$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    $("#dataTable").LoadingOverlay("show");

    dataTable = $("#dataTable").DataTable({
        "ajax": {
            "url": "/BusinessManagement/Project/GetAll",
        },
        "columns": [
            { "data": "name", "width": "20%" },
            {
                "data": "manager",
                "render": function (data, type, row) {
                    if (data) {
                        return data.firstName + ' ' + data.lastName;
                    }
                    return '';
                },
                "width": "15%"
            },
            { "data": "description", "width": "10" },
            {
                "data": "startDate",
                "render": function (data, type, row) {
                    return new Date(data).toLocaleDateString('es')
                },
                "width": "10%"
            },
            {
                "data": "endDate",
                "render": function (data, type, row) {
                    return new Date(data).toLocaleDateString('es')
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
                            <a onclick=Details("/BusinessManagement/Project/Details/${data}") class="btn btn-sm btn-secondary text-white" style="cursor:pointer" title="Project details">
                                <i class="bi bi-card-checklist"></i>
                            </a>
                            <a href="/BusinessManagement/Project/Upsert/${data}" class="btn btn-sm btn-info text-white" style="cursor:pointer" title="Update Project"> 
                                <i class="bi bi-pencil-square" style="font-size:0.8rem;"></i>
                            </a>
                            <a onclick=Delete("/BusinessManagement/Project/Delete/${data}") class="btn btn-sm btn-danger text-white" style="cursor:pointer" title="Delete Project"> 
                                <i class="bi bi-trash3-fill" style="font-size:0.8rem;"></i>
                            </a>
                        </div>
                    `
                },
                "width": "15%"
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

function Details(url) {

    $.ajax({
        url: url,
        method: 'GET',
        success: function (response) {
            var data = response.data;
            getProgressBar(data);
            $('#projectName').text(data.name);
            $('#projectDescription').text(data.description);
            $('#projectManager').text(data.manager.firstName + ' ' + data.manager.lastName);
            $('#projectStartDate').text(new Date(data.startDate).toLocaleDateString());
            $('#projectEndDate').text(new Date(data.endDate).toLocaleDateString());
            $('#projectStatus').text(data.isActive ? 'Active' : 'Inactive');
            $('#projectCreatedAt').text(new Date(data.createAt).toLocaleDateString());
            $('#projectUpdatedAt').text(data.updateAt ? new Date(data.updateAt).toLocaleDateString() : 'N/A');
        },
        error: function (xhr, status, error) {
            console.error("Error fetching project details:", status, error);
        }
    });
    $('#detailsModal').modal('show');
}

function Delete(url) {
    swal({
        title: "Are you sure you want to delete this project?",
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

function getProgressBar(data) {
    var startDate = new Date(data.startDate);
    var endDate = new Date(data.endDate);
    var currentDate = new Date();

    var totalDuration = endDate - startDate;

    var daysPassed = currentDate - startDate;

    var progressPercentage = 0;

    if (totalDuration > 0) {
        progressPercentage = (daysPassed / totalDuration) * 100;
        progressPercentage = Math.min(Math.max(progressPercentage, 0), 100);
    }

    $('#projectProgress')
        .css('width', progressPercentage + '%')
        .attr('aria-valuenow', progressPercentage)
        .text(Math.round(progressPercentage) + '%');
}