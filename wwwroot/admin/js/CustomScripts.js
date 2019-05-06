function ToggleReadOnly(idParam) {
    if (document.getElementById(idParam).readOnly == false) {
        document.getElementById(idParam).readOnly = true;
        document.getElementById(idParam + "icon").classList.remove('fa-lock-open');
        document.getElementById(idParam + "icon").classList.add('fa-lock');
    }
    else {
        document.getElementById(idParam).readOnly = false;
        document.getElementById(idParam + "icon").classList.remove('fa-lock');
        document.getElementById(idParam + "icon").classList.add('fa-lock-open');
    }
}

function ToggleDisabled(idParam) {
    if (document.getElementById(idParam).disabled  == false) {
        document.getElementById(idParam).disabled  = true;
        document.getElementById(idParam + "icon").classList.remove('fa-lock-open');
        document.getElementById(idParam + "icon").classList.add('fa-lock');
    }
    else {
        document.getElementById(idParam).disabled  = false;
        document.getElementById(idParam + "icon").classList.remove('fa-lock');
        document.getElementById(idParam + "icon").classList.add('fa-lock-open');
    }
}

function PrepareFormForSubmit() {
    $("input:disabled").removeAttr('disabled');
    $("select:disabled").removeAttr('disabled');
}

function searchInTableFunction() {
    var input, filter, table, tr, td, i;
    input = document.getElementById("searchInTableTextField");
    filter = input.value.toUpperCase();
    table = document.getElementById("tableToSearch");
    tr = table.getElementsByTagName("tr");
    for (i = 0; i < tr.length; i++) {
        td = tr[i].getElementsByTagName("td")[0];
        if (td) {
            if (td.innerHTML.toUpperCase().indexOf(filter) > -1) {
                tr[i].style.display = "";
            } else {
                tr[i].style.display = "none";
            }
        }
    }
}