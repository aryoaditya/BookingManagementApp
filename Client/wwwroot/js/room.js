// Show Room Table
$('#room-table').DataTable({
    ajax: {
        url: "/room/GetAll/",
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
        { "data": "name" },
        { "data": "floor" },
        { "data": "capacity" },

        {
            "data": null,
            render: function (data, type, row) {
                return `<button id="editButton" class="btn btn-primary"><i class="ti ti-edit"></i></button>
                        <button id="deleteButton" class="btn btn-danger"><i class="ti ti-trash"></i></button>`;
            }
        }
    ]
});