$(document).ready(function () {
    // Show Employee Table
    $('#employee-table').DataTable({
        ajax: {
            url: "https://localhost:7290/api/Employee",
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
            { "data": "nik" },
            { "data": "firstName" },
            { "data": "lastName" },
            {
                "data": "birthDate",
                render: function (data, type, row) {
                    return `${formatDate(row.birthDate)}`;
                }
            },
            { "data": "gender" },
            {
                "data": "hiringDate",
                render: function (data, type, row) {
                    return `${formatDate(row.hiringDate)}`;
                }
            },
            { "data": "email" },
            { "data": "phoneNumber" },
            {
                "data": null,
                render: function (data, type, row) {
                    return `<button id="editButton" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#editEmployeeModal"><i class="ti ti-edit"></i></button>
                            <button id="deleteButton" class="btn btn-danger"><i class="ti ti-trash"></i></button>`;
                }
                //render: function (data, type, row) {
                //    return `<button id="editButton" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#editEmployeeModal"><i class="ti ti-edit"></i></button>
                //            <button id="deleteButton" class="btn btn-danger" data-bs-toggle="modal" data-bs-target="#deleteModal" onclick="deleteEmployee('${row.guid}')"><i class="ti ti-trash"></i></button>`;
                //}
            }
        ]
    });

    // Add Employee
    $("#submitEmployee").click(function () {
        submitForm();
        return false;
    });

    // Edit Employee
    $('#employee-table').on('click', '#editButton', function () {
        // Ambil data employee dari baris yang sesuai
        var data = $('#employee-table').DataTable().row($(this).parents('tr')).data();

        // Form value pre-filling
        $('#editNik').val(data.nik);
        $('#editFirstName').val(data.firstName);
        $('#editLastName').val(data.lastName);
        $('#editEmail').val(data.email);
        $('#editPhoneNumber').val(data.phoneNumber);
        if (data.gender === "Female") {
            $("#editGender option[value=0]").prop("selected", true);
        } else {
            $("#editGender option[value=1]").prop("selected", true);
        }
        let birth = data.birthDate.split('T')[0]; // Memotong string untuk mendapatkan tanggal dengan format html
        $("#editBirthDate").val(birth);
        let hiring = data.hiringDate.split('T')[0];
        $("#editHiringDate").val(hiring);

        // Ketika click submit edit
        $('#editSubmitEmployee').off('click').on('click', function () {
            Swal.fire({
                title: 'Do you want to save the changes?',
                showDenyButton: true,
                showCancelButton: true,
                confirmButtonText: 'Save',
                denyButtonText: `Don't save`,
            }).then((result) => {
                /* Read more about isConfirmed, isDenied below */
                if (result.isConfirmed) {
                    let editedData = {
                        firstName: $('#editFirstName').val(),
                        lastName: $('#editLastName').val(),
                        birthDate: $('#editBirthDate').val(),
                        hiringDate: $('#editHiringDate').val(),
                        gender: parseInt($('#editGender').val()),
                        email: $('#editEmail').val(),
                        phoneNumber: $('#editPhoneNumber').val(),
                        guid: data.guid, // GUID employee yang akan diubah
                        nik: $('#editNik').val()
                    };

                    // Send request ke API
                    $.ajax({
                        url: 'https://localhost:7290/api/Employee',
                        method: 'PUT',
                        contentType: 'application/json',
                        data: JSON.stringify(editedData)
                    }).done((result) => {
                        // Hide Modal
                        $("#editEmployeeModal").modal('hide');

                        // Refresh table
                        $('#employee-table').DataTable().ajax.reload();

                        // Success Alert
                        Swal.fire({
                            icon: 'success',
                            title: result.message,
                            showConfirmButton: false,
                            timer: 1300
                        });
                        //Swal.fire('Saved!', result.message, 'success');
                        //$("#successAlert").text(result.message).show();
                    }).fail((jqXHR, textStatus, errorThrown) => {
                        let alertMsg = jqXHR.responseJSON.message;

                        // Fail Alert
                        Swal.fire('Employee data failed to update', alertMsg, 'warning');
                        //$("#edit-fail").text(alertMsg).show();
                    });
                } else if (result.isDenied) {
                    Swal.fire('Changes are not saved', '', 'info')
                }
            })

            return false;
        });
    });

    // Delete Employee
    $('#employee-table').on('click', '#deleteButton', function () {
        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {
                // Ambil data employee dari baris yang sesuai
                var data = $('#employee-table').DataTable().row($(this).parents('tr')).data();
                let guid = data.guid;
                // send DELETE req with 'guid' to API
                $.ajax({
                    url: `https://localhost:7290/api/Employee/${guid}`,
                    type: 'DELETE'
                }).done((result) => {
                    // Hide Modal
                    //$("#deleteModal").modal('hide');

                    // Refresh table
                    $('#employee-table').DataTable().ajax.reload();

                    // Success Alert
                    Swal.fire({
                        icon: 'success',
                        title: result.message,
                        showConfirmButton: false,
                        timer: 1300
                    });
                    //Swal.fire(
                    //    'Deleted!',
                    //    'Your file has been deleted.',
                    //    'success'
                    //);
                    //$("#successAlert").text(result.message).show();
                }).fail((jqXHR, textStatus, errorThrown) => {
                    // Fail Alert
                    Swal.fire(
                        'Failed!',
                        jqXHR.responseJSON.message,
                        'error'
                    );
                    //$("#failAlert").text(jqXHR.responseJSON.message).show();
                });
            }
        })
    });
});

