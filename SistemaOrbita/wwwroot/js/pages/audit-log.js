$(document).ready(function () {
    $("#btn-save").on('click', function () {
        var userId = $("#users_id option:selected").text();
        var startDate = $("#start_date").val();
        var endDate = $("#end_date").val();

        if ((startDate === "" && endDate !== "") || (startDate !== "" && endDate === "")) {
            swal("Error", "Both dates must be selected or both must be empty. \n If you leave both empty all data will be displayed", "error");
            return; 
        }

        var data = {
            UserId: userId === "All users" ? null : userId,
            StartDate: startDate || null,
            EndDate: endDate || null
        }


        $.ajax({
            url: '/Admin/Audit/FilteredLogs',
            type: 'GET',
            data: data,
            success: function (response) {
                loadDataTable(response);
            },
            error: function (xhr, status, error) {
                console.log(error)
            }

        });
    })
});

function loadDataTable(response) {
    $("#dataTable").LoadingOverlay("show");

    var transformedResponse = transformResponse(response.response);

    dataTable = $("#dataTable").DataTable({
        "destroy": true,
        "data": transformedResponse,
        "columns": [
            { "data": "userId", "width": "16%" },     
            {
                "data": "changeDate",
                "className": "align-middle text-center",
                "render": function (data, type, row) {
                    return new Date(data).toLocaleDateString('es')
                },
                "width": "16%"
            },
            { "data": "entityType", "width": "20%" },
            { "data": "actionType", "width": "16%" },
            { "data": "changes", "width": "16%" },     
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

function transformResponse(response) {
    return response.map(function (item) {
        // Extracción del nombre del controlador
        var parts = item.entityType.split(".");
        var controllerFullName = parts[parts.length - 1];
        var desiredName = controllerFullName.replace("Controller", "");
        item.entityType = desiredName;

        // Extracción del nombre del método
        var actionType = item.actionType.split(" ")[0].split(".").pop(); 
        item.actionType = actionType; 

        return item;
    });
}
