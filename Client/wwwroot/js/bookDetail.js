// Show Room Table
$('#booking-table').DataTable({
    ajax: {
        url: "/Booking/GetDetail/",
        dataSrc: "data",
        dataType: "JSON"
    },
    dom: 'Bfrtip',
    buttons: {
        buttons: [
            {
                extend: 'copy',
                className: 'btn btn-light',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'csv',
                className: 'btn btn-warning',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'excel',
                className: 'btn btn-success',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'pdf',
                className: 'btn btn-danger',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'print',
                className: 'btn btn-dark',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'colvis',
                className: 'btn btn-outline-secondary'
            }
        ],
        columnDefs: [{
            targets: -1,
            visible: false
        }],
        dom: {
            button: {
                className: 'btn'
            }
        }
    },
    columns: [
        { "data": "bookedNik" },
        { "data": "bookedBy" },
        { "data": "roomName" },
        { "data": "startDate" },
        { "data": "endDate" },
        { "data": "status" },
        { "data": "remarks" }
    ]
});