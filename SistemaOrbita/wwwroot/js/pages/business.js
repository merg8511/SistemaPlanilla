let tableQuotation;
let tableIsr;

$(function () {
    loadtableQuotation();
    loadTableIsr();

    $('#btn-business').on('click', function (event) {
        event.preventDefault();
        var formData = $('#business-form').serialize();
        console.log(formData)
        $.ajax({
            url: '/Admin/Business/CreateBusiness',
            type: 'POST',
            data: formData,
            success: function (data) {
                if (data.success) {
                    toastr.success(data.message, 'Success');
                }
                else {
                    toastr.error(data.message, 'Error');
                }
            },
            error: function (error) {
                console.log('Error', error);
            }
        })
    })

    $('#btn-quotation').on('click', function (event) {
        event.preventDefault();
        var patronalPercentage = $('#patronal_percentage').val().trim();
        var employeePercentage = $('#employee_percentage').val().trim();
        var typeSelect = $('#type_select').val().trim();

        if (typeSelect === "") {
            swal("Error", "You must write a name for quotation", "error");
            typeSelect.foucs();
            return false;
        }

        if (patronalPercentage === "" || employeePercentage === "") {
            swal("Error", "You must write both percentages \n (Employer and Employee)", "error");
            return false;
        }
        var formData = $('#quotation-form').serialize();
        $.ajax({
            url: '/Admin/Business/CreateQuotation',
            type: 'POST',
            data: formData,
            success: function (data) {
                if (data.success) {
                    toastr.success(data.message, 'Success');
                    loadtableQuotation()
                    cleanInputs()
                }
                else {
                    toastr.error(data.message, 'Error');
                }
            },
            error: function (error) {
                console.log('Error', error);
            }
        })
    })

    $('#btn-isr').on('click', function (event) {
        event.preventDefault();
        var formData = $('#isr-form').serialize();
        $.ajax({
            url: '/Admin/Business/CreateIsr',
            type: 'POST',
            data: formData,
            success: function (data) {
                if (data.success) {
                    toastr.success(data.message, 'Success');
                    loadTableIsr();
                    cleanInputs()
                }
                else {
                    toastr.error(data.message, 'Error');
                }
            },
            error: function (error) {
                console.log('Error', error);
            }
        })
    })
});

function loadtableQuotation() {
    $("#table-quotation").DataTable().destroy();
    $("table-quotation").LoadingOverlay("show");

    tableQuotation = $("#table-quotation").DataTable({
        "ajax": {
            "url": "/Admin/Business/GetQuotes",
        },
        "columns": [
            { "data": "name", "width": "20%" },
            {
                "data": "employerPercentage",
                "className": "align-middle text-center",
                "render": function (data) {
                    return `${data} %`;
                },
                "width": "25%"
            },
            {
                "data": "employeePercentage",
                "className": "align-middle text-center",
                "render": function (data) {
                    return `${data} %`;
                },
                "width": "25%"
            },
            {
                "data": "id",
                "className": "align-middle text-center",
                "render": function (data, type, row) {
                    return `
                            <div class="text-center">
                                 <a onclick=EditQuote("${data}") class="btn btn-sm btn-info text-white" style="cursor:pointer" title="Edit quote"> 
                                     <i class="bi bi-pencil-square" style="font-size:0.8rem;"></i>
                                 </a>

                                <a onclick=Delete("/Admin/Business/DeleteQuotation/${data}","quotation") class="btn btn-sm btn-danger text-white" style="cursor:pointer" title="Delete event"> 
                                    <i class="bi bi-trash3-fill" style="font-size:0.8rem;"></i>
                                </a>
                            </div>
                          `
                },
                "width": "10%"
            }
        ],
        "responsive": "true",
        "dom": 'rti', // l = length changing input, B = buttons, f = filtering input, r = processing display element, t = table, i = information summary, p = pagination control

        "initComplete": function (settings, json) {
            $("#table-quotation").LoadingOverlay("hide");
        }
    })
}