// Convert format date
function formatDate(inputDate) {
    const options = { year: 'numeric', month: 'short', day: 'numeric' };
    const date = new Date(inputDate);
    return date.toLocaleDateString('en-GB', options);
}

// Function to handle form add employee
function submitForm() {
    let emp = {};
    emp.firstName = $("#firstName").val();
    emp.lastName = $("#lastName").val();
    emp.gender = parseInt($("#gender").val());
    emp.email = $("#email").val();
    emp.birthDate = $("#birthDate").val();
    emp.hiringDate = $("#hiringDate").val();
    emp.phoneNumber = $("#phone-number").val();
    let jsonString = JSON.stringify(emp);

    $.ajax({
        url: "https://localhost:7290/api/Employee",
        type: "POST",
        cache: false,
        data: jsonString,
        contentType: "application/json"
    }).done((result) => {
        // Hide Modal
        $("#employee-modal").modal('hide');

        // Refresh table
        $('#employee-table').DataTable().ajax.reload();

        // Success Alert
        Swal.fire({
            icon: 'success',
            title: result.message,
            showConfirmButton: false,
            timer: 1300
        });

        //$("#successAlert").text(result.message).show();
    }).fail((jqXHR, textStatus, errorThrown) => {
        let alertMsg = jqXHR.responseJSON.message;

        // Fail Alert
        Swal.fire('Failed to create', alertMsg, 'warning');
        //$("#add-fail").text(alertMsg).show();
    });
}

// Function to handle delete confirmation
function deleteEmployee(guid) {
    // Delete confirmation button
    $('#deleteEmployee').click(function () {
        // send DELETE req with 'guid' to API
        $.ajax({
            url: `https://localhost:7290/api/Employee/${guid}`,
            type: 'DELETE'
        }).done((result) => {
            // Hide Modal
            $("#deleteModal").modal('hide');

            // Refresh table
            $('#employee-table').DataTable().ajax.reload();

            // Success Alert
            $("#successAlert").text(result.message).show();
        }).fail((jqXHR, textStatus, errorThrown) => {
            // Fail Alert
            $("#failAlert").text(jqXHR.responseJSON.message).show();
        });
    });
}

//$(document).ready(function () {
//    $('#submitEmployee').click(() => {
//        Create();
//    });
//});

//function Create() {
//    console.log(firstName, lastName);
//}

//$("#poke-table").DataTable({
//    ajax: {
//        url: "https://pokeapi.co/api/v2/pokemon/",
//        dataSrc: "results",
//        dataType: "JSON"
//    },
//    columns: [
//        { data: "name" },
//        { data: "name" },
//        {
//            data: "url",
            //render: function (data, type, row) {
            //    return `<button type="button" onclick="detail('${row.url}')" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#modalPoke">Detail</button>`;
            //}
//        }
//    ]
//});

//$.ajax({
//    url: "https://pokeapi.co/api/v2/pokemon/",
//    //success: (result) => {
//    //    console.log(result);
//    //}
//}).done((result) => {
//    let temp = "";
//    $.each(result.results, (key, value) => {
//        temp += `<tr>
//                    <td>${key + 1}</td>
//                    <td>${value.name}</td>
//                    <td><button type="button" onclick="detail('${value.url}')" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#modalPoke">Detail</button></td>
//                </tr>`;
//    });
//    $("#tBodyPoke").html(temp);
//}).fail((error) => {
//    console.log(error);
//})

