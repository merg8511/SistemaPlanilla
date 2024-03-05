$(document).ready(function () {
    const preselectedEmployees = $('#preselectedEmployees').data('employees');
    initializeSelect2();
    initializeDataTable();
    handlePreselectedEmployees(preselectedEmployees);
    handleMemberRemoval();

    function createMemberHTML({ selectedValue, selectedText, position, imageUrl }) {
        return `
            <div class="selected-member" data-member-id="${selectedValue}">
                <img src="${imageUrl}" alt="Profile Image" class="employee-image">
                <div class="employee-details">
                    <span class="employee-name">${selectedText}</span>
                    <span class="employee-position">${position}</span>
                </div>
                <button type="button" class="btn remove-member-btn" data-member-id="${selectedValue}">Remove</button>
            </div>`;
    }

    function addEmployeeToList({ selectedValue, selectedText, position, imageUrl }) {
        const newMemberHtml = createMemberHTML({ selectedValue, selectedText, position, imageUrl });
        $('#selectedMembers').append(newMemberHtml);
    }

    function updateSelectedEmployees() {
        const selectedIds = $('#selectedMembers .selected-member').map(function () {
            return $(this).data('member-id').toString();
        }).get();
        $('#selectedEmployeeIds').val(selectedIds.join(','));
    }

    function handlePreselectedEmployees(employees) {
        if (Array.isArray(employees) && employees.length) {
            employees.forEach(emp => {
                addEmployeeToList({
                    selectedValue: emp.Id,
                    selectedText: `${emp.FirstName} ${emp.LastName}`,
                    position: emp.Title,
                    imageUrl: emp.GenderImage
                });
                $('#searchEmployee option[value="' + emp.Id + '"]').remove();
            });
        }
    }

    function initializeSelect2() {
        $('#searchEmployee').select2().on('select2:select', function (e) {
            const { element: selectedElement } = e.params.data;
            const $selectedElement = $(selectedElement);
            const selectedValue = $selectedElement.val();
            const selectedText = $selectedElement.text();
            const position = $selectedElement.data('position');
            const imageUrl = $selectedElement.data('image-url');

            addEmployeeToList({ selectedValue, selectedText, position, imageUrl });
            updateSelectedEmployees();
            $selectedElement.remove();
            $(this).trigger('change');
        });
    }

    function handleMemberRemoval() {
        $('#selectedMembers').on('click', '.remove-member-btn', function () {
            const $parentDiv = $(this).parent('.selected-member');
            const memberId = $parentDiv.data('member-id');
            const memberName = $parentDiv.find('.employee-name').text();
            const memberPosition = $parentDiv.find('.employee-position').text();
            const memberImage = $parentDiv.find('img').attr('src');

            // Volver a añadir el empleado al select
            const newOption = new Option(memberName, memberId, false, false);
            $(newOption).data('position', memberPosition).data('image-url', memberImage);
            $('#searchEmployee').append(newOption).trigger('change');

            $parentDiv.remove();
            updateSelectedEmployees();
        });
    }

    function initializeDataTable() {
        $('#dataTable').DataTable();
    }
});

function Delete(assignmentId, url) {
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
                        var row = $("#dataTable").DataTable().row($('tr[data-id="' + assignmentId + '"]')).remove();
                        row.draw();
                        toastr.success(data.message, 'Success');
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