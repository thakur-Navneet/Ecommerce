var DataTable;

$(document).ready(function () {
    loadDataTable();
})

function loadDataTable() {
    DataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Category/GetAll"
        },
        "columns": [
            {
                "data": "id",
                "render": function (data) {//render  function html code to js access
                    return `
                    <div class ="text-center">
                        <a href="/Admin/Category/Upsert/${data}"class="btn btn-info">
                            <i class="fas fa-edit"></i>
                        </a>
                    </div>
                    `;
                }
            },
            { "data": "name", "width": "60%" },
            {
                "data": "id",
                "render": function (data) {//render  function html code to js access
                    return `
                    <div class ="text-center">
                        <a class="btn btn-danger" onclick = Delete("/Admin/Category/Delete/${data}")>
                            <i class="fas fa-trash-alt"></i>
                        </a>
                    </div>
                    `;
                }
            }
        ]
    })
}

function Delete(url) {
    swal({
        title: "want to delete data ?",
        text: "Delete Information !!!!",
        icon: "warning",
        buttons: true,
        dangerModel: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                url: url,
                type: "Delete",
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        DataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}