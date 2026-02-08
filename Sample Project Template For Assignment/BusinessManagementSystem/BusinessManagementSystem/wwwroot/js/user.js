$("body").on("click", "#btnAdd", function () {
    //Reference the Name and Country TextBoxes.
    var txtRelationWithEmployee = $("#ddlRelationWithEmployee");
    var txtDateOfBirth = $("#txtDateOfBirth");
    var txtFullName = $("#txtFullName");
    var txtGender = $("#txtGender");

    //Get the reference of the Table's TBODY element.
    var tBody = $("#tblFamilyDetail > TBODY")[0];

    //Add Row.
    var row = tBody.insertRow(-1);

    //Add RelationWithEmployee cell.
    var cell = $(row.insertCell(-1));
    cell.html(txtRelationWithEmployee.val());

    //Add DateOfBirth cell.
    cell = $(row.insertCell(-1));
    cell.html(txtDateOfBirth.val());

    //Add FullName cell.
    cell = $(row.insertCell(-1));
    cell.html(txtFullName.val());

    //Add Gender cell.
    cell = $(row.insertCell(-1));
    cell.html(txtGender.val());

    //Add Button cell.
    cell = $(row.insertCell(-1));
    var btnRemove = $("<input />");
    btnRemove.attr("type", "button");
    btnRemove.attr("onclick", "Remove(this);");
    btnRemove.val("Remove");
    cell.append(btnRemove);

    //Clear the TextBoxes.
    txtName.val("");
    txtCountry.val("");
});

function Remove(button) {
    //Determine the reference of the Row using the Button.
    var row = $(button).closest("TR");
    var name = $("TD", row).eq(0).html();
    if (confirm("Do you want to delete: " + name)) {
        //Get the reference of the Table.
        var table = $("#tblFamilyDetail")[0];

        //Delete the Table row using it's Index.
        table.deleteRow(row[0].rowIndex);
    }
};

$("body").on("click", "#btnSave", function () {
    //Loop through the Table rows and build a JSON array.
    var customers = new Array();
    $("#tblFamilyDetail TBODY TR").each(function () {
        var row = $(this);
        var customer = {};
        customer.Name = row.find("TD").eq(0).html();
        customer.Country = row.find("TD").eq(1).html();
        customers.push(customer);
    });

    //Send the JSON array to Controller using AJAX.
    $.ajax({
        type: "POST",
        url: "/Home/InsertCustomers",
        data: JSON.stringify(customers),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (r) {
            alert(r + " record(s) inserted.");
        }
    });

});