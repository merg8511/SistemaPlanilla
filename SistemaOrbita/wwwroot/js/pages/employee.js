$(function () {
    loadDataTable();
    initializeSelect2ForMunicipality();
    setupSliderAndInput();
});

const initializeSelect2ForMunicipality = () => {
    const $municipality = $('#municipality');
    $municipality.select2({
        theme: "bootstrap-5",
        allowClear: true,
        width: '100%',
        placeholder: "Select a municipality",
        dropdownParent: $municipality.parent()
    });

    $(document).on('select2:open', () => {
        document.querySelector('.select2-search__field').focus();
    });
};

const setupSliderAndInput = () => {
    const slider = document.getElementById('salarySlider');
    const input = document.getElementById('salaryInput');
    const minSalaryDisplay = document.getElementById('minSalary');
    const maxSalaryDisplay = document.getElementById('maxSalary');
    const salaryError = document.getElementById('salaryError');
    const positionSelect = document.getElementById('positionSelect');

    if (positionSelect) {
        positionSelect.onchange = handlePositionChange(slider, input, minSalaryDisplay, maxSalaryDisplay, salaryError);
        slider.oninput = handleSliderInput(input, salaryError);
        input.onchange = handleInputSalaryChange(slider, salaryError);
    }


};

const handlePositionChange = (slider, input, minSalaryDisplay, maxSalaryDisplay, salaryError) => (event) => {
    const selectedOption = event.target.options[event.target.selectedIndex];
    const minSalary = selectedOption.getAttribute('data-min-salary');
    const maxSalary = selectedOption.getAttribute('data-max-salary');

    slider.min = minSalary;
    slider.max = maxSalary;
    slider.value = minSalary;
    input.value = minSalary;

    minSalaryDisplay.textContent = formatCurrency(minSalary);
    maxSalaryDisplay.textContent = formatCurrency(maxSalary);
    salaryError.style.display = 'none';
};

const handleSliderInput = (input, salaryError) => (event) => {
    input.value = event.target.value;
    salaryError.style.display = 'none';
};

const handleInputSalaryChange = (slider, salaryError) => (event) => {
    const value = parseFloat(event.target.value);
    const min = parseFloat(slider.min);
    const max = parseFloat(slider.max);

    if (value < min || value > max) {
        salaryError.style.display = 'block';
        event.target.value = slider.value;
    } else {
        salaryError.style.display = 'none';
        slider.value = value;
    }
};

const formatCurrency = (value) => `$${parseFloat(value).toFixed(2)}`;

function loadDataTable() {
    $("#dataTable").LoadingOverlay("show");

    dataTable = $("#dataTable").DataTable({
        "ajax": {
            "url": "/BusinessManagement/Employee/GetAll",
        },
        "columns": [
            {
                "data": null,
                "render": function (data, type, row) {
                    return row.firstName + ' ' + row.lastName;
                },
                "width": "20%"
            },
            { "data": "email", "width": "15%" },
            { "data": "position.title", "width": "10" },
            {
                "data": "salary",
                "className": "text-end",
                "render": function (data) {
                    var d = data.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');
                    return d;
                },
                "width": "10%"
            },
            {
                "data": "birthday",
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
                            <a href="/BusinessManagement/Employee/Details/${data}" class="btn btn-sm btn-secondary text-white" style="cursor:pointer" title="Employee details">
                                <i class="bi bi-card-checklist"></i>
                            </a>
                            <a href="/BusinessManagement/Employee/Upsert/${data}" class="btn btn-sm btn-info text-white" style="cursor:pointer" title="Update employee"> 
                                <i class="bi bi-pencil-square" style="font-size:0.8rem;"></i>
                            </a>
                            <a onclick=Delete("/BusinessManagement/Employee/Delete/${data}") class="btn btn-sm btn-danger text-white" style="cursor:pointer" title="Delete employee"> 
                                <i class="bi bi-trash3-fill" style="font-size:0.8rem;"></i>
                            </a>
                        </div>
                    `
                },
                "width": "15%"
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