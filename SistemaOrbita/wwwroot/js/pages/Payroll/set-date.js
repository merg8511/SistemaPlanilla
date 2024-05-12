document.addEventListener('DOMContentLoaded', function () {
    const calendarEl = document.getElementById('calendar');
    let selectedStartDate = null;
    let selectedEndDate = null;

    const calendar = initializeCalendar(calendarEl);
    calendar.render();

    const storedDates = getStoredDates();
    selectDatesBasedOnStoredOrType(storedDates, calendar);

    $("#btn-save-setDate").on('click', function (event) {
        event.preventDefault();
        saveSelectedDates();

        window.location.href = "/Payroll/PayrollConfig/SetHours";
    });

    document.getElementById('clearCalendar').addEventListener('click', function () {
        localStorage.removeItem('selectedDates');
        calendar.refetchEvents();
    });
});

function initializeCalendar(calendarEl) {
    return new FullCalendar.Calendar(calendarEl, {
        plugins: ['interaction', 'dayGrid'],
        defaultView: 'dayGridMonth',
        selectable: true,
        locale: 'es',
        select: handleDateSelection
    });
}

function handleDateSelection(info) {
    let endDate = adjustEndDate(info);

    selectedStartDate = info.startStr;
    selectedEndDate = formatDate(endDate);

}

function adjustEndDate(info) {
    let endDate = new Date(info.end);

    if (info.allDay) {
        endDate.setDate(endDate.getDate() - 1);
    }

    return endDate;
}

function formatDate(date) {
    return date.toISOString().slice(0, 10);
}

function getStoredDates() {
    const storedDates = localStorage.getItem("selectedDates");
    return storedDates ? JSON.parse(storedDates) : null;
}

function selectDatesBasedOnStoredOrType(storedDates, calendar) {
    if (storedDates) {
        let endDate = new Date(storedDates.end);
        endDate.setDate(endDate.getDate() + 1); // Asegurándonos de que el rango incluya el último día
        calendar.select(storedDates.start, formatDate(endDate));
    } else {
        // Asume que la función selectRangeBasedOnType ya maneja esto correctamente
        selectRangeBasedOnType(calendar);
    }
}

function selectRangeBasedOnType(calendar) {
    const { start, end } = calculateRange(type);
    calendar.select(start, end);
}

function calculateRange(type) {
    const today = new Date();
    let start, end;

    if (type === 1) { // Mensual
        start = new Date(today.getFullYear(), today.getMonth(), 1);
        end = new Date(today.getFullYear(), today.getMonth() + 1, 0); // Último día del mes
        end.setDate(end.getDate() + 1); // Ajuste clave para incluir el último día en la selección
    } else { // Quincenal
        if (today.getDate() <= 15) {
            start = new Date(today.getFullYear(), today.getMonth(), 1);
            end = new Date(today.getFullYear(), today.getMonth(), 16);
        } else {
            start = new Date(today.getFullYear(), today.getMonth(), 16);
            end = new Date(today.getFullYear(), today.getMonth() + 1, 0);
        }
    }

    return {
        start: formatDate(start),
        end: formatDate(end)
    };
}

function saveSelectedDates() {
    const dates = { start: selectedStartDate, end: selectedEndDate };
    localStorage.setItem("selectedDates", JSON.stringify(dates));

    const test = JSON.parse(localStorage.getItem("selectedDates"));
}
