
function getData() {
    var personnel, annualLeave;

    fetch('http://localhost:60954/api/AnnualLeave/GetAnnualLeave')
        .then(response => response.json())
        .then(data => {
            annualLeave = data;


            fetch('http://localhost:60954/api/Personnel/GetPersonnel')
                .then(response => response.json())
                .then(d => {
                    personnel = d;
                    fillGrid(annualLeave, personnel);

                })
                .catch(err => {
                    console.error('An error ocurred', err);
                });


        })
        .catch(err => {
            console.error('An error ocurred', err);
        });


}

getData();


function fillGrid(annualLeave, personnel) {
    $('#jsGrid').jsGrid({
        width: "100%",
        height: "800px",

        inserting: true,
        editing: true,
        sorting: true,
        paging: true,

        data: annualLeave,

        onItemInserting: function (args) {

            if (args.item.LeaveStart === "") {
                args.cancel = true;
                alert("Specify the name of the item!");
            } else {
                console.log("insertion")
                console.log(args.item)
                insertData(args.item)
            }


            // cancel insertion of the item with empty 'name' field
            if (args.item.name === "") {
                args.cancel = true;
                alert("Specify the name of the item!");
            }
        },

        onItemUpdating: function (args) {

            if (args.item.Id === "") {
                args.cancel = true;
                alert("Specify the name of the item!");
            } else {
                console.log("update")
                console.log(args.item)
                updateData(args.item)
            }


            // cancel insertion of the item with empty 'name' field
            if (args.item.name === "") {
                args.cancel = true;
                alert("Specify the name of the item!");
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


            // cancel insertion of the item with empty 'name' field
            if (args.item.name === "") {
                args.cancel = true;
                alert("Specify the name of the item!");
            }
        },

        fields: [
            { name: "Id", type: "number", width: 50, readOnly: true },
            //{ name: "Name", type: "text", width: 50, validate: "required" },
            //{ name: "Surname", type: "text", width: 50 },
            { name: "PersonnelId", type: "select", items: personnel, valueField: "Id", textField: "FullName" },
            { name: "LeaveStart", type: "text", width: 100 },
            { name: "LeaveEnd", type: "text", width: 100 },
            { type: "control" }
        ]
    });
}

function insertData(item) {

    (async () => {
        const rawResponse = await fetch('http://localhost:60954/api/AnnualLeave/AddAnnualLeave', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(item)
        })
            .then(data => {
                console.log(data);
                if (data.status === 200) {
                    location.reload();
                } else {
                    data.json().then(body => {
                        alert(body);
                        location.reload();
                    });
                    
                    //alert(data)
                    //alert('kayit olmadi')
                    //res.json().then(body => console.log(body));
                }
            })
            .catch(err => {
                console.error('An error ocurred', err);
            });

        

    })();
}


function updateData(item) {

    //delete item.DepartmentName

    var newItem = JSON.stringify(item);
    console.log(newItem);

    (async () => {
        const rawResponse = await fetch('http://localhost:60954/api/AnnualLeave/UpdateAnnualLeave', {
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
        const content = await rawResponse.json();

        console.log(content);
    })();


}


function deleteData(item) {


    (async () => {
        const rawResponse = await fetch('http://localhost:60954/api/AnnualLeave/DeleteAnnualLeave/' + item.Id, {
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