function EditQuote(id) {
    var data = tableQuotation.row(function (idx, data, node) {
        return data.id === id;
    }).data();

    if (data) {
        $("#type_select").val(data.name);
        $("#patronal_percentage").val(data.employerPercentage);
        $("#employee_percentage").val(data.employeePercentage);
        $("input[name='Id']").val(id);
    }
}

function cleanInputs() {
    $("#type_select").val("");
    $("#patronal_percentage").val("");
    $("#employee_percentage").val("");
    $("#quotation-id").val("");
    $("#isr-id").val("");
    $("#from-amount").val("");
    $("#to-amount").val("");
    $("#percentage").val("");
    $("#about-excess").val("");
    $("#fixed-amount").val("");
    $("#name").val("");
}


function Delete(url, tableName) {
    swal({
        title: "Are you sure you want to delete this?",
        text: "This record cannot be retrieved",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((del) => {
        if (del) {
            $("#table-" + tableName + " tbody").LoadingOverlay("show");
            $.ajax({
                type: "POST",
                url: url,
                success: function (data) {
                    $("#table-" + tableName + " tbody").LoadingOverlay("hide");
                    if (data.success) {
                        toastr.success(data.message, 'Success');
                        if (tableName === "quotation") loadtableQuotation()
                        else loadTableIsr()
                        loadtableQuotation()
                    }
                    else {
                        $("#table-" + tableName + " tbody").LoadingOverlay("hide");
                        toastr.error(data.message, 'Error');
                    }
                }
            });
        }
    });
}

function loadTableIsr() {
    $("#table-isr").DataTable().destroy();
    $("table-isr").LoadingOverlay("show");

    tableIsr = $("#table-isr").DataTable({
        "ajax": {
            "url": "/Admin/Business/GetBrackets",
        },
        "columns": [
            { "data": "name", "width": "15%" },
            {
                "data": "fromAmount",
                "className": "align-middle text-end",
                "render": function (data) {
                    return `$${data}`;
                },
                "width": "15%"
            },
            {
                "data": "toAmount",
                "className": "align-middle text-end",
                "render": function (data) {
                    if (data != null) {
                        return `$${data}`;
                    }
                    else {
                        return "En adelante"
                    }
                },
                "width": "15%"
            },
            {
                "data": "percentage",
                "className": "align-middle text-center",
                "render": function (data) {
                    return `${data} %`;
                },
                "width": "15%"
            },
            {
                "data": "aboutExcess",
                "className": "align-middle text-end",
                "render": function (data) {
                    return `$${data}`;
                },
                "width": "10%"
            },
            {
                "data": "fixedAmount",
                "className": "align-middle text-end",
                "render": function (data) {
                    return `$${data}`;
                },
                "width": "15%"
            },
            {
                "data": "id",
                "className": "align-middle text-center",
                "render": function (data) {
                    return `
                        <div class="text-center">
                                 <a onclick=EditBracket("${data}") class="btn btn-sm btn-info text-white" style="cursor:pointer" title="Edit quote"> 
                                     <i class="bi bi-pencil-square" style="font-size:0.8rem;"></i>
                                 </a>

                                <a onclick=Delete("/Admin/Business/DeleteBrackets/${data}","isr") class="btn btn-sm btn-danger text-white" style="cursor:pointer" title="Delete event"> 
                                    <i class="bi bi-trash3-fill" style="font-size:0.8rem;"></i>
                                </a>
                        </div>
                    `
                },
                "width": "15%"
            }
        ],
        "responsive": "true",
        "dom": 'rti', // l = length changing input, B = buttons, f = filtering input, r = processing display element, t = table, i = information summary, p = pagination control

        "initComplete": function (settings, json) {
            $("#table-isr").LoadingOverlay("hide");
        }
    })
}

function EditBracket(id) {
    var data = tableIsr.row(function (idx, data, node) {
        return data.id === id;
    }).data();

    if (data) {
        $("#name").val(data.name);
        $("#from-amount").val(data.fromAmount);
        $("#to-amount").val(data.toAmount);
        $("#percentage").val(data.percentage);
        $("#about-excess").val(data.aboutExcess);
        $("#fixed-amount").val(data.fixedAmount);
        $("#isr-id").val(id);
    }
}