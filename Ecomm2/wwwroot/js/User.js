var dataTable;

$(document).ready(function () {
    loadDataTable();
})
function loadDataTable() {
    dataTable = $("#tblData").DataTable({
        "ajax":
            { "url": "/Admin/User/GetAll" },
        "columns":[
            { "data": "name", "width": "" },
            { "data": "email", "width": "" },
            { "data": "phoneNumber", "width": "" },
            { "data": "company.name", "width": "" },
            { "data": "role", "width": "" },
            {
                "data":
                {
                    id: "id", lockoutEnd: "lockoutEnd"
                },
                "render": function (data) {
                    var today = new Date().getTime();
                    var lockOut = new Date(data.lockoutEnd).getTime();
                    if (lockOut > today) {
                        // user is locked
                        return `
                            <div class="text-center">
                                <a class="btn btn-danger" onclick=LockUnlock('${data.id}')>Unlock</a>
                            </div>
                            `;
                    }
                    else {
                        // user is Unlocked
                        return `
                            <div class="text-center">
                                <a class="btn btn-success" onclick=LockUnlock('${data.id}')>Lock</a>
                            </div>
                            `;
                    }
                }
            }
            ]
    })
}
function LockUnlock(id) {
    //alert(id);

    $.ajax({
        url: "/Admin/User/LockUnlock",
        type: "POST",
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