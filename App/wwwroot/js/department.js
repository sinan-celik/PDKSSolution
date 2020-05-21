
function getData() {
    var departments;

    fetch('http://localhost:60954/api/Department/GetDepartments')
        .then(response => response.json())
        .then(data => {
            departments = data;
            fillGrid(departments);

        })
        .catch(err => {
            console.error('An error ocurred', err);
        });
}

getData();

function fillGrid( departments) {
    $('#jsGrid').jsGrid({
        width: "100%",
        height: "800px",

        inserting: true,
        editing: true,
        sorting: true,
        paging: true,

        data: departments,

        onItemInserting: function (args) {

            if (args.item.DepartmentName === "") {
                args.cancel = true;
                alert("Specify the name of the item!");
            } else {
                console.log("insertion")
                console.log(args.item)
                insertData(args.item)
            }


        },

        onItemUpdating: function (args) {

            if (args.item.DepartmentName === "") {
                args.cancel = true;
                alert("Specify the name of the item!");
            } else {
                console.log("update")
                console.log(args.item)
                updateData(args.item)
            }
        },

        onItemDeleting: function (args) {

            if (args.item.Id === "") {
                args.cancel = true;
                alert("Specify the name of the item!");
            } else {
                console.log("update")
                console.log(args.item)
                deleteData(args.item)
            }

        },

        fields: [
            { name: "Id", type: "number", width: 50, readOnly: true },
            { name: "DepartmentName", type: "text", width: 50, validate: "required" },
            { type: "control" }
        ]
    });
}

function insertData(item) {

    (async () => {
        const rawResponse = await fetch('http://localhost:60954/api/Department/AddDepartment', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(item)
        })
            .then(data => {
                console.log(data.status);
                if (data.status === 200) {
                    location.reload();
                }
            })
            .catch(err => {
                console.error('An error ocurred', err);
            });
    })();
}


function updateData(item) {

    var newItem = JSON.stringify(item);
    console.log(newItem);

    (async () => {
        const rawResponse = await fetch('http://localhost:60954/api/Department/UpdateDepartments', {
            method: 'PUT',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: newItem//JSON.stringify(item)
        })
            .then(data => {
                console.log(data);
                if (data.status === 200) {
                    location.reload();
                }
            })
            .catch(err => {
                console.error('An error ocurred', err);
            });
        //const content = await rawResponse.json();

        //console.log(content);
    })();


}


function deleteData(item) {


    (async () => {
        const rawResponse = await fetch('http://localhost:60954/api/Department/DeleteDepartment/' + item.Id, {
            method: 'DELETE',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            //  body: newItem//JSON.stringify(item)
        })
            .then(data => {
                console.log(data);
                if (data.status === 200) {
                    location.reload();
                }
            })
            .catch(err => {
                console.error('An error ocurred', err);
            });
        //const content = await rawResponse.json();

        //console.log(content);
    })();


}