//function detail(stringUrl) {
//    $.ajax({
//        url: stringUrl
//    }).done((res) => {
//        // Fungsi untuk format judul
//        function convertToTitleCase(str) {
//            return str
//                .split('-') // Membagi string berdasarkan dash
//                .map(function (word) {
//                    return word.charAt(0).toUpperCase() + word.slice(1); // Ubah huruf pertama menjadi besar
//                })
//                .join(' '); // Gabungkan kembali kata-kata dengan spasi
//        }

//        // Pokemon Name
//        $(".modal-title").html(res.name.toUpperCase());

//        // Pokemon image
//        $("#gambarPoke").attr('src', res.sprites.other['official-artwork'].front_default);

//        // Pokemon Bar Status
//        const stats = res.stats;

//        let progressBar = "";

//        let i = 0; // untuk perbedaan warna badge tiap 3 type
//        $.each(stats, (key, value) => {
//            const baseStat = value.base_stat;
//            const statName = value.stat.name;

//            // Tambahkan progress bar dengan nilai `base_stat` ke dalam kontainer.
//            if (i === 0) {
//                progressBar += `<label for="progress-bar">${convertToTitleCase(statName)} :</label>
//                            <div class="progress mb-3" role="progressbar" aria-label="Warning example" aria-valuenow="${baseStat}" aria-valuemin="0" aria-valuemax="100">
//                                <div class="progress-bar bg-danger" style="width: ${baseStat}%"></div>
//                            </div>`;
//            } else if (i === 1) {
//                progressBar += `<label for="progress-bar">${convertToTitleCase(statName)} :</label>
//                            <div class="progress mb-3" role="progressbar" aria-label="Warning example" aria-valuenow="${baseStat}" aria-valuemin="0" aria-valuemax="100">
//                                <div class="progress-bar bg-success" style="width: ${baseStat}%"></div>
//                            </div>`;
//            } else if (i === 2) {
//                progressBar += `<label for="progress-bar">${convertToTitleCase(statName)} :</label>
//                            <div class="progress mb-3" role="progressbar" aria-label="Warning example" aria-valuenow="${baseStat}" aria-valuemin="0" aria-valuemax="100">
//                                <div class="progress-bar bg-warning" style="width: ${baseStat}%"></div>
//                            </div>`;
//            } else if (i === 3) {
//                progressBar += `<label for="progress-bar">${convertToTitleCase(statName)} :</label>
//                            <div class="progress mb-3" role="progressbar" aria-label="Warning example" aria-valuenow="${baseStat}" aria-valuemin="0" aria-valuemax="100">
//                                <div class="progress-bar bg-primary" style="width: ${baseStat}%"></div>
//                            </div>`;
//            } else if (i === 4) {
//                progressBar += `<label for="progress-bar">${convertToTitleCase(statName)} :</label>
//                            <div class="progress mb-3" role="progressbar" aria-label="Warning example" aria-valuenow="${baseStat}" aria-valuemin="0" aria-valuemax="100">
//                                <div class="progress-bar bg-dark" style="width: ${baseStat}%"></div>
//                            </div>`;
//            } else if (i === 5) {
//                progressBar += `<label for="progress-bar">${convertToTitleCase(statName)} :</label>
//                            <div class="progress mb-3" role="progressbar" aria-label="Warning example" aria-valuenow="${baseStat}" aria-valuemin="0" aria-valuemax="100">
//                                <div class="progress-bar bg-secondary" style="width: ${baseStat}%"></div>
//                            </div>`;
//            } i++;
//        });
//        $("#poke-stats").html(progressBar);

//        // Pokemon Types
//        const types = res.types;
//        let pokeTypes = "";
//        i = 0; // untuk perbedaan warna badge tiap 3 type
//        $.each(types, (key, value) => {
//            if (i === 0) {
//                pokeTypes += `<h4><span class="badge rounded-pill bg-success mx-1">${value.type.name}</span></h4>`;
//            } else if (i === 1) {
//                pokeTypes += `<h4><span class="badge rounded-pill bg-danger mx-1">${value.type.name}</span></h4>`;
//            } else if (i === 2) {
//                pokeTypes += `<h4><span class="badge rounded-pill bg-warning mx-1">${value.type.name}</span></h4>`;
//            }
//            i = (i + 1) % 3;
//        });
//        console.log(pokeTypes);
//        $("#poke-types").html(pokeTypes);

//        // Pokemon Moves
//        const moves = res.moves;
//        let pokeMoves = "";
//        $.each(moves, (key, value) => {
//            pokeMoves += `<h4 style="display:inline;"><span class="badge bg-dark mx-1">${value.move.name}</span></h4>`;
//        });
//        $("#poke-moves").html(pokeMoves);
//    }).fail((error) => {
//        console.log(error);
//    })
//}