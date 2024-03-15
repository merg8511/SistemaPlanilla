let dataTable;

$(document).ready(function () {
    loadDataTable();
});

document.addEventListener('DOMContentLoaded', function () {
    var passwordInput = document.getElementById('password');
    var confirmPasswordInput = document.getElementById('confirmPassword');
    var passwordStrengthProgress = document.getElementById('passwordStrengthProgress');
    var passwordStrengthText = document.getElementById('passwordStrengthText');
    var passwordMatchText = document.getElementById('passwordMatchText');

    function updatePasswordStrength() {
        var strength = checkPasswordStrength(passwordInput.value);
        var percentage = strength * 20; // Convertir a un porcentaje
        passwordStrengthProgress.style.width = percentage + '%';

        if (percentage < 40) {
            passwordStrengthProgress.className = 'progress-bar bg-danger';
            passwordStrengthText.innerHTML = 'Weak - Add more characters, numbers, and symbols';
        } else if (percentage < 60) {
            passwordStrengthProgress.className = 'progress-bar bg-warning';
            passwordStrengthText.innerHTML = 'Moderate - Add more numbers and symbols';
        } else if (percentage < 80) {
            passwordStrengthProgress.className = 'progress-bar bg-info';
            passwordStrengthText.innerHTML = 'Good - Add a symbol to make stronger';
        } else {
            passwordStrengthProgress.className = 'progress-bar bg-success';
            passwordStrengthText.innerHTML = '';
        }
        checkPasswordMatch();
    }

    function checkPasswordMatch() {
        if (confirmPasswordInput.value !== passwordInput.value) {
            passwordMatchText.innerHTML = 'Passwords do not match.';
            passwordMatchText.className = 'text-danger';
        } else {
            passwordMatchText.innerHTML = 'Passwords match.';
            passwordMatchText.className = 'text-success';
        }
    }

    passwordInput.oninput = updatePasswordStrength;
    confirmPasswordInput.oninput = checkPasswordMatch;
});


function checkPasswordStrength(password) {
    var strength = 0;
    if (password.length >= 8) strength++;
    if (/[a-z]+/.test(password)) strength++;
    if (/[A-Z]+/.test(password)) strength++;
    if (/[0-9]+/.test(password)) strength++;
    if (/[\W]+/.test(password)) strength++;
    return strength;
}


function loadDataTable() {
    $("#dataTable").LoadingOverlay("show");

    dataTable = $("#dataTable").DataTable({
        "ajax": {
            "url": "/Admin/User/GetAll",
        },
        "columns": [
            { "data": "userName", "width": "20%" },
            { "data": "email", "width": "25%" },
            { "data": "phoneNumber", "width": "25%" },
            {
                "data": {
                    id: "id", lockoutEnd: "lockoutEnd"
                },
                "render": function (data) {
                    let today = new Date().getTime();
                    let block = new Date(data.lockoutEnd).getTime();

                    if (block > today) {
                        return `
                        <div class="text-center">
                            <a onclick=BlockUnblock('${data.id}') class="btn btn-danger text-white" style="cursor:pointer", width:150px>
                                <i class="bi bi-unlock-fill"></i> Desbloquear
                            </a>
                        </div>
                    `;
                    } else {
                        return `
                        <div class="text-center">
                            <a onclick=BlockUnblock('${data.id}') class="btn btn-success text-white" style="cursor:pointer",width:150px>
                                <i class="bi bi-lock-fill"></i> Bloquear
                            </a>
                        </div>
                    `;
                    }
                }
            },
            {
                "data": "id",
                "className": "align-middle text-center",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a href="/Admin/User/Upsert/${data}" class="btn btn-sm btn-info text-white" style="cursor:pointer" title="Update user"> 
                                <i class="bi bi-pencil-square" style="font-size:0.8rem;"></i>
                            </a>
                            <a onclick=Delete("/Admin/User/Delete/${data}") class="btn btn-sm btn-danger text-white" style="cursor:pointer" title="Delete user"> 
                                <i class="bi bi-trash3-fill" style="font-size:0.8rem;"></i>
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

function BlockUnblock(id) {

    $.ajax({
        type: "POST",
        url: '/Admin/User/BlockAndUnBlock',
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                dataTable.ajax.reload();
            }
            else {
                toastr.error(data.message);
            }
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

